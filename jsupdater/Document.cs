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

			if (_text[offsetCounter] == '\n')
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