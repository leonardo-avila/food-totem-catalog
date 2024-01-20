variable "mysql_user" {
  description = "MySQL user"
}

variable "mysql_password" {
  description = "MySQL password"
}

variable "lab_account_id" {
  description = "AWS Labs account ID"
}

variable "lab_account_region" {
  description = "AWS Labs account region"
  default = "us-west-2"
}

variable "food_totem_catalog_image" {
  description = "Food Totem Catalog Docker image"
}