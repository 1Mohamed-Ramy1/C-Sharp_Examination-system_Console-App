using System.IO;
using System.Linq;
using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class DeleteExamPage : Page
{
    public override void Display()
    {
        Print.OutLine("🗑️ DELETE EXAM", ConsoleColor.Red);
        Console.WriteLine(new string('═', 60));
    }

    public override void HandleInput(Router router)
    {
        Console.Clear();
        Display();
        Console.WriteLine();

        var exams = DataManager.ExamDB.GetAll();
        var subjects = DataManager.SubjectDB.GetAll();
        if(exams.Count ==0)
        {
            Print.OutLine("No exams available to delete.",ConsoleColor.Yellow);
            Print.Pause();
            router.Navigate("admin");
            return;
        }

        // Display all exams
        Print.OutLine("Available Exams:", ConsoleColor.Cyan);
        Console.WriteLine();

        var examChoices = exams.Select(e =>
        {
            var subject = subjects.FirstOrDefault(s => s.Id ==e.SubjectId);
            return $"{e.Title} ({subject?.Name ?? "Unknown"})-{e.Questions.Count} Questions";
        }).ToList();

        examChoices.Add("🔙 Back to Admin Panel");
        var selectedExam = Print.AskChoice("Select exam to delete:", examChoices);
        if(selectedExam =="__ESC__" || selectedExam =="🔙 Back to Admin Panel")
        {
            router.Navigate("admin");
            return;
        }

        int examIndex = examChoices.IndexOf(selectedExam);
        var exam = exams[examIndex];

        //Show exam details
        Console.Clear();
        Display();
        Console.WriteLine();
        var subject = subjects.FirstOrDefault(s => s.Id == exam.SubjectId);
        Print.OutLine("Exam Details:", ConsoleColor.Yellow);
        Console.WriteLine($"Title: {exam.Title}");
        Console.WriteLine($"Subject: {subject?.Name ?? "Unknown"}");
        Console.WriteLine($"Type: {exam.Type}");
        Console.WriteLine($"Questions: {exam.Questions.Count}");
        Console.WriteLine($"Time Limit: {exam.TimeLimit} minutes");
        Console.WriteLine($"Total Marks: {exam.GetTotalMarks()}");
        Console.WriteLine();

        //Check if exam has results
        var examResults = DataManager.ResultDB.GetAll().Where(r => r.ExamId == exam.Id).ToList();
        if(examResults.Count >0)
        {
            Print.OutLine($"⚠️ Warning: This exam has {examResults.Count} student results!", ConsoleColor.Yellow);
            Print.OutLine("Deleting this exam will also delete all associated results.", ConsoleColor.Yellow);
            Console.WriteLine();
        }

        var confirmation = Print.AskChoice(
            "⚠️ Are you sure you want to delete this exam?",
            new List<string> { "Yes, Delete Permanently","No, Cancel" }
        );

        if(confirmation =="Yes, Delete Permanently")
        {
            //Delete the exam
            DataManager.ExamDB.Delete(exam.Id);

            //Delete associated results
            foreach (var result in examResults)
            {
                DataManager.ResultDB.Delete(result.Id);
            }

            //Remove exam from user histories
            var users =DataManager.UserDB.GetAll();
            foreach(var user in users)
            {
                if(user.ExamHistory.Contains(exam.Id))
                {
                    user.ExamHistory.Remove(exam.Id);
                    DataManager.UserDB.Update(user.Id, user);
                }
            }

            Print.OutLine("✓ Exam deleted successfully!", ConsoleColor.Green);
            if (examResults.Count > 0)
            {
                Print.OutLine($"✓ {examResults.Count} associated results deleted.", ConsoleColor.Green);
            }
        }
        else
        {
            Print.OutLine("Deletion cancelled.",ConsoleColor.Yellow);
        }

        Print.Pause();
        router.Navigate("admin");
    }
}