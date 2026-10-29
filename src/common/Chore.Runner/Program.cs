using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Chore.Runner;

internal class Program
{
    public static void Main(string[] args)
    {
        if (args is { Length: < 1 })
            return;

        var handlersDirectory = GetRunnerExecutableDirectory();

        var handlerName = $"{Const.HandlerExecutablePrefix}.{args.ElementAt(0)}.exe";
        var handlerPath = Path.Combine(handlersDirectory, handlerName);
        var handlerArguments = args.Skip(1);

        if (!File.Exists(handlerPath))
        {
            return;
        }

        var startInfo = new ProcessStartInfo(handlerPath, handlerArguments)
        {
            UseShellExecute = false,
        };

        using var process = Process.Start(startInfo);
        process?.WaitForExit();
    }

    private static string GetRunnerExecutableDirectory()
    {
        var thisAssembly = Assembly.GetExecutingAssembly();
        var thisAssemblyLocation = thisAssembly.Location;
        var currentDirectory = Path.GetDirectoryName(thisAssemblyLocation);

        if (currentDirectory is null)
            throw new Exception("Executing assembly location denotes a root directory");

        if (string.IsNullOrEmpty(currentDirectory))
            throw new Exception(
                "Executing assembly location does not contain directory information"
            );

        return currentDirectory;
    }

    private static class Const
    {
        public const string HandlerExecutablePrefix = "Chore";
    }
}
