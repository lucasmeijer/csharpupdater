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

	private Document _document;

	private List<Replacement> _replacements = new List<Replacement>();

	public ReplacementCollector(Document document)
	{
		_document = document;
	}

	public void Add(LexicalInfo lexicalInfo, int length, string replacementstring)
	{
		_replacements.Add(new Replacement() {length = length, start = lexicalInfo, replacementstring = replacementstring});
	}

	public string ApplyOn(string input)
	{
		string result = input;
		var orderByDescending = _replacements.OrderByDescending(r => _document.LexicalInfoToOffset(r.start)).ToArray();
		foreach (var replacement in orderByDescending)
		{
			var startOffset = _document.LexicalInfoToOffset(replacement.start);
			var endOffset = startOffset + replacement.length;
			var tail = endOffset < result.Length ? result.Substring(endOffset) : "";
			result = result.Substring(0,startOffset) + replacement.replacementstring + tail;
		}
		return result;
	}
}