using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

string scriptFile;

if (args.Length == 0 && !Debugger.IsAttached)
{
    Console.WriteLine("No script file supplied.");
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

if (!File.Exists(workingDir + "/bin/Debug/net9.0/script.dll"))
{
    if (!BuildScriptAssembly())
    {
        return -1;
    }
}

Assembly scriptAssembly = Assembly.LoadFile(workingDir + "/bin/Debug/net9.0/script.dll");

var mainMethod = scriptAssembly.EntryPoint;

object result = mainMethod.Invoke(null, new object[] { args.Skip(1).ToArray()} );

if (mainMethod.ReturnType == typeof(int))
{
    return (int)result;
}

return 0;


bool BuildScriptAssembly()
{
    CreateDirectory(workingDir);

    FileCopyOver( appWorkingDir + "/res/script-project.xml", workingDir + "/script.csproj");
    File.WriteAllBytes(workingDir + "/Program.cs", script);

    var buildProcess = RunShellCommand(workingDir, "dotnet", "build script.csproj", true);

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

Process RunShellCommand(string workingDir, string command, string arguments, bool writeOutput)
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