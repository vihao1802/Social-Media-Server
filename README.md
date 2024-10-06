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
2. **Add Required NuGet Packages from csproj file**
   ```bash
   dotnet restore
   ```

Packages would be installed:

- [Microsoft.VisualStudio.Web.CodeGeneration.Design](https://www.nuget.org/packages/Microsoft.VisualStudio.Web.CodeGeneration.Design)
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)
- [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools)
- [Microsoft.AspNetCore.Mvc.NewtonsoftJson](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.NewtonsoftJson) --version 3.0.0
- [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer) --version 8.0.8
- [Microsoft.Extensions.Identity.Core](https://www.nuget.org/packages/Microsoft.Extensions.Identity.Core) --version 8.0.8
- [Microsoft.AspNetCore.Identity.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore) --version 8.0.8

3. **Install Entity Framework CLI Tool**
   Install the global Entity Framework Core CLI tool to handle migrations and database updates:

   ```bash
   dotnet tool install --global dotnet-ef --version 8.*
   ```
4. **Run Migrations and update changes to database**
   ```bash
   dotnet-ef migrations add [name_of_migration]
   ```
   Apply the migration to the database:
   ```bash
   dotnet-ef database update
   ```
5. **Running and build the Application**
   To watch for changes and automatically rebuild the application during development, use:
   ```bash
   dotnet watch run
   ```

   
   To run the application, use:
   ```bash
   dotnet build
   ```

   ```bash
   dotnet run
   ```

