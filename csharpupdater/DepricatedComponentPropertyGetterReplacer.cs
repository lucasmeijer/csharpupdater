using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

internal class DepricatedComponentPropertyGetterReplacer : ReplacingAstVisotor
{
	public DepricatedComponentPropertyGetterReplacer(ReplacementCollector replacementCollector, CSharpAstResolver resolver) : base(replacementCollector, resolver)
	{
	}

	IEnumerable<Tuple<string, string>> PropertiesToReplace()
	{
		yield return new Tuple<string, string>("rigidbody","Unity.Runtime.Physics.RigidBody");
		yield return new Tuple<string, string>("collider", "Unity.Runtime.Physics.Collider");
		yield return new Tuple<string, string>("constantForce", "Unity.Runtime.Physics.ConstantForce");
		yield return new Tuple<string, string>("camera", "Unity.Runtime.Rendering.Camera");
		yield return new Tuple<string, string>("animation", "Unity.Runtime.DepricatedAnimation.Animation");
	}

	public override void VisitIdentifierExpression(IdentifierExpression identifierExpression)
	{
		base.VisitIdentifierExpression(identifierExpression);
		ReplacePropertyReferenceIfRequired(identifierExpression,identifierExpression);
	}

	public override void VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression)
	{
		base.VisitMemberReferenceExpression(memberReferenceExpression);
		ReplacePropertyReferenceIfRequired(memberReferenceExpression,memberReferenceExpression.MemberNameToken);
	}

	private void ReplacePropertyReferenceIfRequired(AstNode thingThatShouldResolveToProperty, AstNode expressionToReplace)
	{
		var match = PropertiesToReplace().SingleOrDefault(p => IsMemberReferenceTo(thingThatShouldResolveToProperty, "UnityEngine.Component."+p.Item1));
		if (match == null)
			return;
		
		_replacementCollector.Add(expressionToReplace, "GetComponent<"+match.Item2+">()");
	}
}