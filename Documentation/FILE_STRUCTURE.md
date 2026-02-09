# File Structure Documentation

## Complete Overview

```
IEEE_Mid-project_Csharp/
│
├── 📄 C-sharp-exam-console-app-main.sln    # Solution File
├── 📄 Mid_Proj.csproj                      # Project Configuration
├── 📄 Program.cs                           # Entry Point
├── 📄 README.md                            # Project Documentation
├── 📄 ARCHITECTURE.md                      # Architecture Details
├── 📄 FILE_STRUCTURE.md                    # This File
├── 📄 EXAMPLES.md                          # Code Examples
├── 📄 PROJECT_SUMMARY.md                   # Project Summary
│
├── 📁 Data/                                # JSON Database Files
│   ├── users.json                         # User Records
│   ├── subjects.json                      # Academic Subjects
│   ├── exams.json                         # Exam Definitions
│   └── results.json                       # Exam Results
│
├── 📁 Models/                              # Data Models
│   ├── User.cs                            # User Entity
│   ├── Subject.cs                         # Subject Entity
│   ├── StoredExam.cs                      # Exam Entity
│   └── ExamResultRecord.cs                # Result Entity
│
├── 📁 Services/                            # Business Logic
│   ├── DataManager.cs                     # Central Data Access
│   ├── JsonDatabase.cs                    # Generic Repository
│   ├── AppState.cs                        # Application State
│   └── AdminManager.cs                    # Admin Operations
│
├── 📁 Pages/                               # UI Pages
│   ├── Page.cs                            # Base Page Class
│   │
│   ├── 📁 Home/                           # Home Section
│   │   ├── HomePage.cs                    # Landing Page
│   │   ├── LoginPage.cs                   # Login Interface
│   │   ├── RegisterPage.cs                # Registration
│   │   └── AboutPage.cs                   # About Info
│   │
│   ├── 📁 MainMenu/                       # Main Menu Section
│   │   ├── MainMenuPage.cs                # Main Dashboard
│   │   ├── ProfilePage.cs                 # User Profile
│   │   ├── SubjectsPage.cs                # Available Subjects
│   │   ├── TakeExamPage.cs                # Exam Interface
│   │   └── HistoryPage.cs                 # Exam History
│   │
│   └── 📁 Admin/                          # Admin Section
│       ├── AdminPage.cs                   # Admin Dashboard
│       ├── CreateExamPage.cs              # Exam Creation
│       └── StatisticsPage.cs              # Statistics View
│
├── 📁 Routes/                              # Navigation System
│   ├── Router.cs                          # Route Manager
│   └── Route.cs                           # Route Definition
│
├── 📁 Utils/                               # Utility Functions
│   ├── Print.cs                           # Console Output
│   ├── Input_Handler.cs                   # Input Processing
│   └── Arrow_Menu.cs                      # Interactive Menus
│
├── 📁 bin/                                 # Build Output
│   └── Debug/net8.0/                      # Debug Build
│
└── 📁 obj/                                 # Build Objects
    └── Debug/net8.0/                      # Build Cache
```

---

## Detailed Breakdown

## 1. Root Files

### 📄 C-sharp-exam-console-app-main.sln
**Description**: Solution File for Visual Studio  
**Purpose**: Organize projects and settings  
**File Type**: XML-based  
**Usage**: Opened by Visual Studio or Rider

### 📄 Mid_Proj.csproj
**Description**: Project configuration file  
**Content**:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" />
  </ItemGroup>
</Project>
```
**Key Settings**:
- `TargetFramework`: net8.0
- `OutputType`: Exe (Console Application)
- `Dependencies`: Newtonsoft.Json

### 📄 Program.cs
**Description**: Application entry point  
**Size**: ~30 lines  
**Main Function**: Register pages and start application

**Structure**:
```csharp
namespace MID_PROJ;

