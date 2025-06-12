# ToDoApplication

A full-stack ToDo application (in progress) starting with an **ASP.NET Core** backend. Frontend (React), PostgreSQL, and Docker will be all be implemented eventually.

---
## 📚 Table of Contents

- [📦 NuGet Package References](#-nuget-package-references)
- [🚀 Running the Project](#-running-the-project)
  - [🔹 Option 1: Run via Docker Compose](#-option-1-run-via-docker-compose)
  - [🔹 Option 2: Run via Visual Studio Debugger](#-option-2-run-via-visual-studio-debugger)
- [🛠 Troubleshooting Docker & EF Core](#-troubleshooting-docker--ef-core)
- [🛠️ Project Progress](#️-project-progress)
  - [✅ Technologies in Use](#-technologies-in-use)
- [📋 Development Checklist](#-development-checklist)
  - [Backend](#backend)
    - [🔧 Backend Setup](#-backend-setup)
    - [🔁 Backend - Planned CRUD Operations](#-backend---planned-crud-operations)
  - [Frontend](#frontend)
    - [🧪 Frontend (Razor for Testing)](#-frontend-razor-for-testing)
    - [💻 Frontend (React – Planned Migration)](#-frontend-react--planned-migration)
    - [🎨 Frontend Design (React – Planned)](#-frontend-design-react--planned)
  - [SQL Services & Containers](#sql-services--containers)
    - [🧱 Planned: Database & Docker Integration](#-planned-database--docker-integration)
- [📝 Notes](#-notes)
---
## 📦 NuGet Package References

Project `ToDoApplication` has the following package references for **`net8.0`**:

| Package Name                                               | Requested Version | Resolved Version |
|------------------------------------------------------------|-------------------|------------------|
| BCrypt.Net-Core                                            | 1.6.0             | 1.6.0            |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore          | 8.0.3             | 8.0.3            |
| Microsoft.EntityFrameworkCore                              | 9.0.4             | 9.0.4            |
| Microsoft.EntityFrameworkCore.InMemory                     | 9.0.4             | 9.0.4            |
| Microsoft.EntityFrameworkCore.Tools                        | 9.0.4             | 9.0.4            |
| Microsoft.VisualStudio.Azure.Containers.Tools.Targets      | 1.21.0            | 1.21.0           |
| Npgsql.EntityFrameworkCore.PostgreSQL                      | 9.0.4             | 9.0.4			|

---

## 🚀 Running the Project (Using Visual Studio)

1. Open the solution (`.sln`) file in **Visual Studio**.
2. Set the ASP.NET project as the startup project.
3. Press `F5` or click **Start Debugging** to run the app.

## 🚀 Running the Project (Using Docker)

### 🔹 Option 1: Run via Docker Compose

1. Make sure Docker is running.
2. In your terminal, navigate to the project root and run:

```bash
docker compose up --build
```

3. Once containers are up, open your browser and go to:
http://localhost:5000

### 🔹 Option 2: Run via Visual Studio Debugger

1. Open the solution (`.sln`) file in **Visual Studio**.
2. Set the Container (Dockerfile) as the startup project.
3. Press `F5` or click **Start Debugging** to run the app.

---
## 🛠 Troubleshooting Docker & EF Core

If you encounter errors like:

> `relation "AspNetUserRoles" does not exist`

It usually means the database migrations haven't been applied.  

Here are three potential fixes:

### ✅ Fix Option 1: Apply existing migrations
If migrations are already set up (e.g., included in the project), run the following 
commands to apply them:

```powershell
$env:ConnectionStrings__DefaultConnection="Host=<host>;Port=<port>;Database=<database>;Username=<username>;Password=<password>"
dotnet ef database update
```

### ✅ Fix Option 2: Create and apply new migrations
If no migrations exist yet, you'll need to create them first:

```powershell
$env:ConnectionStrings__DefaultConnection="Host=<host>;Port=<port>;Database=<database>;Username=<username>;Password=<password>"
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### ✅ Fix Option 3: Run migrations while Docker is running
If you're using Docker and the above commands don't work (e.g., due to connection issues or missing EF CLI tools locally), 
try running the migration while the Docker containers are running:

```powershell
docker-compose build
docker-compose up -d
```
Once the containers are running, execute the following command (from your host machine— i.e., localhost— or inside the container, depending
on your setup):
```powershell
dotnet ef database update --connection "Host=<host>;Port=<port>;Database=<database>;Username=<username>;Password=<password>"
```
---

## 🛠️ Project Progress

### ✅ Technologies in Use
- [X] ASP.NET Core
- [ ] React *(planned)*
- [X] PostgreSQL
- [X] Docker

---

## 📋 Development Checklist

### Backend

#### 🔧 Backend Setup
- [X] Create and run an ASP.NET MVC project
- [X] Set up basic routing and controllers


#### 🔁 Backend - Planned CRUD Operations
  - [X] Register new user
  - [X] Login
  - [X] Get user’s todo list
  - [X] Check/uncheck todo items updates within db
  - [X] Add new todo item
  - [X] Delete todo item
  - [X] Update todo item

---

### Frontend

#### 🧪 Frontend (Razor for Testing)
Using Razor Pages for quick UI while backend is under development.

- [X] Create working routes (Razor)
- [X] Create login page (Razor)
- [X] Create user registration page (Razor)
- [X] Create to-do list page (Razor)
- [X] Create "add task" page (Razor)

#### 💻 Frontend (React – Planned Migration)
React will be used for the final frontend interface once backend logic is complete.
- [ ] Set up React routing
- [ ] Create login page (React)
- [ ] Create registration page (React)
- [ ] Create to-do list page (React)
- [ ] Create "add task" page (React)

#### 🎨 Frontend Design (React – Planned)
- [ ] Style login page
- [ ] Style registration page
- [ ] Style to-do list page
- [ ] Style "add task" page

---

### SQL Services & Containers

#### 🧱 Planned: Database & Docker Integration
- [X] Set up PostgreSQL for local use
- [X] Connect backend to database (locally installed)
- [X] Convert to Docker support
- [X] Create `docker-compose.yml` for orchestration

---

## 📝 Notes
- Currently using **Visual Studio** and the built-in debugger for development.
- Razor Pages are being used temporarily for UI testing during backend development.
- React will replace Razor once API logic is stable.
- Database and container support (PostgreSQL on Docker) will be added later.
