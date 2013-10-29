using System.Collections.Generic;

namespace ScriptUpdating
{
	public interface IScriptUpdater
	{
		string Update(IEnumerable<SourceFile> input);
		string UpdateSmall(IEnumerable<SourceFile> input);
	}
}