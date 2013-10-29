using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Editor;

namespace CSharpUpdater
{
	public class ReplacementCollector
	{
		struct Replacement
		{
			public TextLocation Start;
			public TextLocation End;
			public string ReplaceString;

			public Replacement(AstNode astNode, string replaceString)
			{
				Start = astNode.StartLocation;
				End = astNode.EndLocation;
				this.ReplaceString = replaceString;
			}
		}

		readonly List<Replacement> _replacements = new List<Replacement>();

		void Add(Replacement replacement)
		{
			_replacements.Add(replacement);
		}

		public string ApplyTo(ReadOnlyDocument doc)
		{
			var input = doc.Text;
			foreach (var replacement in _replacements.OrderBy(t => t.Start).Reverse())
			{
				input = input.Substring(0, doc.GetOffset(replacement.Start)) + replacement.ReplaceString +
				        input.Substring(doc.GetOffset(replacement.End));
			}
			return input;
		}

		public void Add(AstNode astNode, string replacement)
		{
			Add(new Replacement(astNode,replacement));
		}
	}
}