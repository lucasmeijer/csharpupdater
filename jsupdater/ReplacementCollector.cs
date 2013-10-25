using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;

internal class ReplacementCollector
{
	struct Replacement
	{
		public LexicalInfo start;
		public int length;
		public string replacementstring;
	}

	private List<Replacement> _replacements = new List<Replacement>();

	public void Add(LexicalInfo lexicalInfo, int length, string replacementstring)
	{
		_replacements.Add(new Replacement() {length = length, start = lexicalInfo, replacementstring = replacementstring});
	}

	public string ApplyOn(string input)
	{
		string result = input;
		var orderByDescending = _replacements.OrderByDescending(r => LexicalInfoToOffset(r.start, input)).ToArray();
		foreach (var replacement in orderByDescending)
		{
			var startOffset = LexicalInfoToOffset(replacement.start, input);
			var endOffset = startOffset + replacement.length;
			var tail = endOffset < result.Length ? result.Substring(endOffset) : "";
			result = result.Substring(0,startOffset) + replacement.replacementstring + tail;
		}
		return result;
	}

	private int LexicalInfoToOffset(LexicalInfo info, string input)
	{
		int charcount = 1;
		int linecount = 1;
		int offsetCounter = 0;
		while (true)
		{
			if (info.Line == linecount && info.Column == charcount)
				return offsetCounter;

			if (input[offsetCounter] == '\n')
			{
				linecount++;
				charcount = 1;
			}
			else
			{
				charcount++;
			}
			offsetCounter++;
		}

	}
}