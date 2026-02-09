# Architecture Documentation

## Overview

The Exam Console Application project follows a layered architecture with a routing pattern to ensure clear separation of concerns, easy maintenance, and development.

---

## General Architecture

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│              (Pages)                    │
├─────────────────────────────────────────┤
│         Routing Layer                   │
│         (Router + Routes)               │
├─────────────────────────────────────────┤
│         Business Logic Layer            │
│         (Services)                      │
├─────────────────────────────────────────┤
│         Data Access Layer               │
│         (JsonDatabase)                  │
├─────────────────────────────────────────┤
│         Data Storage Layer              │
│         (JSON Files)                    │
└─────────────────────────────────────────┘
```

---

## Detailed Layers

### 1. Presentation Layer

**Responsibility**: Display information to the user and interact with them

**Components**:
```
Pages/
├── Page.cs (Base Class)
├── Home/
│   ├── HomePage.cs
│   ├── LoginPage.cs
│   ├── RegisterPage.cs
│   └── AboutPage.cs
├── MainMenu/
│   ├── MainMenuPage.cs
│   ├── ProfilePage.cs
│   ├── SubjectsPage.cs
│   ├── TakeExamPage.cs
│   └── HistoryPage.cs
└── Admin/
    ├── AdminPage.cs
    ├── CreateExamPage.cs
    └── StatisticsPage.cs
```

**Design**:
- Each page inherits from the base `Page` class
- Implements two main functions: `Display()` and `HandleInput(Router)`
- Uses Utils for printing and input

**Example Structure**:
```csharp
public class HomePage : Page
{
    public override void Display()
    {
        // Display page interface
    }

    public override void HandleInput(Router router)
    {
        // Handle user input and navigation
    }
}
```

---

### 2. Routing Layer

**Responsibility**: Manage navigation between pages

**Components**:
```
Routes/
├── Router.cs
└── Route.cs
```

**Working Mechanism**:

1. **Registration**:
```csharp
router.Register("home", () => new HomePage());
router.Register("login", () => new LoginPage());
```

2. **Navigation**:
```csharp
router.Navigate("login");
```

3. **Re-routing**:
```csharp
router.RerouteCurrent();
```

**Features**:
- ✅ Factory Pattern for page creation
- ✅ Dictionary-based routing for high performance
- ✅ Auto screen clear on navigation
- ✅ Error handling for non-existent routes

---

### 3. Business Logic Layer

**Responsibility**: Implement business rules and operations

**Components**:
```
Services/
├── DataManager.cs      # Central data manager
├── AppState.cs         # Global application state
├── AdminManager.cs     # Admin operations
└── JsonDatabase.cs     # Database
```

#### DataManager

**Role**: Central access point for all databases

```csharp
public static class DataManager
{
    public static JsonDatabase<User> UserDB { get; }
    public static JsonDatabase<Subject> SubjectDB { get; }
    public static JsonDatabase<StoredExam> ExamDB { get; }
    public static JsonDatabase<ExamResultRecord> ResultDB { get; }
}
```

**Benefits**:
- ✅ Singleton pattern to prevent duplication
- ✅ Lazy initialization
- ✅ Unified interface for data access

#### AppState

**Role**: Store global application state

```csharp
public static class AppState
{
    public static User? CurrentUser { get; set; }
    public static bool IsLoggedIn => CurrentUser != null;
}
```

**Uses**:
- Track current user
- Verify access permissions
- Share data between pages

#### AdminManager

**Role**: Execute complex admin operations

**Main Operations**:
- Create subjects
- Create exams
- Calculate statistics
- Manage results

---

### 4. Data Access Layer

**Responsibility**: Provide programmatic interface for data operations

#### JsonDatabase<T>

**Design**: Generic Repository Pattern

```csharp
public class JsonDatabase<T> where T : class, IIdentifiable
{
    public List<T> GetAll()
    public void Add(T item)
    public T? GetById(int id)
    public void Update(int id, T data)
    public void Delete(int id)
    public void Save(List<T> items)
}
```

**Features**:
- ✅ Generic to work with any model
- ✅ Auto-increment for IDs
- ✅ Complete CRUD operations
- ✅ Error handling
- ✅ Thread-safe operations

**Constraints**:
```csharp
where T : class, IIdentifiable
```
- Must be `class` (Reference Type)
- Must implement `IIdentifiable` interface

---

### 5. Models Layer

**Responsibility**: Represent data entities

```
Models/
├── User.cs
├── Subject.cs
├── StoredExam.cs
└── ExamResultRecord.cs
```

#### User Model

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

**Properties**:
- ✅ Data Transfer Object (DTO)
- ✅ JSON Serializable
- ✅ Business logic-free
- ✅ Multiple constructors

#### StoredExam Model

```csharp
public class StoredExam : IIdentifiable
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public string Title { get; set; }
    public int TimeLimit { get; set; }
    public string Type { get; set; }
    public List<StoredQuestion> Questions { get; set; }
    
    public double GetTotalMarks() { /* ... */ }
}
```

**Design**:
- ✅ Nested models (StoredQuestion, StoredAnswer)
- ✅ Calculated properties (GetTotalMarks)
- ✅ Support for different question types

---

### 6. Utilities Layer

**Responsibility**: Provide helper functions

```
Utils/
├── Print.cs           # Advanced printing functions
├── Input_Handler.cs   # Input processing
└── Arrow_Menu.cs      # Interactive menus
```

#### Print Utility

**Functions**:
```csharp
public static class Print
{
    public static void Title(string text)
    public static void Success(string message)
    public static void Error(string message)
    public static void Info(string message)
    public static void Line()
    public static void ColoredText(string text, ConsoleColor color)
}
```

**Usage**:
- Unified formatting for interfaces
- Colored messages by type
- Separator lines and titles

#### Arrow_Menu Utility

**Function**: Interactive menu with arrow keys

```csharp
public static int ShowMenu(List<string> options, string title)
```

**Features**:
- ✅ Navigate with arrows ↑↓
- ✅ Colored selection
- ✅ Unicode support

---

## Design Patterns Used

### 1. **Repository Pattern**
- `JsonDatabase<T>` implements Repository pattern
- Separates data access logic from business logic

### 2. **Singleton Pattern**
- `DataManager` static class
- `AppState` static class
- Ensures single instance only

### 3. **Factory Pattern**
- Router uses `Func<Page>` delegates
- Creates pages only when needed

### 4. **MVC-like Pattern**
- Models: Data entities
- Views: Pages
- Controllers: Services + Router

### 5. **Strategy Pattern**
- Handle different question types
- `IsTrueFalse` vs Multiple Choice

---

## Data Flow

### Example: Register New User

```
User Input (RegisterPage)
        ↓
