namespace MID_PROJ.Services;
using MID_PROJ.Models;

public static class DataManager
{
    public static JsonDatabase<User> UserDB { get; } = new("Data/users.json");
    public static JsonDatabase<Subject> SubjectDB { get; } = new("Data/subjects.json");
    public static JsonDatabase<StoredExam> ExamDB { get; } = new("Data/exams.json");
    public static JsonDatabase<ExamResultRecord> ResultDB { get; } = new("Data/results.json");
}
