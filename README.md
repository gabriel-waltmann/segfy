# Car Insurance Policy System

## Project Structure
- `/api` - .NET 8 (Backend)
- `/client` - Vue.js / Vite (Frontend)
- `docker-compose.yml` - Containers.

## Main Functionalities
* Policy Management (CRUD): Create, read, update, and delete insurance policies.
* Customer & Vehicle Profiling: Store and manage policyholder details and associated vehicle data.
* Automated Quoting/Validation: Backend business logic to validate policy rules and calculate premium structures.
* RESTful API Design: Fully documented endpoints using Swagger UI for easy integration and testing.
* Responsive Frontend: Fast, modern single-page application built with Vue.js and Vite.
* Containerized Infrastructure: One-command setup for the entire stack (Database, API, Client) using Docker Compose.

## Prerequisites
* Docker Compose
* .NET 8 SDK
* EF Core CLI tools (`dotnet tool install --global dotnet-ef`)

## How run the application in development mode
OBS: Project was tested in a arch linux machine
1. Open your terminal
2. run:
  ```bash
  git clone git@github.com:gabriel-waltmann/segfy.git

  cd segfy
  
  docker compose up -d --build
  
  cd api

  dotnet ef database update --connection "Server=localhost;Database=CarInsuranceDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;"
  ``
  The application will be available at:
    * Swagger: http://localhost:8080/swagger

    * Api: http://localhost:8080

    * Client: http://localhost:3000

## How run backend unit tests
```bash
cd api.tests

dotnet test
``
