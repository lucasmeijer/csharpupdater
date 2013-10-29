using Boo.Lang.Compiler.Ast;

internal class Document
{
	private readonly string _text;

	public Document(string text)
	{
		_text = text;
	}

	public int LexicalInfoToOffset(LexicalInfo info)
	{
		int charcount = 1;
		int linecount = 1;
		int offsetCounter = 0;
		while (true)
		{
			if (info.Line == linecount && info.Column == charcount)
				return offsetCounter;

			var c = _text[offsetCounter];
			
			if (c == "\u000A"[0])
			{
				linecount++;
				charcount = 1;
			} else if (c == "\u000D"[0])
			{
			}
			
			else if (c == '\t')
			{
				charcount++;
				while (charcount%8 != 1) charcount++;
			}
			else
			{
				charcount++;
			}
			offsetCounter++;
		}

	}
}