using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

string scriptFile;

if (Debugger.IsAttached)
{
    scriptFile = "TestScript.csx";
}
else
{
    scriptFile = args[0];
}

string scriptName = Path.GetFileName(scriptFile);
string scriptRootPath = Path.GetPathRoot(scriptFile);

CreateDirectory("compilations");

CreateDirectory($"compilations/{scriptName}");

byte[] script = File.ReadAllBytes(scriptFile);

string scriptVersionHash = Convert.ToHexString(SHA1.HashData(script));

string workingDir = System.IO.Path.GetDirectoryName(
    System.Reflection.Assembly.GetExecutingAssembly().Location) + $"/compilations/{scriptName}/{scriptVersionHash}";

CreateDirectory(workingDir);

File.Copy("res/script-project.xml", workingDir + "/script.csproj");
File.WriteAllBytes(workingDir + "/Program.cs", script);

RunShellCommand(workingDir, "dotnet", "build script.csproj");

RunShellCommand(workingDir, "dotnet", "run bin/Debug/net9.0/script.dll " + string.Join(' ', args.Skip(1)));

void CreateDirectory(string path)
{
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
}

void RunShellCommand(string workingDir, string command, string arguments)
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

    Console.WriteLine(process.StandardOutput.ReadToEnd());
    ;
}