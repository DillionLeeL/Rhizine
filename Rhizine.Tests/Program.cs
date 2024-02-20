using Rhizine.Tests.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Xunit.Abstractions;


namespace Cmptr.Tests;
#if DEBUG
public class Program
{

    static public void Main(string[] args)
    {
        AnsiConsole.Profile.Capabilities.Ansi = true;
        AnsiConsole.Profile.Capabilities.Legacy = false;
        AnsiConsole.Profile.Width = 120;
        AnsiConsole.Profile.Encoding = System.Text.Encoding.UTF8;

        AnsiConsole.Write(new FigletText("Test Runner").Centered().Color(Color.Cyan1));
        AnsiConsole.Write(new Rule("[green]Dynamic Test Script Runner[/]").RuleStyle("grey").Centered());

        var testScripts = LoadTestScripts();
        List<TestScriptResult> results = new();

        // Command line argument to run all test cases
        if (args.Length > 0 && args[0] == "--run-all")
        {
            results = ExecuteAllTestScripts(testScripts.Values).ToList();

            OutputResults(results);
        }

        testScripts.Add("Run all test cases", null);

        do
        {
            var selectedTestScriptName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a test case to run:")
                    .PageSize(10)
                    .AddChoices(testScripts.Keys));

            if (selectedTestScriptName == "Run all test cases")
            {
                results = ExecuteAllTestScripts(testScripts.Values.Where(ts => ts != null)).ToList();

                OutputResults(results);
            }
            else
            {
                var testScript = testScripts[selectedTestScriptName];
                var result = AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .Start($"[yellow]Executing: {testScript.Name}...[/]", ctx => testScript.Execute());
                results.Add(result);

                AnsiConsole.MarkupLine($"[green]Execution Completed for: {testScript.Name}[/]");
                AnsiConsole.WriteLine();


                OutputResults(result);
            }
        }
        while (AnsiConsole.Confirm("Run another test case?", false));
        DisplayResultsInTable(results);

        RenderJsonResults(results);
    }
    static void DisplayResultsInTable(List<TestScriptResult> results)
    {
        var table = new Table().Border(TableBorder.Rounded).BorderColor(Color.LightSlateGrey)
            .AddColumn(new TableColumn("[u]Test Case[/]").Centered())
            .AddColumn("Status")
            .AddColumn("Duration")
            .AddColumn("Remarks");

        foreach (var result in results)
        {
            table.AddRow(result.Name, result.Status, $"{result.Duration} ms", result.Remarks);
        }

        AnsiConsole.Write(table);
    }

    static void RenderJsonResults(List<TestScriptResult> results)
    {
        // Assuming ConvertResultsToJson is a method that converts results to a JSON string
        var json = ConvertResultsToJson(results);
        AnsiConsole.Write(new Panel(json).Header("[blue]JSON Results[/]"));
    }
    public static void OutputResults(TestScriptResult result)
    {
        var table = new Table();
        table.AddColumn("Test Case");
        table.AddColumn("Status");
        table.AddColumn("Duration");
        table.AddColumn("Remarks");

        table.AddRow(result.Name, result.Status, $"{result.Duration} ms", result.Remarks);

        AnsiConsole.Write(table);
    }
    public static void OutputResults(IEnumerable<TestScriptResult> results)
    {
        var table = new Table();
        table.AddColumn("Test Case");
        table.AddColumn("Status");
        table.AddColumn("Duration");
        table.AddColumn("Remarks");

        foreach (var result in results)
        {
            table.AddRow(result.Name, result.Status, $"{result.Duration} ms", result.Remarks);
        }

        AnsiConsole.Write(table);
    }
    public static string ConvertResultsToJson(IEnumerable<TestScriptResult> results)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        return JsonSerializer.Serialize(results, options);
    }
    public static Dictionary<string, ITestScript> LoadTestScripts()
    {
        var testScriptType = typeof(ITestScript);
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(p => testScriptType.IsAssignableFrom(p) && !p.IsInterface);
        var testScripts = new Dictionary<string, ITestScript>();

        foreach (var type in types)
        {
            var testScript = (ITestScript)Activator.CreateInstance(type);
            testScripts.Add(testScript.Name, testScript);
        }

        return testScripts;
    }

    static void ExecuteTestScript(ITestScript testScript)
    {
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .Start($"[yellow]Executing: {testScript.Name}...", ctx =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                try
                {
                    testScript.Execute();
                    watch.Stop();
                    AnsiConsole.MarkupLine($"[green]Success:[/] {testScript.Name} completed in {watch.ElapsedMilliseconds} ms.");
                }
                catch (Exception ex)
                {
                    watch.Stop();
                    AnsiConsole.MarkupLine($"[red]Failure:[/] {testScript.Name} failed after {watch.ElapsedMilliseconds} ms with error: {ex.Message}");
                }
            });
    }
    static IEnumerable<TestScriptResult> ExecuteAllTestScripts(IEnumerable<ITestScript> testScripts)
    {
        foreach (var testScript in testScripts)
        {
            yield return testScript.Execute();
        }
    }
    
}

#endif
/*
public void ActionTest()
{
    public void SetupHighPriorityEventHandling()
    {
        notificationService.Subscribe("CriticalPopupDetected", context =>
        {
            var handleCriticalPopupAction = new ActionItem(
                ctx => { // todo 
                },
                priority: 0, // Highest priority
                description: "Handle Critical Popup");

            // Directly handle the high-priority event, bypassing normal queue processing
            new HighPriorityEventHandler(actionExecutor, highPriorityActionQueue).HandleHighPriorityEvent(handleCriticalPopupAction);
        });
    }
    var executor = new ActionExecutor();
    var highPriorityAction = new ActionItem(ctx => { //todo
    }, 1, "High Priority");
    var regularAction = new ActionItem(ctx => { //todo
    }, 2, "Regular Action");

    // Execute a regular action
    await executor.ExecuteAction(regularAction);

    // If a high-priority action occurs and interrupts
    executor.InterruptCurrentAction();
    await executor.ExecuteAction(highPriorityAction);

    // Later, attempt to resume the interrupted regular action
    executor.ResumeInterruptedAction();
    var actionExecutor = new ActionExecutor();
    var highPriorityQueue = new HighPriorityActionQueue(actionExecutor);

    // Example of enqueuing a high-priority action
    var closePopupAction = new ActionItem(ctx => { Console.WriteLine("Closing popup"); }, priority: 0, description: "Close Popup");
    highPriorityQueue.EnqueueHighPriorityAction(closePopupAction);


    ActionExecutor actionExecutor = new ActionExecutor();

    actionExecutor.OnActionCompleted += (actionItem) =>
    {
        Console.WriteLine($"Action '{actionItem.Description}' completed successfully.");
    };

    actionExecutor.OnActionFailed += (actionItem, ex) =>
    {
        Console.WriteLine($"Action '{actionItem.Description}' failed with error: {ex.Message}");
    };
}
*/
