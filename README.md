Social Media - Server
# Project Setup

This README file provides a step-by-step guide to setting up and configuring the project. Follow the instructions below to get your development environment ready.

## Prerequisites

Ensure you have the following tools installed:
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or higher)
- A code editor such as [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

## Setup Instructions

1. **Trust the HTTPS development certificate**  
   Run the following command to trust the development certificate for secure HTTPS connections:
   ```bash
   dotnet dev-certs https --trust
   ```
2. **Add Required NuGet Packages**
Install the following NuGet packages required for the project.

Add ASP.NET Core Scaffolding Tools:

```bash

dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design 
```
Add Entity Framework Core Design Package:
```bash

dotnet add package Microsoft.EntityFrameworkCore.Design
```
Add Entity Framework Core for SQL Server:

```bash

dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```
Add Entity Framework Core Tools:
```bash

dotnet add package Microsoft.EntityFrameworkCore.Tools
```
Add Newtonsoft.Json package for JSON serialization support in MVC:

```bash

dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 3.0.0
```
3. **Install Entity Framework CLI Tool**
Install the global Entity Framework Core CLI tool to handle migrations and database updates:

```bash

dotnet tool install --global dotnet-ef --version 8.*
```
4. **Run Migrations and update changes to database**
After setting up your database context and models, run the following command to create the initial migration:

```bash

dotnet ef migrations add init
```
Update the Database
Apply the migration to the database:

```bash

dotnet ef database update
```
5. **Running and build the Application**
To run the application, use:

```bash

dotnet build
```

```bash

dotnet run
```
To watch for changes and automatically rebuild the application during development, use:

```bash

dotnet watch run
```
