using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScriptUpdating;

namespace ScriptUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			var dir = "C:\\Users\\Public\\Documents\\Unity Projects\\4-0_AngryBots\\Assets";
			//var dir = "C:\\Users\\box1\\Documents\\stealth\\Assets";

			UpdateLanguage(dir, ".js", new JavascriptUpdater.JavascriptUpdater());
			UpdateLanguage(dir, ".cs", new CSharpUpdater.CSharpUpdater());
		}

		private static void UpdateLanguage(string dir, string extension, IScriptUpdater updater)
		{
			var allfiles = AllFilesIn(dir);
			var files = allfiles.Where(f => Path.GetExtension(f).ToLower() == extension);
			var output = updater.UpdateSmall(files.Select(SourceFile.For).ToArray());
			foreach (var file in output)
			{
				if (file.FileName.Contains("PatrolPoint.js"))
				{
					Console.WriteLine("Writing:");
					Console.WriteLine(file.Contents);
				}
				File.WriteAllText(file.FileName, file.Contents);
			}
		}

		private static IEnumerable<string> AllFilesIn(string dir)
		{
			return Directory.EnumerateFileSystemEntries(dir, "*.*", SearchOption.AllDirectories);
		}

	}
}
