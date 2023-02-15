using System;
using System.Diagnostics;
using System.Security.Principal;

namespace SandboxTool
{
	class Program
	{
		static void Main(string[] args)
		{
			// Check if the user has administrative privileges
			WindowsIdentity identity = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(identity);
			if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
			{
				Console.WriteLine("Sandbox tool must be run as administrator!");
				return;
			}

			// Prompt the user to select an executable program
			Console.WriteLine("Please enter the path of the program you want to run in the sandbox:");
			string executablePath = Console.ReadLine();

			// Prompt the user to select the relevant permissions
			Console.WriteLine("Please select the relevant permissions (Enter 1 or 2):");
			Console.WriteLine("1. No network access and read-only access to the file system");
			Console.WriteLine("2. Full access to the network and the file system");
			string permission = Console.ReadLine();

			// Create the sandbox process with the selected permissions
			ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
			if (permission == "1")
			{
				startInfo.Arguments = "/c start ms-settings:sandbox";
			}
			else if (permission == "2")
			{
				startInfo.Arguments = $"/c start ms-settings:sandbox /EnableNetworkAccess /MappedFolder:\"{executablePath};{executablePath}\"";
			}
			else
			{
				Console.WriteLine("Invalid input!");
				return;
			}

			Process sandboxProcess = new Process();
			sandboxProcess.StartInfo = startInfo;
			sandboxProcess.Start();

			// Wait for the sandbox process to exit
			sandboxProcess.WaitForExit();
		}
	}
}

// TODO: DELETE THIS COMMENT AFTER READING IT.

// This code checks if the user running the tool has administrative privileges, prompts the user to select an executable program and relevant permissions, and then creates a sandbox process with the selected permissions using the built-in Windows 10 Sandbox feature.

// Note that this code assumes that the user has already enabled the Windows 10 Sandbox feature on their system. If it's not enabled, the tool will prompt the user to enable it manually. To make the tool more user-friendly, you can add a check for the Sandbox feature and prompt the user to enable it automatically if it's not already enabled.