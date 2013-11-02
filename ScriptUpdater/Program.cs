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
			//var dir = "C:\\Users\\Public\\Documents\\Unity Projects\\4-0_AngryBots\\Assets";
			//var dir = "C:\\Users\\box1\\Documents\\stealth\\Assets";

			var dir = args[0];
			dir.Replace("/", "\\");
			Console.WriteLine("About to upgrade scripts in : "+dir);

			UpdateLanguage(dir, ".js", new JavascriptUpdater.JavascriptUpdater());
			UpdateLanguage(dir, ".cs", new CSharpUpdater.CSharpUpdater());
		}

		private static void UpdateLanguage(string dir, string extension, IScriptUpdater updater)
		{
			var allfiles = AllFilesIn(dir);
			var files = allfiles.Where(f => Path.GetExtension(f).ToLower() == extension);
			var output = updater.UpdateSmall(files.Select(SourceFile.For).ToArray());
			foreach (var file in output)
				file.Write();
		}


		private static IEnumerable<string> AllFilesIn(string dir)
		{
			return Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
		}

	}
}
