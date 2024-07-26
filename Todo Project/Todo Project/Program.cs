using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Todo_Project
{
    internal class Program
    {
        
        static readonly Random Random = new Random();
        static readonly List<string> TodoList = new List<string>();
        private const string FilePath = @"TodoFile.txt";

        private static void Main()
        {
            Console.Title = "Todo App";
            Load();
            MainMenu();
            Console.ReadKey();
        }

        private static void MainMenu()
        {
            Console.Clear();
            string[] menuActions = { "Check your list", "Quit App" };
            Console.WriteLine("\u001b[34;1m<-----------------[\u001b[0m Main Menu \u001b[34;1m]----------------->\u001b[0m\n");
            Console.WriteLine
            (
            "Hello and welcome to my Todo Console app.\nIn this app you can add your Todo things and check them later on.\nYou are able to save, edit and remove things or even mark them as completed.."
            );
            if (File.Exists(FilePath))
            {
                var fileInfo = new FileInfo(FilePath);
                Console.WriteLine($"\u001b[4mLast Edit: {fileInfo.LastWriteTime:yyyy/MM/dd - HH:mm}\u001b[0m");
            }
            Console.WriteLine("\n\n<---------[Actions]--------->");
            for (var i = 0; i < menuActions.Length; i++)
            {
                Console.WriteLine($"\u001b[35;1m{i + 1}-\u001b[0m {menuActions[i]}");
            }
            switch (AskForInput())
            {
                case 1: ListWidget(); break;
                case 2: QuitApp(); break;
                default: WrongInput(); MainMenu(); break;
            }
        }

        private static int AskForInput()
        {
            Console.Write("\n");
            Console.Write("\u001b[35;1m-->\u001b[0m \u001b[1mEnter your choice: \u001b[0m");
            var success = int.TryParse(Console.ReadLine(), out var result);
            return success ? result : 0;
        }

        private static void ListWidget()
        {
            Console.Clear();
            if ( TodoList.Count > 0 )
            {
                Console.WriteLine("\u001b[33m<-----------------[\u001b[0m Todo List \u001b[33m]----------------->\u001b[0m\n");
                var id = 1;
                foreach (var todo in TodoList)
                {
                    Console.WriteLine($"\u001b[36;1m{id}.\u001b[0m {todo}");
                    id++;
                }
                string[] actions = { "Add new", "Update one", "Remove one", "Remove all", "Give importance to an element", "Mark as completed", "Main Menu" };
                Console.WriteLine("\n\n<---------[Actions]--------->");
                for (var i = 0; i < actions.Length; i++)
                {
                    Console.WriteLine($"\u001b[35;1m{i + 1}-\u001b[0m {actions[i]}");
                }
                switch (AskForInput())
                {
                    case 1: AddNew(); break;
                    case 2: UpdateOne(); break;
                    case 3: RemoveOne(); break;
                    case 4: RemoveAll(); break;
                    case 5: MarkAsImportant(); break;
                    case 6: MarkAsCompleted(); break;
                    case 7: MainMenu(); break;
                    default: WrongInput(); ListWidget(); break;
                }
            }
            else
            {
                Console.WriteLine("\u001b[33m<-----------------[\u001b[0m List Menu \u001b[33m]----------------->\u001b[0m");
                Console.WriteLine("\nThere is no list to show, Go ahead and add some..");
                Console.WriteLine("\n\n<---------[Actions]--------->");
                Console.WriteLine("\u001b[35;1m1-\u001b[0m Add New One");
                Console.WriteLine("\u001b[35;1m2-\u001b[0m Main Menu");
                switch (AskForInput())
                {
                    case 1: AddNew(); break;
                    case 2: MainMenu(); break;
                    default: WrongInput(); ListWidget(); break;
                }
            }
        }

        private static void AddNew()
        {
            Console.Clear();
            Console.WriteLine("\u001b[34;1m<-----------------[\u001b[0m Adding New Todo \u001b[34;1m]----------------->\u001b[0m\n");
            Console.Write("\u001b[33;1m-->\u001b[0m What do you want to add: ");
            var todo = Console.ReadLine();
            if ( todo != string.Empty)
            {
                Console.WriteLine("\n\n<---------[Actions]--------->");
                Console.WriteLine("\u001b[35;1m1-\u001b[0m Save and add new");
                Console.WriteLine("\u001b[35;1m2-\u001b[0m Save and return");
                Console.WriteLine("\u001b[35;1m3-\u001b[0m Cancel");
                switch (AskForInput())
                {
                    case 1: 
                        FileManager.AddLineToFile(FilePath, todo);
                        TodoList.Add(todo);
                        AddNew();
                        break;
                    case 2: 
                        FileManager.AddLineToFile(FilePath, todo);
                        TodoList.Add(todo);
                        ListWidget();
                        break;
                    case 3: ListWidget(); break;
                    default: WrongInput(); ListWidget(); break;
                }
                
            }
            else
            {
                Console.Write("\u001b[31;1mYou cant add an empty field..\u001b[0m");
                Task.Delay(3000).Wait();
                ListWidget();
            }

        }

        private static void UpdateOne()
        {
            Console.Write("\u001b[33;1m-->\u001b[0m Enter note id you wish to update: ");
            var success = int.TryParse(Console.ReadLine(), out var result);
            if (success)
            {
                if (result <= 0 || result > TodoList.Count)
                {
                    WrongIndex();
                    ListWidget();
                }
                else
                {
                    Console.Write("\u001b[33;1m-->\u001b[0m Update your note: ");
                    var todo = Console.ReadLine();
                    if (todo != string.Empty)
                    {
                        TodoList[result - 1] = todo;
                        FileManager.UpdateLineAtIndex(FilePath, result - 1, todo);
                        ListWidget();
                    }
                    else
                    {
                        Console.WriteLine("\u001b[31;1mThe field is empty..\u001b[0m");
                        Task.Delay(1700).Wait();
                        ListWidget();
                    }
                }
            }
            else
            {
                WrongInput();
                ListWidget();
            }
        }

        private static void RemoveOne()
        {
            Console.Write("\u001b[33;1m-->\u001b[0m Enter note id you wish to remove: ");
            var success = int.TryParse(Console.ReadLine(), out var result);
            if (success)
            {
                if (result <= 0 || result > TodoList.Count)
                {
                    WrongIndex();
                    ListWidget();
                }
                else
                {
                    TodoList.RemoveAt(result - 1);
                    FileManager.RemoveLineAtIndex(FilePath, result - 1);
                    ListWidget();
                }
            }
            else
            {
                WrongInput();
                ListWidget();
            }
        }

        private static void RemoveAll()
        {
            if (Confirm("remove all content"))
            {
                TodoList.Clear();
                File.Delete(FilePath);
                ListWidget();
            }
            else
            {
                ListWidget();
            }
        }

        private static int GetDayLength(string note)
        {
            if (note.Contains("Friday")) return 29;
            if (note.Contains("Saturday")) return 31;
            if (note.Contains("Sunday")) return 29;
            if (note.Contains("Monday")) return 29;
            if (note.Contains("Tuesday")) return 30;
            if (note.Contains("Wednesday")) return 32;
            if (note.Contains("Thursday")) return 31;
            return 0;
        }

        private static void MarkAsImportant()
        {
            Console.Write("\u001b[33;1m-->\u001b[0m Enter node id you wish to mark as important: ");
            var success = int.TryParse(Console.ReadLine(), out var result);
            if (success)
            {
                if (result <= 0 || result > TodoList.Count)
                {
                    WrongIndex();
                    ListWidget();
                }
                else
                {
                    if (TodoList[result - 1].Contains("Completed"))
                    {
                        var str = TodoList[result - 1];
                        str = str.Remove(str.IndexOf("At") - 1, GetDayLength(str));
                        str = str.Replace("Completed", "Important");
                        str = str.Replace("[9m", "[4m");
                        str = str.Replace("32m", "31;1m");
                        
                        TodoList.RemoveAt(result - 1);
                        FileManager.RemoveLineAtIndex(FilePath, result - 1);
                        TodoList.Insert(0, str);
                        FileManager.AddLineAtIndex(FilePath, 0, str);
                        ListWidget();
                    }
                    else if (TodoList[result - 1].Contains("Important"))
                    {
                        ListWidget();
                    }
                    else
                    {
                        var mark = TodoList[result - 1] = $"\u001b[4m{TodoList[result - 1]}\u001b[0m      \u001b[31;1m[Important]\u001b[0m";
                        TodoList.RemoveAt(result - 1);
                        FileManager.RemoveLineAtIndex(FilePath, result - 1);
                        TodoList.Insert(0, mark);
                        FileManager.AddLineAtIndex(FilePath, 0, mark);
                        ListWidget();
                    }
                }
            }
            else
            {
                WrongInput();
                ListWidget();
            }
        }

        private static void MarkAsCompleted()
        {
            Console.Write("\u001b[33;1m-->\u001b[0m Enter node id you wish to mark as completed: ");
            var success = int.TryParse(Console.ReadLine(), out var result);
            if (success)
            {
                if (result <= 0 || result > TodoList.Count)
                {
                    WrongIndex();
                    ListWidget();
                }
                else
                {
                    if (TodoList[result - 1].Contains("Important"))
                    {
                        var str = TodoList[result - 1];
                        str = str.Replace("Important", $"Completed At {DateTime.Now.DayOfWeek} {DateTime.Now:yyyy/MM/dd - HH:mm}");
                        str = str.Replace("[4m", "[9m");
                        str = str.Replace("31;1m", "32m");
                        TodoList[result - 1] = str;
                        FileManager.UpdateLineAtIndex(FilePath, result - 1, str);
                        ListWidget();
                    }
                    else if (TodoList[result - 1].Contains("Completed"))
                    {
                        ListWidget();
                    }
                    else
                    {
                        var mark = TodoList[result - 1] = $"\u001b[9m{TodoList[result - 1]}\u001b[0m      \u001b[32m[Completed At {DateTime.Now.DayOfWeek} {DateTime.Now:yyyy/MM/dd - HH:mm}]\u001b[0m";
                        FileManager.UpdateLineAtIndex(FilePath, result - 1, mark);
                        ListWidget();
                    }
                }
            }
            else
            {
                WrongInput();
                ListWidget();
            }
        }

        private static void QuitApp()
        {
            if (Confirm("quit the app"))
            {
                Environment.Exit(0);
            }
            MainMenu();
        }

        private static void WrongInput(int delayTime = 1500)
        {
            string[] errorInput = { "Not valid input!!", "Wrong Input!!", "Please choice a valid action!!", "Bad input!!", "Try Again with valid action!!", "Invalid Input!!" };
            Console.Write($"--> \u001b[31;1m{errorInput[Random.Next(errorInput.Length)]}\u001b[0m");
            Task.Delay(delayTime).Wait();
        }

        private static void WrongIndex(int delayTime = 1800)
        {
            string[] errors = { "The given index is not valid", "Not valid index", "Try again with valid index", "There is no note for this index" };
            Console.Write($"--> \u001b[31;1m{errors[Random.Next(0, errors.Length)]}\u001b[0m");
            Task.Delay(delayTime).Wait();
        }

        private static bool Confirm(string mess)
        {
            Console.WriteLine($"\n\u001b[1mYou are about to {mess}, Are you sure?\u001b[0m");
            Console.Write("\u001b[41m[1. Confirm]\u001b[0m - \u001b[42m[2. Cancel]\u001b[0m : ");
            var answer = Console.ReadLine();
            return answer == "1";
        }

        private static void Load()
        {
            if (!File.Exists(FilePath)) return;
            var allContent = File.ReadAllLines(FilePath);
            foreach (var line in allContent)
            {
                TodoList.Add(line);
            }
        }
    }
}
