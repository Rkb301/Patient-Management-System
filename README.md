# Patient Management System

A robust, modern, and extensible Patient Management System (PMS) built with ASP.NET Core and MySQL.  
This project streamlines patient data management, supports advanced querying, and implements best practices in error handling and logging.

---

## 🚀 Features

- **Patient CRUD:** Create, read, update, and delete patient records by ID or unique email.
- **Advanced Search:** Flexible querying by multiple fields (ID, name, email, gender, blood group, status, etc.), supporting multi-value filters.
- **Unique Constraints:** Enforces unique email addresses for all patients.
- **Structured Logging:** Integrated Serilog for detailed, structured logs (console and file).
- **Centralized Error Handling:** Custom middleware returns consistent, informative error responses (including custom exceptions).
- **Validation:** Automatic and custom validation for safe, reliable data.
- **Extensible:** Clean architecture—easy to add new features or integrate with frontend apps.

---

## 🛠️ Technologies Used

- **Backend:** ASP.NET Core (.NET 9)
- **Database:** MySQL (with Entity Framework Core)
- **ORM:** Entity Framework Core (code-first migrations)
- **Logging:** Serilog
- **API Docs/Test:** Swagger (OpenAPI)
- **Other:** C#, RESTful API

---

## 📦 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [MySQL Server](https://www.mysql.com/)
- [Visual Studio/Rider/VS Code](https://visualstudio.microsoft.com/)

### Setup Steps

1. **Clone the repository:**
git clone https://github.com/Rkb301/Patient-Management-System.git
cd Patient-Management-System


2. **Configure the database:**
- Set your MySQL connection string in `appsettings.json`:
  ```
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PatientDB;User=root;Password=your_mysql_password;"
  }
  ```

3. **Apply migrations and seed data:**
dotnet ef database update
*(Optional: Insert sample data using the provided SQL script.)*

4. **Run the application:**
dotnet run



- Visit `https://localhost:<port>/swagger` for API documentation and testing.

---

## 📖 API Overview

### Patient Endpoints

| Method | Route                              | Description                                 |
|--------|------------------------------------|---------------------------------------------|
| GET    | `/api/patient`                     | Get all patients                            |
| GET    | `/api/patient/{id}`                | Get patient by ID                           |
| GET    | `/api/patient/by-email/{email}`    | Get patient by email                        |
| POST   | `/api/patient`                     | Create new patient                          |
| PUT    | `/api/patient/{id}`                | Update patient by ID                        |
| PUT    | `/api/patient/by-email/{email}`    | Update patient by email                     |
| DELETE | `/api/patient/{id}`                | Delete patient by ID                        |
| DELETE | `/api/patient/by-email/{email}`    | Delete patient by email                     |
| GET    | `/api/patient/search?...`          | Advanced search with multi-field filtering  |

- **Search endpoint** supports multiple values for each field (e.g., `?name=Ruby&name=Priya&gender=1&gender=2`).

---

## 🧑‍💻 Error Handling & Logging

- **Centralized error middleware** returns [RFC 7807 Problem Details](https://datatracker.ietf.org/doc/html/rfc7807) JSON for all errors.
- **Custom exceptions** (e.g., `PatientNotFoundException`) for clear, domain-specific errors.
- **Serilog** logs all requests, errors, and important events to both console and rolling log files.

---

## ✨ Example Error Response

{
"type": "about:blank",
"title": "Patient not found",
"status": 404,
"detail": "Patient with email 'notfound@example.com' not found.",
"instance": "/api/patient/by-email/notfound@example.com"
}


---

## 🗂️ Project Structure

Patient-Management-System/
├── Controllers/
│ └── PatientController.cs
├── Data/
│ └── PatientContext.cs
├── Exceptions/
│ └── PatientNotFoundException.cs
├── Middleware/
│ └── ErrorHandlingMiddleware.cs
├── Models/
│ └── Patient.cs
├── appsettings.json
├── Program.cs
├── ...


---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!  
Please open an [issue](https://github.com/Rkb301/Patient-Management-System/issues) or submit a pull request.

---

## 📄 License

This project is licensed under the MIT License.

---

## 🙏 Acknowledgements

- Built with love by [ThePowerHex](https://github.com/Rkb301)
- Inspired by open-source healthcare solutions

---
