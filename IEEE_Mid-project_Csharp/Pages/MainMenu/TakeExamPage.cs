using System.Linq;
using MID_PROJ.Routes;
using MID_PROJ.Services;
using MID_PROJ.Models;
using MID_PROJ.Utils;

namespace MID_PROJ.Pages;
public class TakeExamPage : Page
{
    private bool _timeExpired = false;
    private bool _escPressed = false;
    private bool _stopTimer = false;

    public override void Display()
    {
        var welcomeName = AppState.CurrentUser?.IsAdmin == true ? "ELOSTORA" : AppState.CurrentUser?.Username;
        if (!string.IsNullOrWhiteSpace(welcomeName))
        {
            Print.OutLine($"👋 Welcome {welcomeName}", ConsoleColor.Yellow);
        }
        Print.OutLine("📝 TAKE EXAM", ConsoleColor.DarkCyan);
        Console.WriteLine(new string('═', 60));
    }

    public override void HandleInput(Router router)
    {
        if (AppState.CurrentUser == null)
        {
            router.Navigate("home");
            return;
        }

        Print.PrintFixedESCMessage();
        Console.WriteLine();
        var subjects = DataManager.SubjectDB.GetAll();
        var exams = DataManager.ExamDB.GetAll();
        
        if (exams.Count == 0)
        {
            Print.OutLine("No exams available at the moment.", ConsoleColor.Yellow);
            Print.Pause();
            router.Navigate("main");
            return;
        }

        var examChoices = new List<string>();
        foreach (var exam in exams)
        {
            var subject = subjects.FirstOrDefault(s => s.Id == exam.SubjectId);
            if (subject != null)
            {
                // Check if exam was already taken and time expired
                var userResults = DataManager.ResultDB.GetAll()
                    .Where(r => r.UserId == AppState.CurrentUser.Id && r.ExamId == exam.Id)
                    .OrderByDescending(r => r.DateTaken)
                    .FirstOrDefault();

                string status = "";
                if (userResults != null)
                {
                    var timeSinceExam = DateTime.Now - userResults.DateTaken;
                    if (timeSinceExam.TotalMinutes >= exam.TimeLimit)
                    {
                        status = " [COMPLETED - Time Expired]";
                    }
                }

                examChoices.Add($"{subject.Name} - {exam.Type} ({exam.Questions.Count} q, {exam.TimeLimit} min){status}");
            }
        }

        var choice = Print.AskChoice("Select an Exam", examChoices, allowBack: true);
        if (choice == "⬅ Back")
        {
            router.Navigate("main");
            return;
        }

        var selectedIndex = examChoices.IndexOf(choice);
        var selectedExam = exams[selectedIndex];
        var selectedSubject = subjects.First(s => s.Id == selectedExam.SubjectId);

        // Check if exam already taken and time expired
        var existingResult = DataManager.ResultDB.GetAll()
            .Where(r => r.UserId == AppState.CurrentUser.Id && r.ExamId == selectedExam.Id)
            .OrderByDescending(r => r.DateTaken)
            .FirstOrDefault();

        if (existingResult != null)
        {
            var timeSinceExam = DateTime.Now - existingResult.DateTaken;
            if (timeSinceExam.TotalMinutes >= selectedExam.TimeLimit)
            {
                Console.WriteLine();
                Print.OutLine("❌ Time limit expired for this exam. You cannot retake it.", ConsoleColor.Red);
                Print.Pause();
                router.Navigate("main");
                return;
            }
        }

        Console.WriteLine();
        Print.OutLine("⚠️  EXAM INSTRUCTIONS:", ConsoleColor.Yellow);
        Console.WriteLine($"   Subject: {selectedSubject.Name}");
        Console.WriteLine($"   Type: {selectedExam.Type}");
        Console.WriteLine($"   Questions: {selectedExam.Questions.Count}");
        Console.WriteLine($"   Time: {selectedExam.TimeLimit} minutes");
        Console.WriteLine($"   Total Marks: {selectedExam.GetTotalMarks()}");
        Console.WriteLine();

        if (!Print.AskYesNo("Start the exam?", true))
        {
            router.Navigate("main");
            return;
        }

        TakeExam(selectedExam, selectedSubject, router);
        router.Navigate("main");
    }

