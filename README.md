# Acacia Back End

The Acacia back end supports the e-commerce platform with a .NET Web API, Docker for containerization, and SQL Server for the database. This project handles server-side logic, data management, and API endpoints.

## Features

- **.NET Web API:** Provides a robust and scalable API.
- **Docker:** Containerizes the application for consistent deployment.
- **SQL Server:** Manages and stores application data efficiently.

## Getting Started

To get started with the Acacia back end:

```bash
# Clone the repository
git clone https://github.com/Quintessential-IT/Acacia_Back_End.git

# Navigate into the project directory
cd Acacia_Back_End

# Build and start the Docker containers
docker-compose up --build

# Run database migrations
# Ensure you have a valid connection string in appsettings.json

# Run migrations for DefaultConnection
dotnet ef database update --context Context

# Run migrations for IdentityConnection
dotnet ef database update --context AppIdentityDbContext

# The API will be accessible at http://localhost:7234

# To stop the containers
docker-compose down
