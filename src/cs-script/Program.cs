using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

string scriptFile;

if (args.Length == 0 && !Debugger.IsAttached)
{
    Console.Error.WriteLine("No script file supplied.");
    return -1;
}

string appWorkingDir; 

#if DEBUG

scriptFile = "TestScript.csx";
appWorkingDir = "/home/anon/Documents/workspace/cs-script/src/cs-script/bin/Debug/net9.0";

#else

scriptFile = args[0];
appWorkingDir = File.ReadAllText("/usr/bin/cs-script.cfg");

#endif

string scriptName = Path.GetFileName(scriptFile);
string dashedScriptNamePath = scriptName + "-" + scriptFile.Replace('/', '-');

CreateDirectory( appWorkingDir + "/cs-script-data/compilations");

CreateDirectory(appWorkingDir + $"/cs-script-data/compilations/{dashedScriptNamePath}");

byte[] script = File.ReadAllBytes(scriptFile);

string scriptVersionHash = Convert.ToHexString(SHA1.HashData(script));

string workingDir = appWorkingDir + $"/cs-script-data/compilations/{dashedScriptNamePath}/{scriptVersionHash}";

string scriptAssemblyPath = workingDir + $"/{scriptName}.dll";

if (!Directory.Exists(workingDir))
{
    if (!BuildScriptAssembly())
    {
        return -1;
    }
}

if (!File.Exists(scriptAssemblyPath))
{
    if (!BuildScriptAssembly())
    {
        return -1;
    }
}

var programRunning = RunShellCommand(
    Path.GetPathRoot(scriptFile),
    "dotnet",
    scriptAssemblyPath
    + string.Join(' ', args.Skip(1)),
    false);

return programRunning.ExitCode;

bool BuildScriptAssembly()
{
    CreateDirectory(workingDir);

    string projectDir = workingDir + "/project";
    
    CreateDirectory(projectDir);
    
    FileCopyOver(appWorkingDir + "/cs-script-data/script-project.xml", projectDir + $"/{scriptName}.csproj");

    string[] scriptLines = File.ReadAllLines(scriptFile);

    File.WriteAllLines(projectDir + "/Program.cs", scriptLines.Skip(1));

    var buildProcess = RunShellCommand(projectDir, "dotnet", $"build {scriptName}.csproj", true);

    if (buildProcess.ExitCode != 0)
    {
        Console.Write(buildProcess.StandardOutput.ReadToEnd());
        Console.Write(buildProcess.StandardError.ReadToEnd());
        return false;
    }
    
    foreach (string outputFile in Directory.GetFiles(projectDir + $"/bin/Debug/net9.0/"))
    {
        string destFile = workingDir + "/" + Path.GetFileName(outputFile);
        File.Copy(outputFile, destFile);
        RunShellCommand(workingDir, "chmod", "a+rwx " + destFile, false);
    }
    
    Directory.Delete(projectDir, true);
    
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
        RunShellCommand(path, "chmod", "-R a+rwx " + path, false);
    }
}

Process RunShellCommand(string workingDir, string command, string arguments, bool redirectOutput)
{
    Process process = new Process();
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.WorkingDirectory = workingDir;
    process.StartInfo.FileName = command;
    process.StartInfo.Arguments = arguments;

    if (redirectOutput)
    {
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
    }
    
    process.Start();
    process.WaitForExit();

    return process;
}