Input Validation
        ↓
DataManager.UserDB.Add(user)
        ↓
JsonDatabase<User>.Add()
        ↓
Auto-increment ID
        ↓
Save to users.json
        ↓
Router.Navigate("login")
```

### Example: Taking an Exam

```
Select Subject (SubjectsPage)
        ↓
Router.Navigate("takeexam")
        ↓
Load Exam from ExamDB
        ↓
Display Questions (TakeExamPage)
        ↓
Collect Answers
        ↓
Calculate Score
        ↓
Create ExamResultRecord
        ↓
Save to ResultDB
        ↓
Update User.ExamHistory
        ↓
Display Result
```

---

## Security Considerations

### 1. **Passwords**
⚠️ **Currently**: Passwords stored as plain text  
✅ **Recommended**: Use Hashing (BCrypt, PBKDF2)

### 2. **Permission Verification**
✅ Check `IsAdmin` before accessing admin pages  
✅ Check `AppState.IsLoggedIn` before protected pages

### 3. **Data Validation**
✅ Validation in Input layer  
⚠️ Recommended to add validation in Models layer

---

## Performance Considerations

### 1. **JSON File Operations**
- ✅ Full file read and write
- ⚠️ May be slow with large data
- 💡 **Future improvement**: Use real database

### 2. **Memory Usage**
- ✅ Load all data in memory
- ⚠️ Suitable for small applications only
- 💡 **Improvement**: Lazy loading for large data

### 3. **Router Performance**
- ✅ Dictionary lookup = O(1)
- ✅ Factory pattern delays object creation

---

## Extensibility

### Adding a New Page:

1. Create the class:
```csharp
public class NewPage : Page
{
    public override void Display() { }
    public override void HandleInput(Router router) { }
}
```

2. Register in Router:
```csharp
router.Register("newpage", () => new NewPage());
```

### Adding a New Model:

1. Create the class:
```csharp
public class NewModel : IIdentifiable
{
    public int Id { get; set; }
    // ... other properties
}
```

2. Add Database in DataManager:
```csharp
public static JsonDatabase<NewModel> NewDB { get; } 
    = new("Data/newmodel.json");
```

---

## Testing Strategy

### Unit Testing
- Services layer
- JsonDatabase operations
- Model validations

### Integration Testing
- Router navigation
- Data persistence
- User workflows

### Manual Testing
- UI/UX testing
- Edge cases
- Error handling

---

## Future Improvements

1. **Database Migration**
   - Migrate from JSON to SQLite or SQL Server
   - Use Entity Framework Core

2. **Async Operations**
   - Make I/O operations asynchronous
   - Improve performance

3. **Dependency Injection**
   - Use DI Container
   - Improve testability

4. **Logging System**
   - Add comprehensive logging
   - Track errors and events

5. **Validation Framework**
   - Use FluentValidation
   - Complex validation rules

---

## Conclusion

The project architecture provides:
- ✅ Clear separation between layers
- ✅ Easy maintenance and development
- ✅ Scalability
- ✅ Proven design patterns
- ✅ Clean and organized code

The project provides a strong foundation that can be built upon and developed for larger and more complex applications.
