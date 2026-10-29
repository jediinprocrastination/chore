using Spectre.Console;
using System;
using System.Collections.Generic;

namespace Chore.Sandbox;

internal class Program
{
    private readonly record struct CommitType(string Code, string Description);

    public static void Main(string[] args)
    {
        var terminal = GetInteractionOutput();
        var commitTypes = GetCommitTypes();

        var commitType = terminal.Prompt(
            new SelectionPrompt<CommitType>()
                .Title("Select a [green]commit type[/]:")
                .PageSize(5)
                .EnableSearch()
                .UseConverter(t => $"{t.Code,-10}[{Color.Gray}]{t.Description}[/]")
                .AddChoices(commitTypes)
                .WrapAround()
        );

        var commitMessage = terminal.Ask<string>("Provide a commit message:");

        var result = $"{commitType.Code}: {commitMessage}";

        Console.Out.WriteLine(result);
    }

    private static IAnsiConsole GetInteractionOutput()
    {
        return AnsiConsole.Create(
            new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Detect,
                ColorSystem = ColorSystemSupport.Detect,
                Interactive = InteractionSupport.Yes,
                Out = new AnsiConsoleOutput(Console.Error),
            }
        );
    }

    private static IReadOnlyCollection<CommitType> GetCommitTypes()
    {
        return
        [
            new CommitType("feat", "A new feature for the user or API"),
            new CommitType("fix", "A bug fix"),
            new CommitType("docs", "Documentation-only changes (README, comments, etc.)"),
            new CommitType(
                "style",
                "Code style changes (formatting, missing semicolons, etc.) — no logic"
            ),
            new CommitType("refactor", "Code changes that neither fix a bug nor add a feature"),
            new CommitType("perf", "Performance improvements"),
            new CommitType("test", "Adding or updating tests"),
            new CommitType(
                "build",
                "Changes that affect the build system or external dependencies"
            ),
            new CommitType(
                "ci",
                "Changes to CI configuration files and scripts (GitHub Actions, Travis)"
            ),
            new CommitType(
                "chore",
                "Routine tasks, maintenance, or other changes that don’t modify src/test"
            ),
            new CommitType("revert", "Reverts a previous commit"),
            new CommitType(
                "config",
                "Changes to configuration files (e.g., .eslintrc, .prettierrc)"
            ),
            new CommitType(
                "env",
                "Environment-specific changes (e.g., Docker, deployment scripts)"
            ),
            new CommitType("security", "Patches or updates related to vulnerabilities"),
            new CommitType("deps", "Dependency updates"),
            new CommitType("init", "Initial commit or project scaffolding"),
        ];
    }
}
