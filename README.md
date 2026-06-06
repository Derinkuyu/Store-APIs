# Store E-Commerce API

A premium, modern, and robust E-Commerce Web API built with **ASP.NET Core** following Clean Architecture principles. This API provides everything needed for a fully functional e-commerce backend, including authentication, catalog management, shopping cart, and order placement.

---

## 🎥 Demo Video

> [!IMPORTANT]
> **[Watch the Walkthrough & Demo Video Here](https://your-video-link-here.com)** (Update this link with your recorded video URL)

---

## 🏗️ Project Architecture

The solution is split into four distinct layers to maintain separation of concerns, loose coupling, and high testability:

*   **`Store.APIs` (Presentation Layer)**: Web API controllers, custom middlewares, configuration, and API routes.
*   **`Store.BLL` (Business Logic Layer)**: DTOs, domain logic managers, mapping profiles (AutoMapper), and validation rules (FluentValidation).
*   **`Store.DAL` (Data Access Layer)**: Entity models, EF Core DbContext, repository implementations (Generic & Specific), migrations, seeders, and Unit of Work.
*   **`Store.Common` (Common Utility Layer)**: Cross-cutting concerns such as filter/pagination parameters, error models, and standard API responses (`GeneralResult`).

---

## 🛠️ Tech Stack & Features

*   **Framework**: .NET 8.0 / 9.0 Web API
*   **Database ORM**: Entity Framework Core with SQL Server
*   **Authentication & Authorization**: JWT Bearer Tokens with Role-Based & Policy-Based Authorization (`Admin` & `User` roles)
*   **Validation**: Automatic model validation using **FluentValidation**
*   **Object Mapping**: AutoMapper for converting between Database Entities and DTOs
*   **Pagination, Filtering & Sorting**: Custom logic for sorting (`price`, `name`), filtering (`category`, `price range`), and searching (`name` or `description`)
*   **API Documentation**: Scalar / OpenAPI for interactive documentation
*   **Seeding**: Automatic database migration and mock data seeding (Admin/User accounts, Categories, and Products) on start

---

## ⚙️ Setup & Installation

### 1. Prerequisites
*   [.NET SDK](https://dotnet.microsoft.com/download) (Version 8.0 or newer)
*   [SQL Server](https://www.microsoft.com/sql-server/)
*   [Postman](https://www.postman.com/) (For testing endpoints)

### 2. Connection String Configuration
Open `Store.APIs/appsettings.json` and verify or update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=StoreDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 🚀 Running the Project

1. **Restore dependencies**:
   ```bash
   dotnet restore Store.slnx
   ```

2. **Apply migrations and update database**:
   The API is configured to apply migrations automatically on startup. However, you can also run it manually:
   ```bash
   dotnet ef database update --project Store.DAL --startup-project Store.APIs
   ```

3. **Run the API**:
   ```bash
   dotnet run --project Store.APIs/Store.APIs.csproj
   ```

4. **Access the Scalar API documentation**:
   Once the project runs, navigate to the URL output in the terminal (usually `https://localhost:7001/scalar/v1` or `https://localhost:5001/scalar/v1`) to view interactive API documentation.

---

## 🧪 Testing with Postman

A complete Postman Guide containing setup instructions, login credentials, and all 20 API requests is available in the workspace:
*   [Postman Testing Guide](postman_guide.md) (or absolute path if viewed locally)

### API Endpoints Summary

| Feature | Method | Endpoint | Access |
| :--- | :---: | :--- | :--- |
| **Authentication** | `POST` | `/api/auth/register` | Public |
| | `POST` | `/api/auth/login` | Public |
| **Categories** | `GET` | `/api/categories` | Public |
| | `POST` | `/api/categories` | 🔒 Admin |
| | `PUT` | `/api/categories/{id}` | 🔒 Admin |
| | `DELETE` | `/api/categories/{id}` | 🔒 Admin |
| **Products** | `GET` | `/api/products` | Public |
| | `POST` | `/api/products` | 🔒 Admin |
| | `PUT` | `/api/products/{id}` | 🔒 Admin |
| | `DELETE` | `/api/products/{id}` | 🔒 Admin |
| **Cart** | `GET` | `/api/cart` | 🔒 User |
| | `POST` | `/api/cart` | 🔒 User |
| | `DELETE` | `/api/cart/{productId}`| 🔒 User |
| **Orders** | `POST` | `/api/orders` | 🔒 User |
| | `GET` | `/api/orders` | 🔒 User |
| | `GET` | `/api/orders/{id}` | 🔒 User / Admin |
