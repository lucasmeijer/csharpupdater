using Boo.Lang.Compiler.Ast;

namespace BooUpdater
{
	internal abstract class ReplacingAstVisitor : DepthFirstVisitor
	{
		protected readonly ReplacementCollector _replacementCollector;

		protected ReplacingAstVisitor(ReplacementCollector replacementCollector, Document document)
		{
			_replacementCollector = replacementCollector;
		}
	}
}