using System;
using System.IO;
using System.Linq;
using Todoist.Net.Models;

namespace ToDo
{
  class Program
  {
    public static string ConfigFile => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.Todoist, "settings.ini");

    static async System.Threading.Tasks.Task Main(string[] args)
    {
      try
      {
        await Config.Read(ConfigFile);
        var token = Config.GetValue(Constants.APIKey);
        var projectName = Config.GetValue(Constants.DefaultProjectName, Constants.Inbox);
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
      finally
      {
        await Config.Write(ConfigFile);
      }
    }
  }
}
