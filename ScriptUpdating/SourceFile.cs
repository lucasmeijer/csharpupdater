using System.IO;

namespace ScriptUpdating
{
	public class SourceFile
	{
		public string FileName;
		public string Contents;

		public static SourceFile For(string file)
		{
			return new SourceFile() {FileName = file, Contents = File.ReadAllText(file)};
		}
	}
}
