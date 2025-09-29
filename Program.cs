using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    static List<TaskItem> tasks = new();
    static string filePath = "tasks.json";

    static void Main()
    {
        LoadTasks();

        while (true)
        {
            ShowMenu();
            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    ListTasks();
                    break;
                case "3":
                    MarkDone();
                    break;
                case "4":
                    SaveTasks();
                    Console.WriteLine("Goodbye 👋");
                    return;
                default:
                    Console.WriteLine("Invalid option, try again!");
                    break;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("\n--- To-Do List ---");
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. List Tasks");
        Console.WriteLine("3. Mark Task as Done");
        Console.WriteLine("4. Exit");
        Console.Write("Choose option: ");
    }

    static void AddTask()
    {
        Console.Write("Enter task: ");
        string? task = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(task))
        {
            Console.WriteLine("⚠️ Task cannot be empty!");
            return;
        }

        tasks.Add(new TaskItem { Description = task, Done = false });
        SaveTasks();
        Console.WriteLine("✅ Task added!");
    }

    static void ListTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks yet!");
            return;
        }

        for (int i = 0; i < tasks.Count; i++)
        {
            var t = tasks[i];
            string status = t.Done ? "✔️" : "❌";
            Console.WriteLine($"{i + 1}. {t.Description} [{status}]");
        }
    }

    static void MarkDone()
    {
        ListTasks();
        Console.Write("Enter task number to mark done: ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int choice) && choice >= 1 && choice <= tasks.Count)
        {
            tasks[choice - 1].Done = true;
            SaveTasks();
            Console.WriteLine("🎉 Task marked as done!");
        }
        else
        {
            Console.WriteLine("Invalid choice!");
        }
    }

    static void SaveTasks()
    {
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    static void LoadTasks()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
    }
}

class TaskItem
{
    public string Description { get; set; } = string.Empty;
    public bool Done { get; set; }
}
