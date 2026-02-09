# Course Tracking App

Course Tracking App is a full-stack web application for managing courses, users, and enrollments.  
The project is built with **ASP.NET Core (.NET 8)** on the backend and **Vue.js (Vite)** on the frontend.

The application supports **role-based access** (Student / Admin), authentication, course enrollment, and basic course management features.

---

## ✨ Features

### 👤 Authentication & Authorization
- User registration and login  
- JWT-based authentication  
- Password hashing using BCrypt  
- Role-based authorization (Student / Admin)  

---

### 🎓 Student Features
- View and update personal profile information  
- Browse available courses  
- Search courses by name  
- Filter courses by level  
- Enroll in courses  
- View enrolled courses  

---

### 🛠 Admin Features
- Admin profile with elevated permissions  
- View course statistics  
  - Total number of courses  
  - Total number of enrollments  
  - Average course price  
  - Average course duration  
- Create, edit, and delete courses  

---

## 🧱 Tech Stack

### Backend
- ASP.NET Core (.NET 8)  
- Entity Framework Core  
- RESTful API  
- JWT Authentication  
- Swagger / OpenAPI  
- SQL Server (Local Development Database)  

---

### Frontend
- Vue.js (Vite)  
- JavaScript  
- HTML5  
- CSS3  
- Axios for API communication  

---

## 🚀 Running the Project Locally

### Backend (API)

```bash
cd backend/CourseTrackerAPI
dotnet restore
dotnet run
```

Backend runs on:
- https://localhost:5001  
- http://localhost:5129  

Swagger documentation:
```
https://localhost:5001/swagger
```

---

### Frontend (Vue)

```bash
cd frontend/frontend
npm install
npm run dev
```

Frontend runs on:
```
http://localhost:5173
```

---

## 📷 Application Preview

### 🔐 Login
![Login](screenshots/login.png)

---

### 👤 Student Profile
Users can view and update their personal data and see enrolled courses.

![Student Profile](screenshots/student-profile.png)

---

### 📚 Course Catalog (Student)
Students can browse available courses and enroll.

![Courses Student](screenshots/courses-student.png)

---

### 🛠 Admin Dashboard
Admins can view course statistics and manage courses.

![Admin Dashboard](screenshots/admin-dashboard.png)

---

### 👨‍💼 Admin Profile
Admin users have access to extended management features.

![Admin Profile](screenshots/admin-profile.png)

---

## 📝 Notes
- The application uses a local SQL database for development purposes.  
- Build artifacts and IDE-specific files are excluded using `.gitignore`.  

---

## 👩‍💻 Author
**Emilija Kalicanin**
