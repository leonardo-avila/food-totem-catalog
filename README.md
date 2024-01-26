# Food Totem Catalog
[![build](https://github.com/leonardo-avila/food-totem-catalog/actions/workflows/build.yml/badge.svg)](https://github.com/leonardo-avila/food-totem-catalog/actions/workflows/build.yml)
[![Deploy](https://github.com/leonardo-avila/food-totem-catalog/actions/workflows/deploy.yml/badge.svg)](https://github.com/leonardo-avila/food-totem-catalog/actions/workflows/deploy.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=leonardo-avila_food-totem-catalog&metric=coverage)](https://sonarcloud.io/summary/new_code?id=leonardo-avila_food-totem-catalog)

This is a repository to maintain the catalog of Food Totem project. It is a microservice that provides the catalog of products and their prices. It is a REST API, accessible through HTTP requests by the API Gateway configured on another repository.

The complete documentation could be found on the [API Gateway repository](https://github.com/leonardo-avila/food-totem).

Besides that, it has an local version to be used on development environment. It is a simple web application that provides a user interface to manage the catalog. It is accessible through a web browser on the address http://localhost:3003/swagger.

## Requirements

- .NET 6.0 or above
- Docker

## Running the application

On the root folder:
  
  ```bash
    make run-services
  ```
