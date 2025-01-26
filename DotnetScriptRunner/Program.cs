using System.Diagnostics;
using System.Security.Cryptography;

string scriptFile = args[0];

string scriptName = Path.GetFileName(scriptFile);

CreateDirectory("compilations");

CreateDirectory($"compilations/{scriptName}");

byte[] script = File.ReadAllBytes(scriptFile);

string scriptVersionHash = Convert.ToHexString(SHA1.HashData(script));

string workingDir = $"compilations/{scriptName}/{scriptVersionHash}";

CreateDirectory(workingDir);

RunShellCommand(workingDir, "dotnet", $"new console");

File.WriteAllBytes(workingDir + "/Program.cs", script);

RunShellCommand(workingDir, "dotnet", $"run");

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
    process.StartInfo.WorkingDirectory = workingDir;
    process.StartInfo.FileName = command;
    process.StartInfo.Arguments = command;

    process.Start();
    process.WaitForExit();
}