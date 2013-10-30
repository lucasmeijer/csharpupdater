using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptUpdating;

namespace IntegrationTests
{
	class CanResolveThingsThroughOtherFiles : MultiFileIntegrationTest
	{
		protected override void CSharp(out IEnumerable<SourceFile> i, out string e)
		{
			i = new[]
			    {
				    new SourceFile()
				    {
					    Contents = "class C : ClassInOtherFile { void Start() { var s = transform.name; } }",
					    FileName = "C.cs"
				    },
				    new SourceFile()
				    {
					    Contents = "class ClassInOtherFile : UnityEngine.MonoBehaviour {}",
					    FileName = "ClassInOtherFile.cs"
				    }
			    };

			e = "class C : ClassInOtherFile { void Start() { var s = Transform.Name; } }";
		}

		protected override void Boo(out IEnumerable<SourceFile> i, out string e)
		{
			i = new[]
			    {
				    new SourceFile()
				    {
					    Contents = "class C(ClassInOtherFile):\n  def S():\n    s as string = transform.name",
					    FileName = "C.boo"
				    },
				    new SourceFile()
				    {
					    Contents = "class ClassInOtherFile(UnityEngine.MonoBehaviour):\n  pass",
					    FileName = "ClassInOtherFile.boo"
				    }
			    };

			e = "class C(ClassInOtherFile):\n  def S():\n    s as string = Transform.Name";
		}

		protected override void Javascript(out IEnumerable<SourceFile> i, out string e)
		{
			i = new[]
			    {
				    new SourceFile()
				    {
					    Contents = "new Rocket().transform;",
					    FileName = "main.ja"
				    },
				    new SourceFile()
				    {
					    Contents = "function Bla() {}",
					    FileName = "Rocket.js"
				    },
			    };

			e = "new Rocket().Transform;";
		}
	}
}