    private void TakeExam(StoredExam exam, Subject subject, Router router)
    {
        _timeExpired = false;
        _escPressed = false;
        _stopTimer = false;
        Console.Clear();
        Print.OutLine($"📝 {exam.Title}", ConsoleColor.Cyan);
        Console.WriteLine(new string('═', 60));
        Print.PrintFixedESCMessage();
        Console.WriteLine();

        var startTime = DateTime.Now;
        var endTime = startTime.AddMinutes(exam.TimeLimit);
        double score = 0;
        var userAnswers = new List<UserAnswer>();
        int currentQuestionIndex = 0;

        // Timer thread
        var timerThread = new Thread(() => MonitorTimer(endTime));
        timerThread.IsBackground = true;
        timerThread.Start();

        while (currentQuestionIndex < exam.Questions.Count && !_timeExpired && !_escPressed)
        {
            var question = exam.Questions[currentQuestionIndex];
            
            Console.Clear();
            Print.OutLine($"📝 {exam.Title}", ConsoleColor.Cyan);
            Console.WriteLine(new string('═', 60));
            Print.PrintFixedESCMessage();
            Console.WriteLine();

            // Display remaining time
            var remainingTime = endTime - DateTime.Now;
            if (remainingTime.TotalSeconds < 0) remainingTime = TimeSpan.Zero;
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⏱️  Time Remaining: {remainingTime.Minutes}m {remainingTime.Seconds}s");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Question {currentQuestionIndex + 1} of {exam.Questions.Count} ({question.Mark} marks)");
            Console.ResetColor();
            Console.WriteLine(question.Header);
            Console.WriteLine();

            bool answered = false;
            bool isCorrect = false;
            string answerText = "";
            bool exitExam = false;

            if (question.IsTrueFalse)
            {
                var answer = Print.AskChoice("Your answer", new List<string> { "True", "False" }, allowBack: true);
                if (answer == "⬅ Back" || answer == "__ESC__")
                {
                    if (ConfirmExitExam())
                    {
                        _escPressed = true;
                        exitExam = true;
                    }
                }
                else
                {
                    bool userAnswer = answer == "True";
                    isCorrect = question.Answer == userAnswer;
                    answerText = answer;
                    answered = true;

                    if (isCorrect) score += question.Mark;
                    userAnswers.Add(new UserAnswer(question.Id, answerText, isCorrect));
                }
            }
            else if (question.Answers != null)
            {
                for (int j = 0; j < question.Answers.Count; j++)
                {
                    Console.WriteLine($"   {(char)('A' + j)}. {question.Answers[j].AnswerText}");
                }
                Console.WriteLine();

                while (!answered)
                {
                    Console.Write("Your answer (A, B, C...): ");
                    if (Print.CancelableInput(out string rawInput, "", false))
                    {
                        if (ConfirmExitExam())
                        {
                            _escPressed = true;
                            exitExam = true;
                            break;
                        }
                        continue;
                    }

                    var input = rawInput.Trim().ToUpper();

                    if (!string.IsNullOrEmpty(input) && input.Length == 1)
                    {
                        int answerIndex = input[0] - 'A';
                        if (answerIndex >= 0 && answerIndex < question.Answers.Count)
                        {
                            var selectedAnswer = question.Answers[answerIndex];
                            isCorrect = selectedAnswer.IsRightAnswer;
                            answerText = selectedAnswer.AnswerText;
                            answered = true;

                            if (isCorrect) score += question.Mark;
                            userAnswers.Add(new UserAnswer(question.Id, answerText, isCorrect));
                            break;
                        }
                    }
                    Print.OutLine("Invalid input. Please select a valid option (A, B, C...).", ConsoleColor.Red);
                }
            }

            if (exitExam || _escPressed || _timeExpired)
            {
                break;
            }

            currentQuestionIndex++;
            Console.WriteLine();
        }

        // Stop timer thread
        _stopTimer = true;

        // If time expired, save exam immediately
        if (_timeExpired)
        {
            Console.Clear();
            Print.OutLine("⏰ TIME EXPIRED!", ConsoleColor.Red);
            Console.WriteLine(new string('═', 60));
            Console.WriteLine();
            Print.OutLine("Your exam has been automatically submitted.", ConsoleColor.Yellow);
            Thread.Sleep(2000);
        }

        // Record exam result
        var actualEndTime = DateTime.Now;
        var timeTaken = actualEndTime - startTime;
        
        var resultRecord = new ExamResultRecord(AppState.CurrentUser!.Id, exam.Id, score, exam.GetTotalMarks())
        {
            TimeTaken = timeTaken,
            UserAnswers = userAnswers
        };

        DataManager.ResultDB.Add(resultRecord);
        var user = AppState.CurrentUser;
        user.ExamHistory.Add(resultRecord.Id);
        DataManager.UserDB.Update(user.Id, user);

        // Show results
        Console.Clear();
        Print.OutLine("✅ EXAM COMPLETED!", ConsoleColor.Green);
        Console.WriteLine(new string('═', 60));
        Console.WriteLine();
        Console.WriteLine($"Subject: {subject.Name}");
        Console.WriteLine($"Score: {score}/{exam.GetTotalMarks()}");
        Console.WriteLine($"Percentage: {resultRecord.Percentage:F2}%");
        Console.WriteLine($"Grade: {resultRecord.Grade}");
        Console.WriteLine($"Status: {(resultRecord.Passed ? "PASSED ✓" : "FAILED ✗")}");
        Console.WriteLine($"Time Taken: {timeTaken.Minutes}m {timeTaken.Seconds}s");
        Console.WriteLine();

        // Show answers review
        Console.WriteLine();
        Print.OutLine("📋 ANSWERS REVIEW:", ConsoleColor.Cyan);
        Console.WriteLine(new string('═', 60));
        Console.WriteLine();

        for (int i = 0; i < exam.Questions.Count; i++)
        {
            var question = exam.Questions[i];
            var userAnswer = userAnswers.FirstOrDefault(ua => ua.QuestionId == question.Id);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Question {i + 1}: {question.Header}");
            Console.ResetColor();

            if (userAnswer == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("   Your answer: [NOT ANSWERED] ❌ (0 marks)");
            }
            else
            {
                Console.ForegroundColor = userAnswer.IsCorrect ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"   Your answer: {userAnswer.AnswerText} {(userAnswer.IsCorrect ? "✓" : "✗")}");
            }
            Console.ResetColor();

            // Show correct answer
            if (question.IsTrueFalse)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"   Correct answer: {question.Answer}");
            }
            else if (question.Answers != null)
            {
                var correctAnswer = question.Answers.FirstOrDefault(a => a.IsRightAnswer);
                if (correctAnswer != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"   Correct answer: {correctAnswer.AnswerText}");
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        Print.Pause();
    }

    private void MonitorTimer(DateTime endTime)
    {
        while (!_stopTimer && !_timeExpired && !_escPressed)
        {
            if (DateTime.Now >= endTime)
            {
                _timeExpired = true;
                break;
            }
            Thread.Sleep(1000);
        }
    }

    private bool ConfirmExitExam()
    {
        Console.WriteLine();
        return Print.AskYesNo("Are you sure you want to exit the exam?", false);
    }
}
