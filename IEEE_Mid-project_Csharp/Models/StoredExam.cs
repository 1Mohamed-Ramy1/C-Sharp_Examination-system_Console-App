namespace MID_PROJ.Models;
using MID_PROJ.Services;
using Newtonsoft.Json;

public class StoredExam : IIdentifiable
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public string Title { get; set; }
    public int TimeLimit { get; set; }
    public string Type { get; set; }
    public List<StoredQuestion> Questions { get; set; }

    [JsonConstructor]
    public StoredExam()
    {
        Title = "";
        Type = "Final";
        Questions = new List<StoredQuestion>();
    }

    public StoredExam(int subjectId, string title, int timeLimit, string type)
    {
        SubjectId = subjectId;
        Title = title;
        TimeLimit = timeLimit;
        Type = type;
        Questions = new List<StoredQuestion>();
    }

    public double GetTotalMarks()
    {
        return Questions.Sum(q => q.Mark);
    }
}

public class StoredQuestion
{
    public int Id { get; set; }
    public string Header { get; set; }
    public double Mark { get; set; }
    public bool IsTrueFalse { get; set; }
    public bool? Answer { get; set; }
    public List<StoredAnswer>? Answers { get; set; }

    [JsonConstructor]
    public StoredQuestion()
    {
        Header = "";
        Mark = 5;
        IsTrueFalse = false;
    }

    public StoredQuestion(string header, double mark, bool isTrueFalse, bool? answer = null)
    {
        Header = header;
        Mark = mark;
        IsTrueFalse = isTrueFalse;
        Answer = answer;
        Answers = isTrueFalse ? null : new List<StoredAnswer>();
    }
}

public class StoredAnswer
{
    public string AnswerText { get; set; }
    public bool IsRightAnswer { get; set; }

    [JsonConstructor]
    public StoredAnswer()
    {
        AnswerText = "";
        IsRightAnswer = false;
    }

    public StoredAnswer(string answerText, bool isRightAnswer)
    {
        AnswerText = answerText;
        IsRightAnswer = isRightAnswer;
    }
}
