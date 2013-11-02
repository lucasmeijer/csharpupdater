using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ScriptUpdating
{
	public class SourceFile
	{
		public string FileName;
		public string Contents;

		public static SourceFile For(string file)
		{
			using (var reader = new StreamReader(file, true))
			{
				var contents = reader.ReadToEnd();
				return new SourceFile() { FileName = file, Contents = contents };
			}
		}

		public void Write()
		{
			File.WriteAllText(FileName,Contents, new UTF8Encoding(HasBOM(FileName)));
		}

		public static bool HasBOM(string file)
		{
			var bytes = File.ReadAllBytes(file);
			var preamble = UTF8BOM();
			var match = bytes.Take(preamble.Length).SequenceEqual(preamble);
			return match;
		}

		private static byte[] UTF8BOM()
		{
			return new Byte[] { 0xEF, 0xBB, 0xBF };
		}
	}

	[TestFixture]
	public class SourceFileTests
	{
		[Test]
		public void RespectsFileWIthUTF8BOM()
		{
			var tmp = Path.GetTempFileName();
			File.WriteAllText(tmp,"Hello",new UTF8Encoding(true));
			Assert.IsTrue(SourceFile.HasBOM(tmp));
			var sourcefile = SourceFile.For(tmp);
			sourcefile.Write();
			Assert.IsTrue(SourceFile.HasBOM(tmp));
		}

		[Test]
		public void RespectsFileWIthoutUTF8BOM()
		{
			var tmp = Path.GetTempFileName();
			File.WriteAllText(tmp, "Hello", new UTF8Encoding(false));
			Assert.IsFalse(SourceFile.HasBOM(tmp));
			var sourcefile = SourceFile.For(tmp);

			sourcefile.Write();
			Assert.IsFalse(SourceFile.HasBOM(tmp));
		}
	}

}