public class Program
{
    static void Main(string[] args)
    {
        // Router setup
        // Page registration
        // Start application
    }
}
```

**Responsibilities**:
1. Setup encoding for special characters
2. Create Router
3. Register all pages
4. Start application from "home"

---

## 2. Data Folder

### 📁 Data/

#### users.json
**Description**: User database  
**Structure**:
```json
[
  {
    "Id": 0,
    "Username": "admin",
    "Email": "admin@exam.com",
    "Password": "admin123",
    "IsAdmin": true,
    "ExamHistory": [],
    "RegistrationDate": "2026-02-04T00:00:00"
  }
]
```

**Fields**:
- `Id` (int): Unique identifier
- `Username` (string): Username
- `Email` (string): Email address
- `Password` (string): Password
- `IsAdmin` (bool): Admin privileges
- `ExamHistory` (int[]): Exam history
- `RegistrationDate` (DateTime): Registration date

#### subjects.json
**Description**: Available academic subjects  
**Structure**:
```json
[
  {
    "Id": 0,
    "Name": "Mathematics",
    "Description": "Advanced Mathematics Course",
    "CreatedBy": 0,
    "CreationDate": "2026-02-04T00:00:00"
  }
]
```

**Fields**:
- `Id` (int): Subject identifier
- `Name` (string): Subject name
- `Description` (string): Description
- `CreatedBy` (int): Creator admin ID
- `CreationDate` (DateTime): Creation date

#### exams.json
**Description**: Exam definitions  
**Structure**:
```json
[
  {
    "Id": 0,
    "SubjectId": 0,
    "Title": "Midterm Exam",
    "TimeLimit": 60,
    "Type": "Midterm",
    "Questions": [
      {
        "Id": 0,
        "Header": "What is 2+2?",
        "Mark": 5,
        "IsTrueFalse": false,
        "Answer": null,
        "Answers": [
          {"Id": 0, "Text": "3", "IsCorrect": false},
          {"Id": 1, "Text": "4", "IsCorrect": true}
        ]
      }
    ]
  }
]
```

**Main Fields**:
- `Id`: Exam identifier
- `SubjectId`: Link to subject
- `Title`: Exam title
- `TimeLimit`: Time limit (minutes)
- `Type`: Exam type (Midterm/Final/Quiz)
- `Questions`: Questions list

**Question Structure**:
- `Header`: Question text
- `Mark`: Score value
- `IsTrueFalse`: Question type
- `Answer`: For True/False
- `Answers`: For multiple choice

#### results.json
**Description**: Student results  
**Structure**:
```json
[
  {
    "Id": 0,
    "UserId": 1,
    "ExamId": 0,
    "Score": 85.5,
    "TotalMarks": 100,
    "Percentage": 85.5,
    "CompletionDate": "2026-02-04T15:30:00",
    "TimeTaken": 45
  }
]
```

**Fields**:
- `UserId`: Link to student
- `ExamId`: Link to exam
- `Score`: Score achieved
- `TotalMarks`: Total marks
- `Percentage`: Percentage score
- `CompletionDate`: Completion date
- `TimeTaken`: Time taken

---

## 3. Models Folder

### 📁 Models/

#### User.cs
**Description**: User data model  
**Size**: ~40 lines  
**Type**: Entity/DTO  

**Properties**:
```csharp
public class User : IIdentifiable
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public List<int> ExamHistory { get; set; }
    public DateTime RegistrationDate { get; set; }
}
```

**Constructors**:
1. `User()` - For JSON Deserialization
2. `User(username, email, password, isAdmin)` - For creation

**Usage**:
- Registration and login
- Permission management
- Exam history tracking

#### Subject.cs
**Description**: Academic subject model  
**Size**: ~35 lines  
**Relationships**: Linked to User (CreatedBy)

**Properties**:
```csharp
public class Subject : IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreationDate { get; set; }
}
```

#### StoredExam.cs
**Description**: Exam model  
**Size**: ~80 lines  
**Complexity**: High (nested objects)

**Hierarchical Structure**:
```
StoredExam
└── List<StoredQuestion>
    └── List<StoredAnswer>
