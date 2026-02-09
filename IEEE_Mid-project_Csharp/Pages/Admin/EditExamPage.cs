using System.IO;
using System.Linq;
using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class EditExamPage : Page
{
    public override void Display()
    {
        Print.OutLine("✏️ EDIT EXAM", ConsoleColor.Yellow);
        Console.WriteLine(new string('═',60));
    }

    public override void HandleInput(Router router)
    {
        Console.Clear();
        Display();
        Console.WriteLine();

        var exams =DataManager.ExamDB.GetAll();
        var subjects =DataManager.SubjectDB.GetAll();
        if(exams.Count ==0)
        {
            Print.OutLine("No exams available to edit.", ConsoleColor.Yellow);
            Print.Pause();
            router.Navigate("admin");
            return;
        }

        var examChoices =exams.Select(e =>
        {
            var subject = subjects.FirstOrDefault(s => s.Id == e.SubjectId);
            return $"{e.Title} ({subject?.Name ?? "Unknown"}) - {e.Questions.Count} Questions";
        }).ToList();

        var selectedExam = Print.AskChoice("Select exam to edit:", examChoices);
        if(selectedExam =="__ESC__")
        {
            router.Navigate("admin");
            return;
        }

        int examIndex =examChoices.IndexOf(selectedExam);
        var exam =exams[examIndex];

        bool editing = true;
        while(editing)
        {
            Console.Clear();
            Display();
            Console.WriteLine();
            Print.OutLine($"Editing: {exam.Title}", ConsoleColor.Cyan);
            Console.WriteLine();

            var editChoice = Print.AskChoice("What would you like to edit?",new List<string>
            {
                "Edit Exam Title",
                "Edit Time Limit",
                "Edit Exam Type",
                "Edit Questions",
                "🔙 Back to Admin Panel"
            });

            if(editChoice =="__ESC__" || editChoice =="🔙 Back to Admin Panel")
            {
                router.Navigate("admin");
                return;
            }

            switch(editChoice)
            {
                case "Edit Exam Title":
                    EditTitle(exam);
                    break;
                case "Edit Time Limit":
                    EditTimeLimit(exam);
                    break;
                case "Edit Exam Type":
                    EditExamType(exam);
                    break;
                case "Edit Questions":
                    EditQuestions(exam);
                    break;
            }
        }
    }

    private void EditTitle(StoredExam exam)
    {
        Console.Clear();
        Print.OutLine($"Current Title: {exam.Title}", ConsoleColor.Yellow);
        Console.WriteLine();
        Print.PrintFixedESCMessage();

        if(Print.CancelableInput(out string newTitle, "Enter new title: "))
            return;
        if(!string.IsNullOrWhiteSpace(newTitle))
        {
            exam.Title = newTitle;
            DataManager.ExamDB.Update(exam.Id, exam);
            Print.OutLine("✓ Title updated and saved!", ConsoleColor.Green);
        }
        else
        {
            Print.OutLine("Title not changed.", ConsoleColor.Yellow);
        }
        Print.Pause();
    }

    private void EditTimeLimit(StoredExam exam)
    {
        Console.Clear();
        Print.OutLine($"Current Time Limit: {exam.TimeLimit} minutes", ConsoleColor.Yellow);
        Console.WriteLine();
        Print.PrintFixedESCMessage();

        if(Print.CancelableInput(out string input, "Enter new time limit (minutes): "))
            return;
        if(int.TryParse(input, out int newTime) && newTime > 0)
        {
            exam.TimeLimit = newTime;
            DataManager.ExamDB.Update(exam.Id, exam);
            Print.OutLine("✓ Time limit updated and saved!", ConsoleColor.Green);
        }
        else if(!string.IsNullOrWhiteSpace(input))
        {
            Print.OutLine("Invalid time limit.", ConsoleColor.Red);
        }
        Print.Pause();
    }

    private void EditExamType(StoredExam exam)
    {
        Console.Clear();
        Print.OutLine($"Current Type: {exam.Type}", ConsoleColor.Yellow);
        Console.WriteLine();

        var typeChoice =Print.AskChoice("Select new exam type:", new List<string>
        {
            "Mid",
            "Final",
            "Quiz"
        });

        if(typeChoice !="__ESC__")
        {
            exam.Type =typeChoice;
            DataManager.ExamDB.Update(exam.Id, exam);
            Print.OutLine("✓ Exam type updated and saved!", ConsoleColor.Green);
            Print.Pause();
        }
    }

    private void EditQuestions(StoredExam exam)
    {
        bool editingQuestions = true;
        while(editingQuestions)
        {
            Console.Clear();
            Print.OutLine($"Questions in {exam.Title}", ConsoleColor.Cyan);
            Console.WriteLine();

            if(exam.Questions.Count ==0)
            {
                Print.OutLine("No questions in this exam.", ConsoleColor.Yellow);
                Print.Pause();
                return;
            }

            var questionChoices = new List<string>();
            for (int i =0; i <exam.Questions.Count;i++ )
            {
                var q = exam.Questions[i];
                string qType = q.IsTrueFalse ? "T/F" : "MCQ";
                questionChoices.Add($"Q{i + 1}: {q.Header} ({qType}) - {q.Mark} marks");
            }
            questionChoices.Add("➕ Add New Question");
            questionChoices.Add("🔙 Back");

            var choice = Print.AskChoice("Select question to edit or add new:", questionChoices);
            if (choice =="__ESC__" || choice == "🔙 Back")
            {
                return;
            }

            if (choice =="➕ Add New Question")
            {
                AddNewQuestion(exam);
            }
            else
            {
                int questionIndex = questionChoices.IndexOf(choice);
                EditSingleQuestion(exam, questionIndex);
            }
        }
    }

    private void AddNewQuestion(StoredExam exam)
    {
        Console.Clear();
        Print.OutLine("➕ Add New Question", ConsoleColor.Cyan);
        Console.WriteLine();
        Print.PrintFixedESCMessage();

        if(Print.CancelableInput(out string header,"Question text: "))
            return;
        if(string.IsNullOrWhiteSpace(header))
            return;

        if(Print.CancelableInput(out string markInput,"Question marks: "))
            return;
        if(!double.TryParse(markInput, out double mark) || mark <= 0)
        {
            Print.OutLine("Invalid marks value.", ConsoleColor.Red);
            Print.Pause();
            return;
        }

        var typeChoice = Print.AskChoice("Question type:", new List<string> { "True/False", "Multiple Choice" });
        if(typeChoice =="__ESC__")
            return;

        bool isTrueFalse =typeChoice=="True/False";
        var newQuestion =new StoredQuestion(header, mark, isTrueFalse);

        if(isTrueFalse)
        {
            var answerChoice = Print.AskChoice("Correct answer:", new List<string> { "True", "False" });
            if (answerChoice =="__ESC__")
                return;
            newQuestion.Answer = answerChoice == "True";
        }
        else
        {
            if(Print.CancelableInput(out string optCountInput, "Number of options (2-6): "))
                return;
            if(!int.TryParse(optCountInput, out int optCount) || optCount < 2 || optCount > 6)
            {
                Print.OutLine("Invalid option count.", ConsoleColor.Red);
                Print.Pause();
                return;
            }

            newQuestion.Answers = new List<StoredAnswer>();
            for (int i =0; i <optCount;i++ )
            {
                if(Print.CancelableInput(out string optText, $"Option {i + 1}: "))
                    return;
                if(string.IsNullOrWhiteSpace(optText))
                {
                    Print.OutLine("Option cannot be empty.", ConsoleColor.Red);
                    i--;
                    continue;
                }
                newQuestion.Answers.Add(new StoredAnswer(optText, false));
            }

            var correctChoices = newQuestion.Answers.Select(a => a.AnswerText).ToList();
            var correctAnswer = Print.AskChoice("Select correct answer:", correctChoices);
            if(correctAnswer !="__ESC__")
            {
                int correctIndex =correctChoices.IndexOf(correctAnswer);
                newQuestion.Answers[correctIndex].IsRightAnswer = true;
            }
        }

        newQuestion.Id = exam.Questions.Count > 0 ? exam.Questions.Max(q => q.Id) + 1 : 1;
        exam.Questions.Add(newQuestion);
        DataManager.ExamDB.Update(exam.Id, exam);
        Print.OutLine("✓ Question added and saved!", ConsoleColor.Green);
        Print.Pause();
    }

    private void EditSingleQuestion(StoredExam exam, int questionIndex)
    {
        var question = exam.Questions[questionIndex];
        bool editing = true;

        while(editing)
        {
            Console.Clear();
            Print.OutLine($"Editing Question {questionIndex + 1}", ConsoleColor.Cyan);
            Console.WriteLine();
            Print.OutLine($"Text: {question.Header}", ConsoleColor.White);
            Print.OutLine($"Marks: {question.Mark}", ConsoleColor.White);
            Print.OutLine($"Type: {(question.IsTrueFalse ? "True/False" : "Multiple Choice")}", ConsoleColor.White);
            Console.WriteLine();

            var editChoice = Print.AskChoice("What would you like to do?", new List<string>
            {
                "Edit Question Text",
                "Edit Marks",
                "Edit Answer/Options",
                "🗑️ Delete Question",
                "🔙 Back"
            });

            if (editChoice =="__ESC__" || editChoice =="🔙 Back")
            {
                return;
            }

            switch(editChoice)
            {
                case "Edit Question Text":
                    Print.PrintFixedESCMessage();
                    if (Print.CancelableInput(out string newHeader, "New question text: "))
                        break;
                    if (!string.IsNullOrWhiteSpace(newHeader))
                    {
                        question.Header = newHeader;
                        DataManager.ExamDB.Update(exam.Id, exam);
                        Print.OutLine("✓ Question text updated and saved!", ConsoleColor.Green);
                    }
                    Print.Pause();
                    break;

                case "Edit Marks":
                    Print.PrintFixedESCMessage();
                    if(Print.CancelableInput(out string markInput, "New marks: "))
                        break;
                    if(double.TryParse(markInput, out double newMark) && newMark > 0)
                    {
                        question.Mark = newMark;
                        DataManager.ExamDB.Update(exam.Id, exam);
                        Print.OutLine("✓ Marks updated and saved!", ConsoleColor.Green);
                    }
                    else if(!string.IsNullOrWhiteSpace(markInput))
                    {
                        Print.OutLine("Invalid marks value.", ConsoleColor.Red);
                    }
                    Print.Pause();
                    break;

                case "Edit Answer/Options":
                    EditQuestionAnswer(question, exam);
                    break;

                case "🗑️ Delete Question":
                    var confirm = Print.AskChoice("Are you sure you want to delete this question?", 
                        new List<string> { "Yes, Delete", "No, Cancel" });
                    if (confirm == "Yes, Delete")
                    {
                        Print.OutLine("✓ Question deleted and saved!", ConsoleColor.Green);
                        exam.Questions.RemoveAt(questionIndex);
                        DataManager.ExamDB.Update(exam.Id, exam);
                        Print.OutLine("✓ Question deleted and saved!", ConsoleColor.Green);
                        Print.Pause();
                        return;
                    }
                    break;
            }
        }
    }

    private void EditQuestionAnswer(StoredQuestion question, StoredExam exam)
    {
        Console.Clear();
        Print.OutLine("Edit Answer/Options", ConsoleColor.Cyan);
        Console.WriteLine();

        if (question.IsTrueFalse)
        {
            Print.OutLine($"Current Answer: {(question.Answer == true ? "True" : "False")}", ConsoleColor.Yellow);
            var newAnswer = Print.AskChoice("Select correct answer:", new List<string> { "True", "False" });
           
            if (newAnswer != "__ESC__")
            {
                question.Answer = newAnswer == "True";
                DataManager.ExamDB.Update(exam.Id, exam);
                Print.OutLine("✓ Answer updated and saved!", ConsoleColor.Green);
            }
        }
        else
        {
            if(question.Answers ==null || question.Answers.Count ==0)
            {

                Print.OutLine("No options available.", ConsoleColor.Red);
                Print.Pause();
                return;
            }
            Print.OutLine("Current Options:", ConsoleColor.Yellow);

            //---------------------------------------------------------------------------

            for (int i =0; i< question.Answers.Count; i++)
            {
                var marker = question.Answers[i].IsRightAnswer ? "✓" : " ";
                Console.WriteLine($"  [{marker}] {i + 1}. {question.Answers[i].AnswerText}");
            }
            Console.WriteLine();

            var correctChoices = question.Answers.Select(a => a.AnswerText).ToList();
            var correctAnswer = Print.AskChoice("Select correct answer:", correctChoices);
            if (correctAnswer != "__ESC__")
            {
                foreach (var ans in question.Answers)
                    ans.IsRightAnswer = false;

                int correctIndex = correctChoices.IndexOf(correctAnswer);
                question.Answers[correctIndex].IsRightAnswer = true;
                DataManager.ExamDB.Update(exam.Id, exam);
                Print.OutLine("✓ Correct answer updated and saved!", ConsoleColor.Green);
            }
        }
        Print.Pause();
    }
}