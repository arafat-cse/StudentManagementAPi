# **Student Management API**

A full-stack Student Management API built using ASP.NET Core, Entity Framework, and SQL Server. This API allows for efficient management of student data, subject assignments, and image uploads, while being consumed by an Angular frontend application.

---

## **Table of Contents**

- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

---

## **Overview**

This project is a **Student Management API** where you can manage student data such as personal information and the subjects assigned to each student. The API also supports image uploads for each student, which are stored on the server and linked to the student's profile.

---

## **Technologies Used**

- **Backend**: ASP.NET Core (Web API)
- **Database**: SQL Server (with Entity Framework Core)
- **Frontend**: Angular (Connecting via HTTP requests)
- **File Handling**: Images are uploaded and stored in the `wwwroot/images/` folder.

---

## **Features**

- **Student CRUD Operations**: Create, Read, Update, Delete students with details like name, phone number, email, and address.
- **Subject Assignment**: Assign multiple subjects to each student.
- **Image Upload**: Allows students to upload profile images.
- **CORS Support**: Supports cross-origin requests from the Angular frontend.
- **Swagger API Documentation**: Interactive API documentation available in development mode for easy testing.
- **Security**: Supports basic API security practices with HTTPS redirection.

---

## **Installation**

### **Prerequisites**

- .NET 8 SDK or higher
- SQL Server
- Node.js and Angular CLI (for the Angular frontend)

### **Step 1: Clone the repository**

```bash
git clone https://github.com/your-username/student-management-api.git
cd student-management-api
```

### **Step 2: Set up the database**

1. Open `appsettings.json` and configure the connection string for your SQL Server.
   Example:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=StudentDB;Trusted_Connection=True;"
   }
   ```
2. Create the database using Entity Framework migrations:
   ```bash
   dotnet ef database update
   ```

### **Step 3: Run the API**

Run the API using the following command:

```bash
dotnet run
```

The API will start running on `https://localhost:5001` by default.

---

## **Usage**

Once the API is up and running, you can test it via the Swagger UI available at:

```
https://localhost:5001/swagger
```

Here, you can perform all available CRUD operations for students and manage subjects.

---

## **API Endpoints**

### **Students**

- **GET /api/students** - Get all students
- **GET /api/students/{id}** - Get student by ID
- **POST /api/students** - Create a new student (including image upload)
- **PUT /api/students/{id}** - Update student by ID
- **DELETE /api/students/{id}** - Delete student by ID

### **Subjects**

- **GET /api/subjects** - Get all subjects
- **GET /api/subjects/{id}** - Get subject by ID

### **Student-Subject Relationships**

- **POST /api/student-subjects** - Assign subjects to a student

---

## **Contributing**

Contributions are welcome! Feel free to fork the repository, create a feature branch, and submit a pull request. Make sure to follow the existing code style and write tests where applicable.

---

## **License**

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

### **Author:**
**Mohammad Arafat Rahman**  
[LinkedIn](https://www.linkedin.com/in/arafatdev2008/) | [Email](mailto:arafat.dev61@gmail.com)