```

**Classes**:
1. `StoredExam`: Main exam
2. `StoredQuestion`: Question
3. `StoredAnswer`: Answer

**Functions**:
```csharp
public double GetTotalMarks()
{
    return Questions.Sum(q => q.Mark);
}
```

#### ExamResultRecord.cs
**Description**: Exam result model  
**Size**: ~40 lines  
**Relationships**: User + Exam

**Properties**:
```csharp
public class ExamResultRecord : IIdentifiable
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ExamId { get; set; }
    public double Score { get; set; }
    public double TotalMarks { get; set; }
    public double Percentage { get; set; }
    public DateTime CompletionDate { get; set; }
    public int TimeTaken { get; set; }
}
```

---

## 4. Services Folder

### 📁 Services/

#### DataManager.cs
**Description**: Central data manager  
**Size**: ~15 lines  
**Pattern**: Singleton Static Class

**Structure**:
```csharp
public static class DataManager
{
    public static JsonDatabase<User> UserDB { get; }
    public static JsonDatabase<Subject> SubjectDB { get; }
    public static JsonDatabase<StoredExam> ExamDB { get; }
    public static JsonDatabase<ExamResultRecord> ResultDB { get; }
}
```

**Usage**:
```csharp
var user = DataManager.UserDB.GetById(1);
DataManager.UserDB.Add(newUser);
```

#### JsonDatabase.cs
**Description**: Generic JSON database  
**Size**: ~75 lines  
**Pattern**: Generic Repository Pattern

**Operations**:
```csharp
public class JsonDatabase<T> where T : class, IIdentifiable
{
    public List<T> GetAll()
    public void Add(T item)
    public T? GetById(int id)
    public void Update(int id, T data)
    public void Delete(int id)
    private void Save(List<T> items)
}
```

**Features**:
- ✅ Generic type support
- ✅ Auto-increment IDs
- ✅ CRUD operations
- ✅ File initialization
- ✅ Exception handling

#### AppState.cs
**Description**: Global application state  
**Size**: ~10 lines  
**Pattern**: Singleton Static Class

**Structure**:
```csharp
public static class AppState
{
    public static User? CurrentUser { get; set; }
    public static bool IsLoggedIn => CurrentUser != null;
}
```

**Usage**:
- Track current user
- Verify login status
- Share data between pages

#### AdminManager.cs
**Description**: Admin operations  
**Size**: ~100+ lines  
**Responsibilities**: Complex admin operations

**Functions**:
```csharp
public static class AdminManager
{
    public static void CreateSubject()
    public static void CreateExam()
    public static void ViewStatistics()
    public static List<ExamResultRecord> GetAllResults()
}
```

---

## 5. Pages Folder

### 📁 Pages/

#### Page.cs (Base Class)
**Description**: Base class for all pages  
**Size**: ~15 lines  
**Pattern**: Abstract Base Class

**Structure**:
```csharp
public abstract class Page
{
    public abstract void Display();
    public abstract void HandleInput(Router router);
}
```

**Purpose**:
- Unify page interface
- Enforce basic functions
- Facilitate Router usage

### 📁 Home/

#### HomePage.cs
**Description**: Landing page  
**Size**: ~60 lines  
**Options**:
1. Login
2. Register
3. About
4. Exit

**Responsibilities**:
- Display main menu
- Navigate to initial pages
- Handle exit

#### LoginPage.cs
**Description**: Login page  
**Size**: ~80 lines  
**Operations**:
1. Enter Email
2. Enter Password
3. Verify credentials
4. Update AppState
5. Route to appropriate page

**Verification**:
```csharp
var user = DataManager.UserDB.GetAll()
    .FirstOrDefault(u => u.Email == email && u.Password == password);

