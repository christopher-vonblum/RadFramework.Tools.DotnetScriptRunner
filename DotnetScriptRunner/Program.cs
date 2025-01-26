using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

string scriptFile;

if (args.Length == 0 && !Debugger.IsAttached)
{
    Console.Error.WriteLine("No script file supplied.");
    return -1;
}

if (Debugger.IsAttached)
{
    scriptFile = "TestScript.csx";
}
else
{
    scriptFile = args[0];
}

string appWorkingDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

string scriptName = Path.GetFileName(scriptFile);
string scriptRootPath = Path.GetPathRoot(scriptFile);

CreateDirectory( appWorkingDir + "/compilations");

CreateDirectory(appWorkingDir + $"/compilations/{scriptName}");

byte[] script = File.ReadAllBytes(scriptFile);

string scriptVersionHash = Convert.ToHexString(SHA1.HashData(script));

string workingDir = appWorkingDir + $"/compilations/{scriptName}/{scriptVersionHash}";

if (!Directory.Exists(workingDir))
{
    if (!BuildScriptAssembly())
    {
        return -1;
    }
}

if (!File.Exists(workingDir + $"/bin/Debug/net9.0/{scriptName}.dll"))
{
    if (!BuildScriptAssembly())
    {
        return -1;
    }
}

/*
Assembly scriptAssembly = Assembly.LoadFile(workingDir + $"/bin/Debug/net9.0/{scriptName}.dll");

var mainMethod = scriptAssembly.EntryPoint;

object result;

try
{
    result = mainMethod.Invoke(null, new object[] { args.Skip(1).ToArray()} );
}
catch (Exception e)
{
    Console.WriteLine("Exception occured in script:");
    Console.Write(e.ToString());
    return -1;
}

if (mainMethod.ReturnType == typeof(int))
{
    return (int)result;
}

*/

var programRunning = RunShellCommand(
    Path.GetPathRoot(scriptFile),
    "dotnet",
    workingDir + $"/bin/Debug/net9.0/{scriptName}.dll "
    + string.Join(' ', args.Skip(1)));

Console.Write(programRunning.StandardOutput.ReadToEnd());
Console.Write(programRunning.StandardError.ReadToEnd());

return programRunning.ExitCode;

bool BuildScriptAssembly()
{
    CreateDirectory(workingDir);

    FileCopyOver(appWorkingDir + "/res/script-project.xml", workingDir + $"/{scriptName}.csproj");

    string[] scriptLines = File.ReadAllLines(scriptFile);

    File.WriteAllLines(workingDir + "/Program.cs", scriptLines.Skip(1));

    var buildProcess = RunShellCommand(workingDir, "dotnet", $"build {scriptName}.csproj");

    if (buildProcess.ExitCode != 0)
    {
        Console.Write(buildProcess.StandardOutput.ReadToEnd());
        Console.Write(buildProcess.StandardError.ReadToEnd());
        return false;
    }

    return true;
}

void FileCopyOver(string path, string target)
{
    if (File.Exists(target))
    {
        File.Delete(target);
    }
    
    File.Copy(path, target);
}
void CreateDirectory(string path)
{
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
}

Process RunShellCommand(string workingDir, string command, string arguments)
{
    Process process = new Process();
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.WorkingDirectory = workingDir;
    process.StartInfo.FileName = command;
    process.StartInfo.Arguments = arguments;

    process.StartInfo.RedirectStandardOutput = true;
    process.StartInfo.RedirectStandardError = true;
    
    process.Start();
    process.WaitForExit();

    return process;
}