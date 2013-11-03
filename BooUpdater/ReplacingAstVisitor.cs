using Boo.Lang.Compiler.Ast;

namespace BooUpdater
{
	public abstract class ReplacingAstVisitor : DepthFirstVisitor
	{
		protected readonly ReplacementCollector _replacementCollector;

		protected ReplacingAstVisitor(ReplacementCollector replacementCollector)
		{
			_replacementCollector = replacementCollector;
		}
	}
}