terraform {
  required_providers {
    kubernetes = {
      source = "hashicorp/kubernetes"
    }
  }
}

provider "kubernetes" {
  config_context_cluster = "docker-desktop"
  config_path = "~/.kube/config"
}

resource "kubernetes_service" "food-totem-catalog-svc" {
  metadata {
    name = "food-totem-catalog-svc"
    labels = {
      app = "food-totem-catalog-api"
    }
  }
  spec {
    type = "LoadBalancer"
    selector = {
      app = "food-totem-catalog-api"
    }
    port {
      port        = 3001
      target_port = 80
      node_port = 30002
    }
  }
}

resource "kubernetes_deployment" "food-totem-catalog-api" {
  metadata {
    name = "food-totem-catalog-deployment"
    labels = {
      app = "food-totem-catalog-api"
    }
  }
  spec {
    replicas = 2
    selector {
      match_labels = {
        app = "food-totem-catalog-api"
      }
    }
    template {
      metadata {
        name = "food-totem-catalog-api"
        labels = {
          app = "food-totem-catalog-api"
        }
      }
      spec {
        restart_policy = "Always"
        container {
          image = "leonardoavila98/food-totem-catalog-api:latest"
          name  = "food-totem-catalog-api"
          image_pull_policy = "IfNotPresent"
          port {
            container_port = 80
          }
        }
      }
    }
  }
}