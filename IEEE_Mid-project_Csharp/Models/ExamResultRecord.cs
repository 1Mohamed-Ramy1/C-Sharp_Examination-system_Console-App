namespace MID_PROJ.Models;
using MID_PROJ.Services;
using Newtonsoft.Json;

public class ExamResultRecord : IIdentifiable
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ExamId { get; set; }
    public double Score { get; set; }
    public double TotalMarks { get; set; }
    public double Percentage { get; set; }
    public string Grade { get; set; }
    public bool Passed { get; set; }
    public DateTime DateTaken { get; set; }
    public TimeSpan TimeTaken { get; set; }
    public List<UserAnswer> UserAnswers { get; set; }
    [JsonConstructor]
    public ExamResultRecord()
    {
        Grade = "";
        UserAnswers = new List<UserAnswer>();
        DateTaken = DateTime.Now;
    }

    public ExamResultRecord(int userId, int examId, double score, double totalMarks)
    {
        UserId = userId;
        ExamId = examId;
        Score = score;
        TotalMarks = totalMarks;
        Percentage = (score / totalMarks) * 100;
        Grade = CalculateGrade(Percentage);
        Passed = Percentage >= 50;
        DateTaken = DateTime.Now;
        UserAnswers = new List<UserAnswer>();
    }

    private string CalculateGrade(double percentage)
    {
        if(percentage >=95) return "A+";
        if(percentage >=90) return "A";
        if(percentage >=85) return "B+";
        if(percentage >=80) return "B";
        if(percentage >=75) return "C+";
        if(percentage >=70) return "C";
        if(percentage >=65) return "D+";
        if(percentage >=60) return "D";
        return "F";
    }
}

public class UserAnswer
{
    public int QuestionId { get;set; }
    public string AnswerText { get;set; }
    public bool IsCorrect { get; set; }

    [JsonConstructor]
    public UserAnswer()
    {
        AnswerText = "";
    }
    public UserAnswer(int questionId, string answerText, bool isCorrect)
    {
        QuestionId = questionId;
        AnswerText = answerText;
        IsCorrect = isCorrect;
    }
}