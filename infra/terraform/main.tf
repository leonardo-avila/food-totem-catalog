terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
    }
  }
  required_version = ">= 1.3.7"
  backend "s3" {
    bucket                  = "terraform-buckets-food-totem"
    key                     = "food-totem-catalog/terraform.tfstate"
    region                  = "us-west-2"
  }
}
provider "aws" {
    region = var.lab_account_region
}

data "aws_security_group" "default" {
  name = "default"
}

data "aws_vpc" "default" {
  default = true
}

data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

locals {
  endpoint_parts = split(":", aws_db_instance.food-totem-catalog-mysql.endpoint)
  port = local.endpoint_parts[1]
}

resource "aws_db_instance" "food-totem-catalog-mysql" {
  engine               = "mysql"
  identifier           = "food-totem-catalog-db"
  allocated_storage    =  20
  engine_version       = "8.0.33"
  instance_class       = "db.t2.micro"
  username             = var.mysql_user
  password             = var.mysql_password
  parameter_group_name = "food-totem-mysql-parameter"
  vpc_security_group_ids = [data.aws_security_group.default.id]
  skip_final_snapshot  = true
  publicly_accessible =  true
}

resource "aws_ecs_task_definition" "food-totem-catalog-task" {
  depends_on = [ aws_db_instance.food-totem-catalog-mysql ]
  family                   = "foodtotem-catalog"
  network_mode             = "awsvpc"
  execution_role_arn       = "arn:aws:iam::${var.lab_account_id}:role/LabRole"
  cpu                      = 256
  memory                   = 512
  requires_compatibilities = ["FARGATE"]
  container_definitions    = jsonencode([
    {
        "name": "food-totem-catalog",
        "image": var.food_totem_catalog_image,
        "essential": true,
        "portMappings": [
            {
              "containerPort": 80,
              "hostPort": 80,
              "protocol": "tcp"
            }
        ],
        "environment": [
            {
                "name": "ConnectionStrings__DefaultConnection",
                "value": join("", ["Server=", element(split(":", aws_db_instance.food-totem-catalog-mysql.endpoint), 0), ";Port=", local.port, ";Database=foodtotem;Uid=", var.mysql_user, ";Pwd=", var.mysql_password, ";"])
            }
        ],
        "cpu": 256,
        "memory": 512,
        "logConfiguration": {
            "logDriver": "awslogs",
            "options": {
                "awslogs-group": "food-totem-catalog-logs",
                "awslogs-region": "us-west-2",
                "awslogs-stream-prefix": "food-totem-catalog"
            }
        }
    }
  ])
}

resource "aws_lb_target_group" "catalog-api-tg" {
  name     = "catalog-api"
  port     = 80
  protocol = "HTTP"
  vpc_id   = data.aws_vpc.default.id
  target_type = "ip"

  health_check {
    enabled = true
    interval = 300
    path = "/health-check"
    protocol = "HTTP"
    timeout = 60
    healthy_threshold = 3
    unhealthy_threshold = 3
  }
}

resource "aws_lb" "catalog-api-lb" {
  name               = "catalog-api"
  internal           = true
  load_balancer_type = "application"
  subnets            = data.aws_subnets.default.ids
}

resource "aws_lb_listener" "catalog-api-lbl" {
  load_balancer_arn = aws_lb.catalog-api-lb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.catalog-api-tg.arn
  }
}

resource "aws_ecs_service" "food-totem-catalog-service" {
  name            = "food-totem-catalog-service"
  cluster         = "food-totem-ecs"
  task_definition = aws_ecs_task_definition.food-totem-catalog-task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    security_groups  = [data.aws_security_group.default.id]
    subnets = data.aws_subnets.default.ids
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.catalog-api-tg.arn
    container_name   = "food-totem-catalog"
    container_port   = 80
  }

  health_check_grace_period_seconds = 120
}

resource "aws_cloudwatch_log_group" "food-totem-catalog-logs" {
  name = "food-totem-catalog-logs"
  retention_in_days = 1
}