if (user != null)
{
    AppState.CurrentUser = user;
    router.Navigate(user.IsAdmin ? "admin" : "main");
}
```

#### RegisterPage.cs
**Description**: Registration page  
**Size**: ~100 lines  
**Steps**:
1. Enter Username
2. Enter Email (+ Validation)
3. Enter Password (+ Confirmation)
4. Create new User
5. Save to Database

**Validation**:
- Email format validation
- Password confirmation
- Username uniqueness
- Email uniqueness

#### AboutPage.cs
**Description**: System information page  
**Size**: ~40 lines  
**Content**:
- System description
- Features
- Team information

### 📁 MainMenu/

#### MainMenuPage.cs
**Description**: Student main menu  
**Size**: ~70 lines  
**Options**:
1. My Profile
2. Available Subjects
3. Take Exam
4. Exam History
5. Logout

**Protection**:
```csharp
if (!AppState.IsLoggedIn)
{
    router.Navigate("login");
    return;
}
```

#### ProfilePage.cs
**Description**: User profile  
**Size**: ~60 lines  
**Displayed Information**:
- Username
- Email
- Registration Date
- Number of Exams Taken
- Admin Status

#### SubjectsPage.cs
**Description**: Display available subjects  
**Size**: ~80 lines  
**Functions**:
- Display all subjects
- Select subject for exam
- Display subject details

**Display**:
```csharp
var subjects = DataManager.SubjectDB.GetAll();
foreach (var subject in subjects)
{
    Print.Info($"[{subject.Id}] {subject.Name}");
    Print.Info($"    {subject.Description}");
}
```

#### TakeExamPage.cs
**Description**: Exam taking page  
**Size**: ~200+ lines  
**Complexity**: High

**Workflow**:
1. Select subject
2. Display available exams
3. Start exam
4. Display questions
5. Record answers
6. Calculate score
7. Save result
8. Display final score

**Features**:
- ✅ Timer countdown
- ✅ Support different question types
- ✅ Automatic grading
- ✅ Instant results display

#### HistoryPage.cs
**Description**: Exam history  
**Size**: ~90 lines  
**Information**:
- All previous exams
- Scores
- Dates
- Percentages

**Display**:
```csharp
var userResults = DataManager.ResultDB.GetAll()
    .Where(r => r.UserId == AppState.CurrentUser.Id)
    .OrderByDescending(r => r.CompletionDate);
```

### 📁 Admin/

#### AdminPage.cs
**Description**: Admin dashboard  
**Size**: ~80 lines  
**Permissions**: Admin Only

**Options**:
1. Create New Subject
2. Create New Exam
3. View Statistics
4. Manage Users
5. Logout

**Protection**:
```csharp
if (!AppState.IsLoggedIn || !AppState.CurrentUser.IsAdmin)
{
    router.Navigate("home");
    return;
}
```

#### CreateExamPage.cs
**Description**: Create new exam  
**Size**: ~250+ lines  
**Complexity**: Very High

**Steps**:
1. Select subject
2. Enter exam details
3. Add questions:
   - True/False questions
   - Multiple Choice questions
4. Set marks
5. Save

**Features**:
- ✅ Interactive interface
- ✅ Multiple question types support
- ✅ Preview before saving
- ✅ Set correct answers

#### StatisticsPage.cs
**Description**: Display statistics  
**Size**: ~120 lines  
**Information**:
- Number of students
- Number of subjects
- Number of exams
- Average scores
- Best and worst results
- Subject statistics

**Calculations**:
```csharp
var averageScore = results.Average(r => r.Percentage);
var topScore = results.Max(r => r.Percentage);
var lowestScore = results.Min(r => r.Percentage);
```

---

## 6. Routes Folder

### 📁 Routes/

#### Router.cs
**Description**: Navigation manager  
**Size**: ~45 lines  
**Pattern**: Router Pattern

**Main Functions**:
```csharp
public void Register(string path, Func<Page> factory)
public void Navigate(string path)
public void Start(string startPath)
public void RerouteCurrent()
```

**Internal Structure**:
```csharp
private readonly Dictionary<string, Route> _routes;
private string _currentPath;
```

#### Route.cs
**Description**: Route definition  
**Size**: ~10 lines  
**Structure**:
```csharp
public class Route
{
    public Func<Page> PageFactory { get; set; }
}
```

---

## 7. Utils Folder

### 📁 Utils/

#### Print.cs
**Description**: Formatted printing functions  
**Size**: ~100 lines  

**Functions**:
```csharp
public static void Title(string text)
public static void Success(string message)
public static void Error(string message)
public static void Info(string message)
public static void Warning(string message)
public static void Line()
public static void ColoredText(string text, ConsoleColor color)
```

**Colors**:
- Title: Cyan
- Success: Green
- Error: Red
- Warning: Yellow
- Info: White

#### Input_Handler.cs
**Description**: Input processing  
**Size**: ~80 lines  

**Functions**:
```csharp
public static string ReadString(string prompt)
public static int ReadInt(string prompt)
public static bool ReadBool(string prompt)
public static string ReadEmail(string prompt)
public static string ReadPassword(string prompt)
```

**Validation**:
- Type validation
- Error handling
- Clear messages

#### Arrow_Menu.cs
**Description**: Interactive arrow menus  
**Size**: ~120 lines  

**Main Function**:
```csharp
public static int ShowMenu(List<string> options, string title)
```

**Features**:
- ✅ Arrow key navigation
- ✅ Colored selection
- ✅ Escape to cancel
- ✅ Attractive interface

**Controls**:
- `↑` - Move up
- `↓` - Move down
- `Enter` - Select
- `Esc` - Cancel

---

## 8. Build Folders

### 📁 bin/
**Description**: Compiled output files  
**Created by**: dotnet build/run  
**Content**:
```
bin/
└── Debug/
    └── net8.0/
        ├── Mid_Proj.exe
        ├── Mid_Proj.dll
        ├── Mid_Proj.deps.json
        ├── Mid_Proj.runtimeconfig.json
        └── Newtonsoft.Json.dll
