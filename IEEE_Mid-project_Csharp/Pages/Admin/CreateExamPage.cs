using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class CreateExamPage : Page
{
    public override void Display()
    {
        var welcomeName = AppState.CurrentUser?.IsAdmin == true ? "ELOSTORA" : AppState.CurrentUser?.Username;
        if (!string.IsNullOrWhiteSpace(welcomeName))
        {
            Print.OutLine($"👋 Welcome {welcomeName}", ConsoleColor.Yellow);
        }
        Print.OutLine("➕ CREATE NEW EXAM", ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═', 60));
    }

    public override void HandleInput(Router router)
    {
        if (AppState.CurrentUser == null || !AppState.CurrentUser.IsAdmin)
        {
            router.Navigate("main");
            return;
        }

        Console.WriteLine();
        var subjects = DataManager.SubjectDB.GetAll();
        
        if (subjects.Count == 0)
        {
            Print.OutLine("No subjects available. Please add subjects first.", ConsoleColor.Yellow);
            Print.Pause();
            router.Navigate("admin");
            return;
        }

        var step = 0;
        var subjectNames = subjects.Select(s => s.Name).ToList();
        Subject? subject = null;
        string title = "";
        int timeLimit = 0;
        string examType = "";

        while (true)
        {
            if (step == 0)
            {
                var selectedSubjectName = Print.AskChoice("Select Subject:", subjectNames, allowBack: true);

                if (selectedSubjectName == "⬅ Back" || selectedSubjectName == "__ESC__")
                {
                    router.Navigate("admin");
                    return;
                }

                subject = subjects.First(s => s.Name == selectedSubjectName);
                step = 1;
                Console.WriteLine();
            }
            else if (step == 1)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Exam Title: ");
                Console.ResetColor();
                if (Print.CancelableInput(out string tempTitle, "", false))
                {
                    step = 0;
                    Console.WriteLine();
                    continue;
                }
                title = tempTitle.Trim();
                if (string.IsNullOrWhiteSpace(title))
                {
                    Print.ErrorMsg("Exam title cannot be empty");
                    continue;
                }
                step = 2;
                Console.WriteLine();
            }
            else if (step == 2)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Time Limit (5-120 minutes): ");
                Console.ResetColor();

                if (Print.CancelableInput(out string timeInput, "", false))
                {
                    step = 1;
                    Console.WriteLine();
                    continue;
                }

                if (!int.TryParse(timeInput, out timeLimit))
                {
                    Print.ErrorMsg("Invalid input. Please enter a number");
                    continue;
                }

                if (timeLimit < 5 || timeLimit > 120)
                {
                    Print.ErrorMsg("Invalid input. Time limit must be between 5 and 120 minutes");
                    continue;
                }
                step = 3;
                Console.WriteLine();
            }
            else
            {
                var selectedType = Print.AskChoice("Exam Type:", new List<string> { "Quiz", "Final", "Practical" }, allowBack: true);
                if (selectedType == "⬅ Back" || selectedType == "__ESC__")
                {
                    step = 2;
                    Console.WriteLine();
                    continue;
                }
                examType = selectedType;
                break;
            }
        }

        var questions = new List<StoredQuestion>();
        var nextQuestionId = 0;

        while (true)
        {
            Console.WriteLine();
            var action = Print.AskChoice("Add Questions", new List<string>
            {
                "Add True/False Question",
                "Add Multiple Choice Question",
                "Finish and Save"
            });

            if (action == "Finish and Save")
            {
                if (questions.Count == 0)
                {
                    Print.ErrorMsg("Please add at least one question before saving.");
                    continue;
                }
                break;
            }

            if (Print.CancelableInput(out string header, "Question: "))
                continue;
            header = header.Trim();
            if (string.IsNullOrWhiteSpace(header))
            {
                Print.ErrorMsg("Question cannot be empty");
                continue;
            }

            if (Print.CancelableNumber(out double mark, "Mark: "))
                continue;
            if (mark <= 0)
            {
                Print.ErrorMsg("Mark must be greater than 0");
                continue;
            }

            if (action == "Add True/False Question")
            {
                var correct = Print.AskChoice("Correct Answer:", new List<string> { "True", "False" });
                var question = new StoredQuestion
                {
                    Id = nextQuestionId++,
                    Header = header,
                    Mark = mark,
                    IsTrueFalse = true,
                    Answer = correct == "True",
                    Answers = null
                };
                questions.Add(question);
                Print.SuccessMsg("✅ Question added");
            }
            else
            {
                if (Print.CancelableNumber(out int optionCount, "Number of options (2-6): "))
                    continue;
                if (optionCount < 2 || optionCount > 6)
                {
                    Print.ErrorMsg("Options must be between 2 and 6");
                    continue;
                }

                var answers = new List<StoredAnswer>();
                for (int i = 0; i < optionCount; i++)
                {
                    if (Print.CancelableInput(out string optionText, $"Option {i + 1}: "))
                    {
                        answers.Clear();
                        break;
                    }
                    optionText = optionText.Trim();
                    if (string.IsNullOrWhiteSpace(optionText))
                    {
                        Print.ErrorMsg("Option text cannot be empty");
                        i--;
                        continue;
                    }
                    answers.Add(new StoredAnswer(optionText, false));
                }
                if (answers.Count != optionCount)
                    continue;

                if (Print.CancelableNumber(out int correctIndex, $"Correct option (1-{optionCount}): "))
                    continue;
                if (correctIndex < 1 || correctIndex > optionCount)
                {
                    Print.ErrorMsg("Invalid correct option index");
                    continue;
                }
                answers[correctIndex - 1].IsRightAnswer = true;

                var question = new StoredQuestion
                {
                    Id = nextQuestionId++,
                    Header = header,
                    Mark = mark,
                    IsTrueFalse = false,
                    Answers = answers
                };
                questions.Add(question);
                Print.SuccessMsg("✅ Question added");
            }
        }

        var newExam = new StoredExam
        {
            Id = DataManager.ExamDB.GetAll().Count,
            SubjectId = subject!.Id,
            Title = title,
            TimeLimit = timeLimit,
            Type = examType,
            Questions = questions
        };

        DataManager.ExamDB.Add(newExam);

        Console.WriteLine();
        Print.SuccessMsg("✅ Exam created successfully with questions!");

        Print.Pause();
        router.Navigate("admin");
    }
}
