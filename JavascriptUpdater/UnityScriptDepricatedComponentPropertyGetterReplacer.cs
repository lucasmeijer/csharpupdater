using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BooUpdater;

namespace JavascriptUpdater
{
	class UnityScriptDepricatedComponentPropertyGetterReplacer : DepricatedComponentPropertyGetterReplacer
	{
		public UnityScriptDepricatedComponentPropertyGetterReplacer(ReplacementCollector replacementCollector) : base(replacementCollector)
		{
		}

		protected override string ReplacementStringFor(string type)
		{
			return "GetComponent(" + type + ")";
		}
	}
}
