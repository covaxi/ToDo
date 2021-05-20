using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Todoist.Net.Models;

namespace ToDo
{
  static class Constants
  {
    public const string Todoist = "Todoist";
    public const string TodoistAPIKey = "TodoistAPIKey";
    public const string DefaultProjectName = "DefaultProjectName";
    public const string Inbox = "Inbox";
  }
  class Program
  {
    public static string GetValue(string name, string defaultValue = "")
    {
      var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Todoist");
      if (!Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }

      var fileName = Path.Combine(directory, $"{name}.txt");
      if (!File.Exists(fileName))
      {
        Console.WriteLine($"Enter {name}" + (string.IsNullOrWhiteSpace(defaultValue) ? ": " : $" [{defaultValue}]: "));
        var value = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(defaultValue))
        {
          value = defaultValue;
        }
        using (var fs = File.CreateText(fileName))
        {
          fs.WriteLine(value);
        }
        return value;
      }
      using (var fs = File.OpenText(fileName))
      {
        return fs.ReadLine();
      }
    }

    static async System.Threading.Tasks.Task Main(string[] args)
    {
      var token = GetValue(Constants.TodoistAPIKey);
      var projectName = GetValue(Constants.DefaultProjectName, Constants.Inbox);
      var user = new Todoist.Net.TodoistClient(token);
      var projects = await user.Projects.GetAsync();
      var project = projects.FirstOrDefault(p => p.Name == projectName);
      if (project == null)
      {
        project = projects.First(p => p.Name == Constants.Inbox);
      }
      var content = string.Join(" ", args);
      var item = new Item(content);
      Console.WriteLine($"Adding '{content}'");
      await user.Items.AddAsync(item);
      Console.WriteLine("Done");
    }
  }
}
