# Code Examples and Usage Guide

This document provides comprehensive code examples and usage patterns for the Exam Console Application.

---

## Table of Contents

1. [Basic Usage Examples](#basic-usage-examples)
2. [Models Usage](#models-usage)
3. [Database Operations](#database-operations)
4. [Page Implementation](#page-implementation)
5. [Router Usage](#router-usage)
6. [Admin Operations](#admin-operations)
7. [Utility Functions](#utility-functions)
8. [Complete Workflows](#complete-workflows)

---

## Basic Usage Examples

### Creating a New User

```csharp
// Create a regular user
var user = new User(
    username: "john_doe",
    email: "john@example.com",
    password: "password123",
    isAdmin: false
);

// Add to database
DataManager.UserDB.Add(user);

// Create an admin user
var admin = new User(
    username: "admin",
    email: "admin@example.com",
    password: "admin123",
    isAdmin: true
);

DataManager.UserDB.Add(admin);
```

### User Authentication

```csharp
public bool AuthenticateUser(string email, string password)
{
    var users = DataManager.UserDB.GetAll();
    var user = users.FirstOrDefault(u => 
        u.Email == email && u.Password == password);
    
    if (user != null)
    {
        AppState.CurrentUser = user;
        return true;
    }
    
    return false;
}

// Usage
if (AuthenticateUser("john@example.com", "password123"))
{
    Console.WriteLine("Login successful!");
    // Navigate to main menu
}
else
{
    Console.WriteLine("Invalid credentials!");
}
```

---

## Models Usage

### Working with User Model

```csharp
// Create user with default constructor
var user = new User();
user.Username = "jane_doe";
user.Email = "jane@example.com";
user.Password = "pass456";
user.IsAdmin = false;

// Add exam to history
user.ExamHistory.Add(1);
user.ExamHistory.Add(2);

// Check if user has taken specific exam
bool hasTakenExam = user.ExamHistory.Contains(examId);

// Get number of exams taken
int examCount = user.ExamHistory.Count;
```

### Working with Subject Model

```csharp
// Create a new subject
var subject = new Subject
{
    Name = "Mathematics",
    Description = "Advanced Mathematics Course",
    CreatedBy = AppState.CurrentUser.Id,
    CreationDate = DateTime.Now
};

DataManager.SubjectDB.Add(subject);

// Get all subjects
var subjects = DataManager.SubjectDB.GetAll();

// Find subject by name
var mathSubject = subjects.FirstOrDefault(s => s.Name == "Mathematics");
```

### Working with StoredExam Model

```csharp
// Create a new exam
var exam = new StoredExam(
    subjectId: 1,
    title: "Midterm Exam",
    timeLimit: 60,
    type: "Midterm"
);

// Add True/False question
var trueFalseQuestion = new StoredQuestion
{
    Header = "The earth is flat.",
    Mark = 5,
    IsTrueFalse = true,
    Answer = false  // Correct answer
};

exam.Questions.Add(trueFalseQuestion);

// Add Multiple Choice question
var mcQuestion = new StoredQuestion
{
    Header = "What is 2 + 2?",
    Mark = 10,
    IsTrueFalse = false,
    Answers = new List<StoredAnswer>
    {
        new StoredAnswer { Text = "3", IsCorrect = false },
        new StoredAnswer { Text = "4", IsCorrect = true },
        new StoredAnswer { Text = "5", IsCorrect = false },
        new StoredAnswer { Text = "22", IsCorrect = false }
    }
};

exam.Questions.Add(mcQuestion);

// Calculate total marks
double totalMarks = exam.GetTotalMarks(); // Returns 15

// Save exam
DataManager.ExamDB.Add(exam);
```

### Working with ExamResultRecord Model

```csharp
// Record exam result
var result = new ExamResultRecord
{
    UserId = AppState.CurrentUser.Id,
    ExamId = examId,
    Score = 12.5,
    TotalMarks = 15,
    Percentage = (12.5 / 15) * 100, // 83.33%
    CompletionDate = DateTime.Now,
    TimeTaken = 45 // minutes
};

DataManager.ResultDB.Add(result);

// Add to user's exam history
var user = DataManager.UserDB.GetById(AppState.CurrentUser.Id);
user.ExamHistory.Add(result.Id);
DataManager.UserDB.Update(user.Id, user);
```

---

## Database Operations

### JsonDatabase CRUD Operations

```csharp
// CREATE - Add new item
var user = new User("john", "john@email.com", "pass", false);
DataManager.UserDB.Add(user);
// ID is auto-assigned

// READ - Get all items
var allUsers = DataManager.UserDB.GetAll();

// READ - Get by ID
var user = DataManager.UserDB.GetById(1);
if (user != null)
{
    Console.WriteLine(user.Username);
}

// UPDATE - Modify existing item
var user = DataManager.UserDB.GetById(1);
if (user != null)
{
    user.Username = "new_username";
    DataManager.UserDB.Update(1, user);
}

// DELETE - Remove item
DataManager.UserDB.Delete(1);
```

### Advanced Queries

```csharp
// Find users by criteria
var adminUsers = DataManager.UserDB.GetAll()
    .Where(u => u.IsAdmin)
    .ToList();

// Get recent registrations
var recentUsers = DataManager.UserDB.GetAll()
    .Where(u => u.RegistrationDate > DateTime.Now.AddDays(-7))
    .OrderByDescending(u => u.RegistrationDate)
    .ToList();

// Count exams taken by user
var user = DataManager.UserDB.GetById(userId);
int examCount = user?.ExamHistory.Count ?? 0;

// Get user's results
var userResults = DataManager.ResultDB.GetAll()
    .Where(r => r.UserId == userId)
    .OrderByDescending(r => r.CompletionDate)
    .ToList();

// Calculate average score
var averageScore = userResults.Any() 
    ? userResults.Average(r => r.Percentage) 
    : 0;

// Get exams for specific subject
var subjectExams = DataManager.ExamDB.GetAll()
    .Where(e => e.SubjectId == subjectId)
    .ToList();
```

### Filtering and Sorting

```csharp
// Get passing results (>= 60%)
var passingResults = DataManager.ResultDB.GetAll()
    .Where(r => r.Percentage >= 60)
    .ToList();

// Get top 5 performers
var topPerformers = DataManager.ResultDB.GetAll()
    .OrderByDescending(r => r.Percentage)
    .Take(5)
    .ToList();

// Group results by exam
var resultsByExam = DataManager.ResultDB.GetAll()
    .GroupBy(r => r.ExamId)
    .ToDictionary(g => g.Key, g => g.ToList());

// Get subjects with exam count
var subjectsWithExamCount = DataManager.SubjectDB.GetAll()
    .Select(s => new
    {
        Subject = s,
        ExamCount = DataManager.ExamDB.GetAll()
            .Count(e => e.SubjectId == s.Id)
    })
    .ToList();
```

---

## Page Implementation

### Creating a Custom Page

```csharp
using MID_PROJ.Pages;
using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Utils;

public class CustomPage : Page
{
    public override void Display()
    {
        Print.Title("Custom Page");
        Print.Line();
        
        Print.Info("This is a custom page example");
        Print.Success("You can add your content here");
        
        Print.Line();
    }

    public override void HandleInput(Router router)
    {
        Console.WriteLine("\n1. Option One");
        Console.WriteLine("2. Option Two");
        Console.WriteLine("3. Back");
        
        Console.Write("\nSelect option: ");
        var choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                // Handle option 1
                Print.Success("Option 1 selected");
                router.RerouteCurrent();
                break;
                
            case "2":
                // Handle option 2
                Print.Success("Option 2 selected");
                router.RerouteCurrent();
                break;
                
            case "3":
                router.Navigate("main");
                break;
                
            default:
                Print.Error("Invalid option!");
                router.RerouteCurrent();
                break;
        }
    }
}
```

### Interactive Menu Page

```csharp
public class MenuPage : Page
{
    public override void Display()
    {
        Print.Title("Menu Page");
        Print.Line();
    }

    public override void HandleInput(Router router)
    {
        var options = new List<string>
        {
            "View Profile",
            "Take Exam",
            "View History",
            "Logout"
        };

        int choice = Arrow_Menu.ShowMenu(options, "Main Menu");

        switch (choice)
        {
            case 0:
                router.Navigate("profile");
                break;
            case 1:
                router.Navigate("takeexam");
                break;
            case 2:
                router.Navigate("history");
                break;
            case 3:
                AppState.CurrentUser = null;
                router.Navigate("home");
                break;
            default:
                router.RerouteCurrent();
                break;
        }
    }
}
```

---

## Router Usage

### Registering Routes

```csharp
// In Program.cs
var router = new Router();

// Register pages
router.Register("home", () => new HomePage());
router.Register("login", () => new LoginPage());
router.Register("register", () => new RegisterPage());
router.Register("main", () => new MainMenuPage());
router.Register("profile", () => new ProfilePage());
router.Register("admin", () => new AdminPage());

// Start application
router.Start("home");
```

### Navigation Examples

```csharp
// Simple navigation
router.Navigate("login");

// Conditional navigation
if (AppState.IsLoggedIn)
{
    router.Navigate(AppState.CurrentUser.IsAdmin ? "admin" : "main");
}
else
{
    router.Navigate("login");
}

// Re-route to current page (refresh)
router.RerouteCurrent();

// Navigation with state change
AppState.CurrentUser = null; // Logout
router.Navigate("home");
```

### Protected Routes

```csharp
public class ProtectedPage : Page
{
    public override void Display()
    {
        // Check authentication
        if (!AppState.IsLoggedIn)
        {
            Print.Error("You must be logged in!");
            return;
        }

        Print.Title("Protected Content");
        // Display protected content
    }

    public override void HandleInput(Router router)
    {
        // Redirect if not authenticated
        if (!AppState.IsLoggedIn)
        {
            router.Navigate("login");
            return;
        }

        // Handle input for authenticated users
        // ...
    }
}

public class AdminOnlyPage : Page
{
    public override void Display()
    {
        // Check admin privileges
        if (!AppState.IsLoggedIn || !AppState.CurrentUser.IsAdmin)
        {
            Print.Error("Access Denied! Admin only.");
            return;
        }

        Print.Title("Admin Panel");
        // Display admin content
    }

    public override void HandleInput(Router router)
    {
        // Redirect if not admin
        if (!AppState.IsLoggedIn || !AppState.CurrentUser.IsAdmin)
        {
            router.Navigate("home");
            return;
        }

        // Handle admin operations
        // ...
    }
}
```

---

## Admin Operations

### Creating a Subject

```csharp
public static void CreateSubject()
{
    Print.Title("Create New Subject");
    Print.Line();

    Console.Write("Subject Name: ");
    string name = Console.ReadLine() ?? "";

    Console.Write("Description: ");
    string description = Console.ReadLine() ?? "";

    var subject = new Subject
    {
        Name = name,
        Description = description,
        CreatedBy = AppState.CurrentUser.Id,
        CreationDate = DateTime.Now
    };

    DataManager.SubjectDB.Add(subject);
    Print.Success($"Subject '{name}' created successfully!");
}
```

### Creating an Exam

```csharp
public static void CreateExam()
{
    Print.Title("Create New Exam");
    
    // Select subject
    var subjects = DataManager.SubjectDB.GetAll();
    if (!subjects.Any())
    {
        Print.Error("No subjects available. Create a subject first.");
        return;
    }

    Print.Info("Available Subjects:");
    for (int i = 0; i < subjects.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {subjects[i].Name}");
    }

    Console.Write("\nSelect subject: ");
    int subjectChoice = int.Parse(Console.ReadLine() ?? "1") - 1;
    var subject = subjects[subjectChoice];

    // Exam details
    Console.Write("Exam Title: ");
    string title = Console.ReadLine() ?? "";

    Console.Write("Time Limit (minutes): ");
    int timeLimit = int.Parse(Console.ReadLine() ?? "60");

    Console.Write("Exam Type (Midterm/Final/Quiz): ");
    string type = Console.ReadLine() ?? "Midterm";

    var exam = new StoredExam(subject.Id, title, timeLimit, type);

    // Add questions
    bool addingQuestions = true;
    while (addingQuestions)
    {
        Console.WriteLine("\n1. Add True/False Question");
        Console.WriteLine("2. Add Multiple Choice Question");
        Console.WriteLine("3. Finish");
        
        Console.Write("\nChoice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddTrueFalseQuestion(exam);
                break;
            case "2":
                AddMultipleChoiceQuestion(exam);
                break;
            case "3":
                addingQuestions = false;
                break;
        }
    }

    DataManager.ExamDB.Add(exam);
    Print.Success($"Exam '{title}' created with {exam.Questions.Count} questions!");
}

private static void AddTrueFalseQuestion(StoredExam exam)
{
    Console.Write("\nQuestion: ");
    string question = Console.ReadLine() ?? "";

    Console.Write("Mark: ");
    double mark = double.Parse(Console.ReadLine() ?? "5");

    Console.Write("Correct Answer (true/false): ");
    bool answer = bool.Parse(Console.ReadLine() ?? "true");

    var q = new StoredQuestion
    {
        Header = question,
        Mark = mark,
        IsTrueFalse = true,
        Answer = answer
    };

    exam.Questions.Add(q);
    Print.Success("Question added!");
}

private static void AddMultipleChoiceQuestion(StoredExam exam)
{
    Console.Write("\nQuestion: ");
    string question = Console.ReadLine() ?? "";

    Console.Write("Mark: ");
    double mark = double.Parse(Console.ReadLine() ?? "5");

    Console.Write("Number of options: ");
    int optionCount = int.Parse(Console.ReadLine() ?? "4");

    var answers = new List<StoredAnswer>();
    for (int i = 0; i < optionCount; i++)
    {
        Console.Write($"\nOption {i + 1}: ");
        string text = Console.ReadLine() ?? "";

        Console.Write("Is this correct? (y/n): ");
        bool isCorrect = Console.ReadLine()?.ToLower() == "y";

        answers.Add(new StoredAnswer
        {
            Text = text,
            IsCorrect = isCorrect
        });
    }

    var q = new StoredQuestion
    {
        Header = question,
        Mark = mark,
        IsTrueFalse = false,
        Answers = answers
    };

    exam.Questions.Add(q);
    Print.Success("Question added!");
}
```

### Viewing Statistics

```csharp
public static void ViewStatistics()
{
    Print.Title("System Statistics");
    Print.Line();

    // User statistics
    var users = DataManager.UserDB.GetAll();
    var students = users.Where(u => !u.IsAdmin).ToList();
    var admins = users.Where(u => u.IsAdmin).ToList();

    Print.Info($"Total Users: {users.Count}");
    Print.Info($"  - Students: {students.Count}");
    Print.Info($"  - Admins: {admins.Count}");

    // Subject statistics
    var subjects = DataManager.SubjectDB.GetAll();
    Print.Info($"\nTotal Subjects: {subjects.Count}");

    // Exam statistics
    var exams = DataManager.ExamDB.GetAll();
    Print.Info($"Total Exams: {exams.Count}");

    // Result statistics
    var results = DataManager.ResultDB.GetAll();
    if (results.Any())
    {
        Print.Info($"\nTotal Exam Attempts: {results.Count}");
        Print.Info($"Average Score: {results.Average(r => r.Percentage):F2}%");
        Print.Info($"Highest Score: {results.Max(r => r.Percentage):F2}%");
        Print.Info($"Lowest Score: {results.Min(r => r.Percentage):F2}%");

        // Passing rate (>= 60%)
        int passed = results.Count(r => r.Percentage >= 60);
        double passRate = (double)passed / results.Count * 100;
        Print.Info($"Pass Rate: {passRate:F2}%");
    }

    // Subject-wise statistics
    Print.Info("\nSubject-wise Statistics:");
    foreach (var subject in subjects)
    {
        var subjectExams = exams.Where(e => e.SubjectId == subject.Id);
        var subjectResults = results.Where(r => 
            subjectExams.Any(e => e.Id == r.ExamId));

        Console.WriteLine($"\n  {subject.Name}:");
        Console.WriteLine($"    Exams: {subjectExams.Count()}");
        Console.WriteLine($"    Attempts: {subjectResults.Count()}");
        
        if (subjectResults.Any())
        {
            Console.WriteLine($"    Avg Score: {subjectResults.Average(r => r.Percentage):F2}%");
        }
    }

    Print.Line();
}
```

---

## Utility Functions

### Print Examples

```csharp
// Display a title
Print.Title("Welcome to Exam System");

// Success message
Print.Success("Exam completed successfully!");

// Error message
Print.Error("Invalid input. Please try again.");

// Info message
Print.Info("You have 3 exams available.");

// Warning message
Print.Warning("Time remaining: 5 minutes");

// Separator line
Print.Line();

// Custom colored text
Print.ColoredText("Important Notice", ConsoleColor.Magenta);

// Combining prints
Print.Title("Exam Results");
Print.Line();
Print.Success($"Score: 85/100");
Print.Info($"Percentage: 85%");
Print.Line();
```

### Input Handler Examples

```csharp
// Read string input
string name = Input_Handler.ReadString("Enter your name: ");

// Read integer input (with validation)
int age = Input_Handler.ReadInt("Enter your age: ");

// Read boolean input
bool isAdmin = Input_Handler.ReadBool("Is admin? (true/false): ");

// Read email with validation
string email = Input_Handler.ReadEmail("Enter your email: ");

// Read password (masked input)
string password = Input_Handler.ReadPassword("Enter password: ");

// Read with default value
Console.Write("Enter time limit (default 60): ");
string input = Console.ReadLine();
int timeLimit = string.IsNullOrEmpty(input) ? 60 : int.Parse(input);
```

### Arrow Menu Examples

```csharp
// Simple menu
var options = new List<string>
{
    "Start Exam",
    "View Results",
    "Exit"
};

int choice = Arrow_Menu.ShowMenu(options, "Main Menu");

switch (choice)
{
    case 0:
        Console.WriteLine("Starting exam...");
        break;
    case 1:
        Console.WriteLine("Viewing results...");
        break;
    case 2:
        Console.WriteLine("Exiting...");
        break;
    case -1:
        Console.WriteLine("Cancelled");
        break;
}

// Dynamic menu from database
var subjects = DataManager.SubjectDB.GetAll();
var subjectNames = subjects.Select(s => s.Name).ToList();
subjectNames.Add("Go Back");

int selected = Arrow_Menu.ShowMenu(subjectNames, "Select Subject");

if (selected >= 0 && selected < subjects.Count)
{
    var selectedSubject = subjects[selected];
    Console.WriteLine($"Selected: {selectedSubject.Name}");
}
```

---

## Complete Workflows

### Complete User Registration Flow

```csharp
public void RegisterUser(Router router)
{
    Print.Title("User Registration");
    Print.Line();

    // Get username
    Console.Write("Username: ");
    string username = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(username))
    {
        Print.Error("Username cannot be empty!");
        router.RerouteCurrent();
        return;
    }

    // Check username uniqueness
    var existingUser = DataManager.UserDB.GetAll()
        .FirstOrDefault(u => u.Username == username);
    
    if (existingUser != null)
    {
        Print.Error("Username already exists!");
        router.RerouteCurrent();
        return;
    }

    // Get email
    Console.Write("Email: ");
    string email = Console.ReadLine() ?? "";

    if (!email.Contains("@"))
    {
        Print.Error("Invalid email format!");
        router.RerouteCurrent();
        return;
    }

    // Check email uniqueness
    existingUser = DataManager.UserDB.GetAll()
        .FirstOrDefault(u => u.Email == email);
    
    if (existingUser != null)
    {
        Print.Error("Email already registered!");
        router.RerouteCurrent();
        return;
    }

    // Get password
    Console.Write("Password: ");
    string password = Console.ReadLine() ?? "";

    Console.Write("Confirm Password: ");
    string confirmPassword = Console.ReadLine() ?? "";

    if (password != confirmPassword)
    {
        Print.Error("Passwords don't match!");
        router.RerouteCurrent();
        return;
    }

    // Create and save user
    var user = new User(username, email, password, false);
    DataManager.UserDB.Add(user);

    Print.Success("Registration successful!");
    Thread.Sleep(1500);
    router.Navigate("login");
}
```

### Complete Exam Taking Flow

```csharp
public void TakeExam(int examId, Router router)
{
    // Load exam
    var exam = DataManager.ExamDB.GetById(examId);
    if (exam == null)
    {
        Print.Error("Exam not found!");
        router.Navigate("subjects");
        return;
    }

    // Display exam info
    Print.Title($"Exam: {exam.Title}");
    Print.Info($"Time Limit: {exam.TimeLimit} minutes");
    Print.Info($"Total Questions: {exam.Questions.Count}");
    Print.Info($"Total Marks: {exam.GetTotalMarks()}");
    Print.Line();

    Console.Write("Press Enter to start...");
    Console.ReadLine();

    // Start timer
    var startTime = DateTime.Now;
    double userScore = 0;

    // Present questions
    for (int i = 0; i < exam.Questions.Count; i++)
    {
        var question = exam.Questions[i];
        
        Console.Clear();
        Print.Title($"Question {i + 1}/{exam.Questions.Count}");
        Print.Info($"Marks: {question.Mark}");
        Print.Line();
        Console.WriteLine(question.Header);
        Console.WriteLine();

        if (question.IsTrueFalse)
        {
            // True/False question
            Console.WriteLine("1. True");
            Console.WriteLine("2. False");
            Console.Write("\nYour answer: ");
            
            string input = Console.ReadLine();
            bool userAnswer = input == "1";

            if (userAnswer == question.Answer)
            {
                userScore += question.Mark;
                Print.Success("Correct!");
            }
            else
            {
                Print.Error("Incorrect!");
            }
        }
        else
        {
            // Multiple choice question
            for (int j = 0; j < question.Answers.Count; j++)
            {
                Console.WriteLine($"{j + 1}. {question.Answers[j].Text}");
            }
            
            Console.Write("\nYour answer: ");
            int answerIndex = int.Parse(Console.ReadLine() ?? "1") - 1;

            if (answerIndex >= 0 && answerIndex < question.Answers.Count &&
                question.Answers[answerIndex].IsCorrect)
            {
                userScore += question.Mark;
                Print.Success("Correct!");
            }
            else
            {
                Print.Error("Incorrect!");
            }
        }

        Thread.Sleep(1000);

        // Check time limit
        var elapsed = (DateTime.Now - startTime).TotalMinutes;
        if (elapsed > exam.TimeLimit)
        {
            Print.Warning("Time's up!");
            break;
        }
    }

    // Calculate results
    var endTime = DateTime.Now;
    var timeTaken = (int)(endTime - startTime).TotalMinutes;
    var totalMarks = exam.GetTotalMarks();
    var percentage = (userScore / totalMarks) * 100;

    // Save result
    var result = new ExamResultRecord
    {
        UserId = AppState.CurrentUser.Id,
        ExamId = examId,
        Score = userScore,
        TotalMarks = totalMarks,
        Percentage = percentage,
        CompletionDate = DateTime.Now,
        TimeTaken = timeTaken
    };

    DataManager.ResultDB.Add(result);

    // Update user history
    var user = DataManager.UserDB.GetById(AppState.CurrentUser.Id);
    user.ExamHistory.Add(result.Id);
    DataManager.UserDB.Update(user.Id, user);

    // Display results
    Console.Clear();
    Print.Title("Exam Complete!");
    Print.Line();
    Print.Info($"Score: {userScore}/{totalMarks}");
    Print.Info($"Percentage: {percentage:F2}%");
    Print.Info($"Time Taken: {timeTaken} minutes");
    
    if (percentage >= 60)
    {
        Print.Success("PASSED!");
    }
    else
    {
        Print.Error("FAILED");
    }

    Print.Line();
    Console.Write("Press Enter to continue...");
    Console.ReadLine();

    router.Navigate("main");
}
```

---

## Error Handling Examples

### Try-Catch in Database Operations

```csharp
try
{
    var user = DataManager.UserDB.GetById(userId);
    if (user == null)
    {
        throw new Exception("User not found");
    }
    
    user.Username = newUsername;
    DataManager.UserDB.Update(userId, user);
    
    Print.Success("User updated successfully!");
}
catch (Exception ex)
{
    Print.Error($"Error: {ex.Message}");
    router.RerouteCurrent();
}
```

### Input Validation

```csharp
public int GetValidInteger(string prompt, int min, int max)
{
    while (true)
    {
        try
        {
            Console.Write(prompt);
            int value = int.Parse(Console.ReadLine() ?? "0");
            
            if (value < min || value > max)
            {
                Print.Error($"Value must be between {min} and {max}");
                continue;
            }
            
            return value;
        }
        catch (FormatException)
        {
            Print.Error("Please enter a valid number");
        }
    }
}

// Usage
int age = GetValidInteger("Enter age (1-100): ", 1, 100);
```

---

## Best Practices

### 1. Always Check Authentication

```csharp
if (!AppState.IsLoggedIn)
{
    Print.Error("Please login first!");
    router.Navigate("login");
    return;
}
```

### 2. Validate Input

```csharp
if (string.IsNullOrWhiteSpace(input))
{
    Print.Error("Input cannot be empty!");
    return;
}
```

### 3. Use Clear Messages

```csharp
Print.Success("✓ Operation completed successfully!");
Print.Error("✗ Operation failed. Please try again.");
Print.Info("ℹ Information: ...");
```

### 4. Handle Edge Cases

```csharp
var results = DataManager.ResultDB.GetAll();
if (!results.Any())
{
    Print.Info("No results found.");
    return;
}

double average = results.Average(r => r.Percentage);
```

---

This documentation provides practical examples for implementing features in the Exam Console Application. Use these patterns as templates for extending and customizing the system.
