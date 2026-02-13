# IEEE Mid Project - Exam Console Application

## Overview

An advanced C# console application for managing online exams. The system provides a complete interactive interface for students and administrators to create, manage, and take exams in a command-line environment.

---

## Picuters:

<img width="1216" height="905" alt="image" src="https://github.com/user-attachments/assets/06c620a2-9a5e-4a1c-834a-df6c3d6dd3f0" />

<img width="1255" height="902" alt="image" src="https://github.com/user-attachments/assets/ef1e4bf0-2e24-4c80-9ac1-43d50e1d0db8" />

<img width="676" height="915" alt="image" src="https://github.com/user-attachments/assets/780dc238-c596-401e-a618-750633e37908" />

<img width="980" height="901" alt="image" src="https://github.com/user-attachments/assets/6053c959-0200-4293-9529-9f30c15f5cf4" />

<img width="749" height="906" alt="image" src="https://github.com/user-attachments/assets/f15a765f-e9c8-4f36-9647-33e70deac95d" />

<img width="746" height="909" alt="image" src="https://github.com/user-attachments/assets/d6da5a5e-ddf4-4a03-9f84-b40a358b476d" />

<img width="729" height="911" alt="image" src="https://github.com/user-attachments/assets/7479682d-9dbb-4e68-9473-46e136c1eb63" />

<img width="762" height="906" alt="image" src="https://github.com/user-attachments/assets/64b7e852-5837-42d9-9f2c-22c152d09b4d" />

<img width="672" height="911" alt="image" src="https://github.com/user-attachments/assets/480d96d0-a8ee-4705-b398-a7f8bb02ee1d" />

<img width="701" height="917" alt="image" src="https://github.com/user-attachments/assets/42d21caa-7ef2-4cd7-9e12-6ae4893d599b" />

<img width="1038" height="915" alt="image" src="https://github.com/user-attachments/assets/ded7ddf8-4b33-48dd-b5cf-6c6fd7fc60f3" />

<img width="843" height="906" alt="image" src="https://github.com/user-attachments/assets/8cded039-6b5d-4ffc-ab20-5738e103fd90" />

<img width="809" height="915" alt="image" src="https://github.com/user-attachments/assets/673bc77e-0d93-49da-9f39-2014e43a019a" />

<img width="763" height="915" alt="image" src="https://github.com/user-attachments/assets/2e6eeb20-15f8-4573-a7c5-9ac871bc80d1" />

<img width="739" height="185" alt="image" src="https://github.com/user-attachments/assets/6c73e600-7510-4999-937d-3a54bad6de56" />

<img width="904" height="915" alt="image" src="https://github.com/user-attachments/assets/96a4883f-951e-4a4b-a2fc-5795fbe93c1b" />

<img width="773" height="905" alt="image" src="https://github.com/user-attachments/assets/c646d2bb-6df8-42b5-b7a1-40d6e8097461" />

<img width="694" height="917" alt="image" src="https://github.com/user-attachments/assets/5c1e0763-ed32-4f80-8d5b-cd1f4db457a6" />

<img width="677" height="910" alt="image" src="https://github.com/user-attachments/assets/d8cc3fb9-7c90-4bb4-b1d4-97770a194620" />

<img width="809" height="249" alt="image" src="https://github.com/user-attachments/assets/3b85f6ad-cb0d-4f5b-8509-367c33eab1c6" />

<img width="1308" height="145" alt="image" src="https://github.com/user-attachments/assets/22f2e1db-737d-44c1-916d-194f41f5c37f" />

---

## ⚠️ Important Note - Admin Access

### Default Admin Credentials:
To access the **Admin Panel** and use administrative features, login with these default credentials:

- **Username**: `admin`
- **Email**: `admin`
- **Password**: `admin`

**Admin Features Include:**
- Create and manage subjects
- Create exams with custom questions
- View system statistics and reports
- Manage all system data

> **Note**: For security purposes, it's recommended to change the default admin credentials after first login in a production environment.

---

## Key Features

### For Students
- ✅ User registration and login
- ✅ Browse available subjects
- ✅ Take timed exams
- ✅ Multiple choice and True/False questions support
- ✅ View results and exam history
- ✅ Profile management

### For Administrators
- ✅ Create and manage subjects
- ✅ Create custom exams
- ✅ Add various question types
- ✅ View statistics and reports
- ✅ Full system management

## Requirements

- .NET 8.0 SDK or higher
- Newtonsoft.Json package
- Windows/Linux/macOS

## Installation & Running

### 1. Clone the Project
```bash
git clone <repository-url>
cd IEEE_Mid-project_Csharp
```

### 2. Restore Packages
```bash
dotnet restore
```

### 3. Build the Project
```bash
dotnet build
```

### 4. Run the Application
```bash
dotnet run
```

## Architecture

The project follows a structured layered architecture with clear separation of concerns:

### Main Layers:
- **Models**: Data entities (User, Exam, Subject, ExamResult)
- **Services**: Business logic and data management
- **Pages**: User interfaces
- **Routes**: Navigation and routing system
- **Utils**: Helper utilities (printing, input, menus)
- **Data**: JSON data storage

## Database

The system uses JSON as a lightweight database with four main files:

- `users.json` - User data
- `subjects.json` - Academic subjects
- `exams.json` - Exams
- `results.json` - Student results

## Quick Start Guide

### For Students:
1. Choose "Register" from the home page
2. Enter required information
3. Login using email and password
4. Choose "Subjects" to view available subjects
5. Choose "Take Exam" to start an exam
6. View your results in "History"

### For Administrators:
1. Login with an admin account
2. Choose "Admin Panel"
3. Create a new subject
4. Create an exam and add questions
5. View statistics from "Statistics"

## Full Documentation

For detailed information, refer to:

- [ARCHITECTURE.md](ARCHITECTURE.md) - Detailed architecture documentation
- [FILE_STRUCTURE.md](FILE_STRUCTURE.md) - File and folder structure
- [EXAMPLES.md](EXAMPLES.md) - Code examples and usage
- [PROJECT_SUMMARY.md](PROJECT_SUMMARY.md) - Comprehensive project summary

## Technologies Used

- **Language**: C# 12.0
- **Framework**: .NET 8.0
- **Data Storage**: JSON (Newtonsoft.Json)
- **Architecture**: Layered Architecture with Routing Pattern
- **UI**: Console-based interactive interface

## Contributing

Feel free to open Issues or Pull Requests to improve the project.

## License

This is an educational project available for academic use.

---

**Developed for IEEE Mid Project**  
*Version: 1.0*  
*Last Updated: February 2026*
