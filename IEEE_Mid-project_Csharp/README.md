# IEEE Mid Project - Exam Console Application

## Overview

An advanced C# console application for managing online exams. The system provides a complete interactive interface for students and administrators to create, manage, and take exams in a command-line environment.

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
