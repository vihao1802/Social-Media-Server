<h1 align="center">Social Media Website</h1>
<p align="center">
  <a href="https://github.com/vihao1802/Social-Media-Client/blob/main/LICENSE">
    <img alt="License: MIT" src="https://img.shields.io/badge/license-MIT-yellow.svg" target="_blank" />
  </a>
  <a href="https://github.com/vihao1802/Social-Media-Client/watchers">
    <img alt="GitHub" src="https://img.shields.io/github/watchers/vihao1802/Website-Classin" target="_blank" />
  </a>
</p>
<p align="center">
  <img src="https://img.shields.io/badge/React-%2320232a.svg?logo=react&logoColor=%2361DAFB"/></a>&nbsp
  <img src="https://img.shields.io/badge/Material%20UI-007FFF?logo=mui&logoColor=white"/></a>&nbsp 
  <img src="https://img.shields.io/badge/Next.js-black?logo=next.js&logoColor=white"/></a>&nbsp
  <br>
  <img src="https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=white"/></a>&nbsp 
  <img src="https://img.shields.io/badge/Microsoft_SQL_Server-CC2927?logo=microsoft-sql-server&logoColor=white"/></a>&nbsp
  <br>
  <img src="https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white"/></a>&nbsp 
  <img src="https://img.shields.io/badge/TypeScript-007ACC?logo=typescript&logoColor=white"/></a>&nbsp 
</p>

> `Social Media Website` that provides an minimal UI with various features such as creat post, react post, comment, chat real-time, call video, watch stories and reels, etc.

<!-- <img src="/docs/screenshot.png" width="100%"> -->

<!-- ### 📄 PDF: <a href="" target="_blank">Link</a> -->
<!-- ### 📄 Slide: <a href="" target="_blank">Link</a> -->

## 🎉 Tech Stack

### Frontend

- React.js: JavaScript library for building dynamic, reusable UI components.
- Next.js: React framework for server-side rendering (SSR) and static site generation (SSG).
- Material UI: React component library implementing Google's Material Design for responsive UIs.
- TypeScript: JavaScript superset offering static typing for better code quality.
- Tailwind CSS: utility-first CSS framework for rapid and flexible styling.
- SWR: lightweight library for data fetching, caching, and revalidation in React.
- WebSocket: protocol enabling real-time, two-way communication between client and server.
- Stream.io: platform for building scalable real-time features like feeds, chat, and video calls.

### Backend

- ASP.NET: framework for building scalable, high-performance web applications on .NET.
- JWT (JSON Web Tokens): secure mechanism for user authentication and information exchange.
- Cloudinary: cloud-based service for managing and optimizing media assets like images and videos.
- Google and Facebook OAuth: Authentication protocols for secure user login using Google or Facebook accounts.

### Database

- SQL Server: A relational database management system by Microsoft, designed for storing, retrieving, and managing data efficiently.

### Others
- Layer Architecture (Service, Repository, Controller): design pattern that separates layers for maintainable and testable applications.
- Vercel: platform for deploying and hosting modern web applications.
- GitHub Actions: CI/CD tool for automating workflows, builds, and deployments directly from your GitHub repository.
- Node.js (v22+): Backend runtime environment for the development and execution of the frontend.
- npm: Package manager to handle JavaScript libraries and dependencies.

### APIs

- RESTful APIs are used to handle interactions between the frontend and backend, ensuring scalability and performance.

<!-- GETTING STARTED -->

## 🎯 Getting Started

- You can view this website backend in repo at: <a href="https://github.com/vihao1802/Social-Media-Client" target="_blank">Social Media Client</a>

### 💎 Prerequisites

You should create a `main` directory to wrap 2 repo (client and server) of this system
- npm(comes with node)
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or higher)
- A code editor such as [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

### ⚙️ Installation

#### 🔧 Frontend 
1. Clone the repo in the `main` directory

```sh
git clone https://github.com/vihao1802/Social-Media-Client.git
```

```sh
git clone https://github.com/vihao1802/Social-Media-Server.git
```

2. Install libraries and dependencies (For client folder):

```
cd client
```

```
npm install
```

3. Create file `.env.local` in folder `client`:

```env
NEXT_PUBLIC_BACKEND_URL=http://localhost:5277/api
NEXT_PUBLIC_WEBSOCKET_URL=ws://localhost:5277/api

NEXT_PUBLIC_STREAM_API_KEY=
STREAM_SECRET_KEY=

CLOUDINARY_CLOUD_NAME=
CLOUDINARY_API_KEY=
CLOUDINARY_API_SECRET=
```

4. Create file `.env` in folder `server` with format:

```env
# Cloudinary Configuration
CLOUDINARY_CLOUD_NAME=
CLOUDINARY_API_KEY=
CLOUDINARY_API_SECRET=
CLIENT_DOMAIN=http://localhost:3000
```

#### 🔧 Backend 
1. **Trust the HTTPS development certificate**
  
- Run the following command to trust the development certificate for secure HTTPS connections:
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
5. Create file `appsettings.Development.json` in folder `server` with format:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JWT": {
    "Issuer": "",
    "Audience": "",


    "SigningKey": ""

  },
  "SmtpSettings": {
    "Server": "smtp.gmail.com",
    "Port": 587,
    "SenderName": "Ninstagram",

    "SenderEmail": "",
    "Username": "",
    "Password": "",
    "EnableSsl": true
  },
  "GoogleKeys":{
    "ClientId":"",
    "ClientSecret":""
  },
  "FacebookKeys":{
    "AppId":"",
    "AppSecret":""
  }
}

```


## 🚀 Usage

### Client ( Run in termnial ):

- From `main` directory

```
cd client
```

- Run `client`

```
npm run dev
```

### Server:
- To watch for changes and automatically rebuild the application during development, use:
```bash
dotnet watch run
```

- To run the application, use:
```bash
dotnet build
```

```bash
dotnet run
```

## ✨ Code Contributors

- This project exists thanks to all the people who contribute.
<a href="https://github.com/vihao1802/Social-Media-Server/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=vihao1802/Social-Media-Server" />
</a>

Made with [contrib.rocks](https://contrib.rocks).

## 📷 Screenshots

- Go to this: <a href="" target="_blank">SCREENSHOT.md</a>

## 📝 License

Copyright © 2024 [Tran Vi Hao](https://github.com/vihao1802).<br />
This project is [MIT](https://github.com/vihao1802/Social-Media-Server/blob/main/LICENSE) licensed.
