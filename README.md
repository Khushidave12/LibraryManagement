📚 Library Management System – ASP.NET Web API

A simple and secure **Library Management System backend** built using **ASP.NET Core Web API**, **JWT Authentication**, and **MySQL**.  
This project provides all essential CRUD operations for managing books and users, with secure login functionality.

🚀 Features

🔐 Authentication
- User Registration  
- User Login  
- JWT Token-based Authentication  
- Protected API Endpoints  

📘 Books Management
- Add new books  
- Update existing books  
- Delete books  
- Get all books  
- Get book by ID  

🗄 MySQL Database
- Stores user data  
- Stores books data  
- Entity Framework Core with code-first or database-first approach  

🧩 Clean & Structured Code
- Controllers  
- Models  
- DTOs  
- Services  
- Repository pattern (if added)  

🛠 Technologies Used

| Technology | Purpose |
|-----------|----------|
| **ASP.NET Core Web API** | Backend framework |
| **JWT Authentication** | Secure login |
| **MySQL** | Database |
| **Entity Framework Core** | ORM for data access |
| **C#** | Programming language |
| **Visual Studio 2022** | IDE |

How to Run the Project

Clone the Repo
git clone https://github.com/NILESHMESHRAM29/LibraryManagement.git
cd LibraryManagement

Update appsettings.json

Add your MySQL Connection String and JWT Key:

"ConnectionStrings": {

  "DefaultConnection": "server=localhost;port=3306;database=librarydb;user=root;password=yourpassword"
  
  },

  "Jwt": {

  "Key": "YOUR_SECRET_KEY_HERE"
  
}

🛡 Security Implemented

JWT Bearer Token Authentication

Password hashing

Authorization for protected endpoints
