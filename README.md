# WebShopSK – ASP.NET Core MVC Application

This is an ASP.NET Core MVC web application built using a clean, modular structure.
The project includes controllers, models, view models, services, EF Core data access
layer, Razor views, and optional Docker support.

---

## ✅ Technologies Used
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **C# 10 / .NET 6** (or your actual version)
- **SQL Server / LocalDB**
- **Razor Views**
- **Dependency Injection**
- **Docker Support**

---

## ✅ Project Structure

```
Areas/              # Optional area modules (e.g. Admin)
Controllers/        # MVC controllers (UI logic)
Data/               # EF Core DbContext + migrations
Extensions/         # Custom extension methods (DI, config, etc.)
Models/             # Domain models (entities)
ViewModels/         # DTOs for views
Views/              # Razor views (.cshtml)
wwwroot/            # Static files (CSS, JS, images)
Program.cs          # Main application bootstrap
WebShopSK.csproj    # Project file
appsettings.json    # App configuration
Dockerfile          # Container build instructions
```

---

## ✅ Configuration

Set your connection string in **appsettings.json**:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=WebShopSK;Trusted_Connection=True;"
}
```

If using EF Core migrations:

```
dotnet ef database update
```

---

## ✅ Running the Application

### ▶️ Using Visual Studio
1. Open `WebShopSK.sln`
2. Set WebShopSK as Startup Project
3. Press **F5** or click **Run**

### ▶️ Using .NET CLI
```
dotnet run
```

App runs at:
```
https://localhost:{port}
```

---

## ✅ Features (example placeholders)
- Product management (CRUD)
- Authentication & authorization
- Razor layout + partial views
- Entity Framework Core database access
- Strongly typed ViewModels
- Validation via DataAnnotations

(Add your real features here.)

---

## ✅ Docker Support (optional)

Build image:
```
docker build -t webshopsk .
```

Run container:
```
docker run -p 8080:80 webshopsk
```

---

## ✅ Requirements
```
.NET 6 SDK (or your version)
SQL Server / LocalDB
Visual Studio 2022 or VS Code
```
