using System.Collections.Generic;
using System.Linq;
using csharpupdater;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.CSharp;
using NUnit.Framework;
using Expression = ICSharpCode.NRefactory.CSharp.Expression;

class StringBasedGetComponentReplacer : ReplacingAstVisitor
{
	public StringBasedGetComponentReplacer(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		: base(replacementCollector, resolver)
	{
	}

	public override void VisitInvocationExpression(InvocationExpression invocationExpression)
	{
		base.VisitInvocationExpression(invocationExpression);

		string argumentused;
		if (!IsInvocationToStringBasedGetComponent(invocationExpression,out argumentused)) 
			return;

		//doing a manual resolve here. as I don't know how to use NRefactory to resolve an identifier that we make on the fly from the argumenstring,   in the context of the "UnresolvedFile".
		var oldType = _resolver.Compilation.ReferencedAssemblies.Single().GetTypeDefinition("UnityEngine", argumentused);
		var replacement = oldType == null ? argumentused : oldType.FullName;
	
		_replacementCollector.Add(invocationExpression.Arguments.Single(),"typeof("+replacement+")");
	}

	private bool IsInvocationToStringBasedGetComponent(InvocationExpression invocationExpression, out string argumentUsed)
	{
		argumentUsed = null;

		var resolveResult = _resolver.Resolve(invocationExpression.Target);
		var methodGroupResolveResult = resolveResult as MethodGroupResolveResult;
		if (methodGroupResolveResult == null)
			return false;

		var fullName = methodGroupResolveResult.Methods.First().FullName;
		if (fullName != "UnityEngine.Component.GetComponent" && fullName != "UnityEngine.GameObject.GetComponent")
			return false;

		if (invocationExpression.Arguments.Count() != 1)
			return false;

		var primitiveExpression = invocationExpression.Arguments.Single() as PrimitiveExpression;
		if (primitiveExpression == null)
			return false;

		argumentUsed = primitiveExpression.Value as string;
		
		return argumentUsed != null;
	}
}


class StringBasedGetComponentReplacerTests : CSharpUpdaterTestsBase
{
	[Test]
	public void OnImplicitThis()
	{
		var i = "using UnityEngine; class L : MonoBehaviour { void Start() { GetComponent(\"Light\"); } } ";
		var e = "using UnityEngine; class L : MonoBehaviour { void Start() { GetComponent(typeof(UnityEngine.Light)); } } ";
		Test(e, i);
	}

	[Test]
	public void OnMemberReference()
	{
		var i = "using UnityEngine; class L { void Start() { Component c = null; c.GetComponent(\"Light\"); } } ";
		var e = "using UnityEngine; class L { void Start() { Component c = null; c.GetComponent(typeof(UnityEngine.Light)); } } ";
		Test(e, i);
	}

	[Test]
	public void UsingUserScriptAsArgument()
	{
		var i = "using UnityEngine; class L { void Start() { Component c = null; c.GetComponent(\"Rocket\"); } } ";
		var e = "using UnityEngine; class L { void Start() { Component c = null; c.GetComponent(typeof(Rocket)); } } ";
		Test(e, i);
	}

	[Test]
	public void GameObject_OnExplicit()
	{
		var i = "using UnityEngine; class L { void Start() { new GameObject().GetComponent(\"Transform\"); } } ";
		var e = "using UnityEngine; class L { void Start() { new GameObject().GetComponent(typeof(UnityEngine.Transform)); } } ";
		Test(e, i);
	}

	[Test]
	public void GameObject_Through_Property()
	{
		var i = "using UnityEngine; class L : MonoBehaviour { void Start() { this.gameObject.GetComponent(\"Transform\"); } } ";
		var e = "using UnityEngine; class L : MonoBehaviour { void Start() { this.gameObject.GetComponent(typeof(UnityEngine.Transform)); } } ";
		Test(e, i);
	}

	[Test]
	public void WithNonLiteralArgument()
	{
		AssertIsNotModified("using UnityEngine; class L : MonoBehaviour { void Start() { var s = \"Light\"; GetComponent(s); } } ");
	}

	protected override IEnumerable<ReplacingAstVisitor> PipelineForTest(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
	{
		yield return new StringBasedGetComponentReplacer(replacementCollector, resolver);
	}
}