```

### 📁 obj/
**Description**: Temporary build files  
**Created by**: dotnet restore/build  
**Content**:
```
obj/
├── project.assets.json
├── Mid_Proj.csproj.nuget.dgspec.json
├── Mid_Proj.csproj.nuget.g.props
└── Debug/
    └── net8.0/
        ├── Mid_Proj.AssemblyInfo.cs
        └── ...
```

---

## File Count Summary

### Source Code Files:
- **Models**: 4 files
- **Services**: 4 files
- **Pages**: 13 files
- **Routes**: 2 files
- **Utils**: 3 files
- **Root**: 1 file (Program.cs)

**Total C# Files**: **27 files**

### Data Files:
- **JSON Files**: 4 files

### Documentation Files:
- **MD Files**: 5 files

### Configuration Files:
- **.csproj**: 1 file
- **.sln**: 1 file

---

## File Size Estimates

| Category | Lines of Code | File Count |
|----------|---------------|------------|
| Models | ~200 | 4 |
| Services | ~250 | 4 |
| Pages | ~1500 | 13 |
| Routes | ~55 | 2 |
| Utils | ~300 | 3 |
| Program.cs | ~30 | 1 |
| **Total** | **~2335** | **27** |

---

## Dependencies Graph

```
Program.cs
    └── Router
        └── Pages (All)
            ├── Models
            ├── Services
            │   ├── DataManager
            │   │   └── JsonDatabase
            │   ├── AppState
            │   └── AdminManager
            └── Utils
                ├── Print
                ├── Input_Handler
                └── Arrow_Menu
```

---

## Naming Conventions

### Files:
- **PascalCase** for all files
- Classes match file names
- Pages end with "Page"

### Folders:
- **PascalCase** for folders
- Clear and concise names
- Logical grouping

### Classes:
- **PascalCase** for classes
- **PascalCase** for Properties
- **camelCase** for private fields

---

## Best Practices

### Organization:
✅ Clear separation between layers  
✅ Logical folders  
✅ Descriptive names  

### Maintainability:
✅ One file per class  
✅ Reasonable file sizes  
✅ Comments when needed  

### Scalability:
✅ Extensible structure  
✅ Easy to add pages  
✅ Easy to add models  

---

This structure provides clear and logical organization that facilitates development and future maintenance.
