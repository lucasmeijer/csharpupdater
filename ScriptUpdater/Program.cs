using System;
using System.Collections.Generic;
using System.IO;

namespace ScriptUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			var dir = "C:\\Users\\Public\\Documents\\Unity Projects\\4-0_AngryBots\\Assets";
			foreach (var file in AllFilesIn(dir))
			{
				var updater = UpdaterFor(file);
				if (updater == null)
					continue;
				Console.WriteLine("Processing: "+file);
				try
				{
					var output = updater.UpdateSmall(File.ReadAllText(file));
					File.WriteAllText(file, output);
				}
				catch (Exception e)
				{
					Console.WriteLine("Failed to update: "+file+" exception: "+e);
				}
			}
		}

		private static IEnumerable<string> AllFilesIn(string dir)
		{
			return Directory.EnumerateFileSystemEntries(dir, "*.*", SearchOption.AllDirectories);
		}



		private static IScriptUpdater UpdaterFor(string file)
		{
			var extension = Path.GetExtension(file);
			if (extension==".cs")
				return new CSharpUpdater();
			if (extension==".js")
				return new JavascriptUpdater();
			return null;
		}
	}
}
