using System.Collections.Generic;
using System.Linq;
using csharpupdater;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using Mono.CSharp;
using NUnit.Framework;

class StringBasedGetComponentReplacer : ReplacingAstVisotor
{
	public StringBasedGetComponentReplacer(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		: base(replacementCollector, resolver)
	{
	}

	public override void VisitInvocationExpression(InvocationExpression invocationExpression)
	{
		base.VisitInvocationExpression(invocationExpression);
		
		if (!IsInvocationToStringBasedGetComponent(invocationExpression)) 
			return;

		_replacementCollector.Add(invocationExpression.Arguments.Single(),"typeof(Unity.Runtime.Rendering.Light)");
	}

	private bool IsInvocationToStringBasedGetComponent(InvocationExpression invocationExpression)
	{
		var resolveresult = _resolver.Resolve(invocationExpression.Target) as MethodGroupResolveResult;
		if (resolveresult == null)
			return false;

		var fullName = resolveresult.Methods.First().FullName;
		if (fullName != "UnityEngine.Component.GetComponent" && fullName != "UnityEngine.GameObject.GetComponent")
			return false;

		if (invocationExpression.Arguments.Count() != 1)
			return false;

		var arg = invocationExpression.Arguments.FirstOrDefault();
		var primitiveExpression = arg as PrimitiveExpression;
		if (primitiveExpression == null)
			return false;

		var resolvedPrimitive = _resolver.Resolve(primitiveExpression);
		if (resolvedPrimitive.Type.FullName != "System.String")
			return false;

		if ((string) primitiveExpression.Value != "Light")
			return false;

		return true;
	}
}


class StringBasedGetComponentReplacerTests : CSharpUpdaterTestsBase
{
	[Test]
	public void OnImplicitThis()
	{
		var i = "using UnityEngine; class L : MonoBehaviour { void Start() { GetComponent(\"Light\"); } } ";
		var e = "using UnityEngine; class L : MonoBehaviour { void Start() { GetComponent(typeof(Unity.Runtime.Rendering.Light)); } } ";
		Test(e, i);
	}

	[Test]
	public void OnMemberReference()
	{
		var i = "using UnityEngine; class L { void Start() { Component c = null; c.GetComponent(\"Light\"); } } ";
		var e = "using UnityEngine; class L { void Start() { Component c = null; c.GetComponent(typeof(Unity.Runtime.Rendering.Light)); } } ";
		Test(e, i);
	}

	[Test]
	public void GameObject_OnImplicitThis()
	{
		var i = "using UnityEngine; class L { void Start() { new GameObject().GetComponent(\"Light\"); } } ";
		var e = "using UnityEngine; class L { void Start() { new GameObject().GetComponent(typeof(Unity.Runtime.Rendering.Light)); } } ";
		Test(e, i);
	}

	[Test]
	public void WithNonLiteralArgument()
	{
		AssertIsNotModified("using UnityEngine; class L : MonoBehaviour { void Start() { var s = \"Light\"; GetComponent(s); } } ");
	}

	protected override IEnumerable<ReplacingAstVisotor> GetPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
	{
		yield return new StringBasedGetComponentReplacer(replacementCollector, resolver);
	}
}
