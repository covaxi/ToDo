using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToDo
{
  public class Config
  {
    public static Dictionary<string, string> Values { get; set; }
    public static async ValueTask Write(string filename)
    {
      Directory.CreateDirectory(Path.GetDirectoryName(filename));
      using(var fs = File.CreateText(filename))
      {
        foreach(var line in Values)
        {
          await fs.WriteLineAsync($"{line.Key}={line.Value}");
        }
      }
    }

    public static async ValueTask<Dictionary<string, string>> Read(string filename)
    {
      var result = new Dictionary<string, string>();
      if (File.Exists(filename))
      {
        using (var fs = File.OpenText(filename))
        {
          while (fs.Peek() >= 0)
          {
            var line = await fs.ReadLineAsync();
            var match = Regex.Match(line, "([^=]+)=(.+)");
            if (match.Success)
            {
              result[match.Groups[1].Value] = match.Groups[2].Value;
            }
          }

        }
      }
      Values = result;
      return result;
    }
  }
}
