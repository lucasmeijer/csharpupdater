using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;
using NUnit.Framework;
using ScriptUpdating;

namespace BooUpdater
{
	internal class ReplacementCollector
	{
		class Replacement
		{
			public LexicalInfo start;
			public int length;
			public string replacementstring;
		}

		private readonly Dictionary<string,List<Replacement>> _replacements = new Dictionary<string, List<Replacement>>();

		public ReplacementCollector()
		{
		}

		public void Add(LexicalInfo lexicalInfo, int length, string replacementstring)
		{
			List<Replacement> replacements = null;
			_replacements.TryGetValue(lexicalInfo.FileName, out replacements);
			if (replacements == null)
			{
				replacements = new List<Replacement>();
				_replacements[lexicalInfo.FileName] = replacements;
			}

			//sometimes boo will expand something like this  bla |= 2 into  bla = bla | 2,  and therefore the visitor will visit bla twice. this is fine.
			var preexisting = replacements.FirstOrDefault(r => r.start == lexicalInfo);
			if (preexisting != null)
			{
				if (preexisting.replacementstring != replacementstring || preexisting.length != length)
					throw new ArgumentException("Replacement being requested on a location that already has a replacement. " + lexicalInfo);
				return;
			}

			replacements.Add(new Replacement() {length = length, start = lexicalInfo, replacementstring = replacementstring});
		}

		public void ApplyOn(IEnumerable<SourceFile> input)
		{
			foreach (var sf in input)
			{
				List<Replacement> replacements = null;
				if (_replacements.TryGetValue(sf.FileName, out replacements))
					ApplyOn(sf, replacements);
			}
		}

		private void ApplyOn(SourceFile sourceFile, IEnumerable<Replacement> replacements)
		{
			var doc = new Document(sourceFile.Contents);
			string result = sourceFile.Contents;
			var orderByDescending = replacements.OrderByDescending(r => doc.LexicalInfoToOffset(r.start)).ToArray();
			foreach (var replacement in orderByDescending)
			{
				var startOffset = doc.LexicalInfoToOffset(replacement.start);
				var endOffset = startOffset + replacement.length;
				var tail = endOffset < result.Length ? result.Substring(endOffset) : "";
				result = result.Substring(0,startOffset) + replacement.replacementstring + tail;
			}


			if (sourceFile.FileName.Contains("PatrolPoint.js"))
			{
				Console.WriteLine("ReplacementCount: "+replacements.Count());
				Console.WriteLine("Before");
				Console.WriteLine(sourceFile.Contents);
				Console.WriteLine("After");
				Console.WriteLine(result);
			}

			sourceFile.Contents = result;
		}
	}
}