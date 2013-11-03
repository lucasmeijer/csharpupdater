namespace IntegrationTests
{
	class WillReplaceInsidePreprocessorIf : IntegrationTest
	{
		protected override void CSharp(out string i, out string e)
		{
			i = "#if UNITY_EDITOR\nclass C : UnityEngine.MonoBehaviour { void S() { Transform t = transform; } }\n#endif";
			e = "#if UNITY_EDITOR\nclass C : UnityEngine.MonoBehaviour { void S() { Transform t = Transform; } }\n#endif";
		}

		protected override void Boo(out string i, out string e)
		{
			i = "#if UNITY_EDITOR\nclass C(UnityEngine.MonoBehaviour):\n def S():\n  t = transform\n#endif";
			e = "#if UNITY_EDITOR\nclass C(UnityEngine.MonoBehaviour):\n def S():\n  t = Transform\n#endif";
		}

		protected override void Javascript(out string i, out string e)
		{
			i = "#if UNITY_EDITOR\nvar t = transform;\n#endif";
			e = "#if UNITY_EDITOR\nvar t = Transform;\n#endif";
		}
	}
}
