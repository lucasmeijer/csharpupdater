﻿using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using ScriptUpdating;
using CSharpParser = ICSharpCode.NRefactory.CSharp.CSharpParser;

namespace CSharpUpdater
{
	public class CSharpUpdater : IScriptUpdater
	{
		public string Update(string input)
		{
			return Update(input, null);
		}

		public string UpdateSmall(string input)
		{
			return Update(input, SmallPipeline);
		}

		class SourceFileData
		{
			public SyntaxTree SyntaxTree;
			public CSharpUnresolvedFile UnresolvedFile;
			public ReadOnlyDocument Document;
			public SourceFile SourceFile;
		}

		internal string Update(string input, Func<ReplacementCollector, CSharpAstResolver,IEnumerable<ReplacingAstVisitor>> pipeLineProvider)
		{
			IProjectContent project = new CSharpProjectContent();
			var cecilLoader = new CecilLoader { LazyLoad = true };
			var assembly = cecilLoader.LoadAssemblyFile("C:/Program Files (x86)/Unity/Editor/Data/PlaybackEngines/windowsstandaloneplayer/Managed/UnityEngine.dll");
			project = project.AddAssemblyReferences(assembly);
			project.AddAssemblyReferences(new CecilLoader().LoadAssemblyFile(typeof(object).Assembly.Location));

			var sourceFilesData = SourceFilesDataFor(input);

			project = AddSourceFilesToProject(sourceFilesData, project);

			if (pipeLineProvider == null)
				pipeLineProvider = BuildPipeline;

			var compilation = project.CreateCompilation();

			foreach (var sourceFileData in sourceFilesData)
			{
				var resolver = new CSharpAstResolver(compilation, sourceFileData.SyntaxTree, sourceFileData.UnresolvedFile);
				var replacementCollector = new ReplacementCollector();

				foreach (var visitor in pipeLineProvider(replacementCollector, resolver))
					sourceFileData.SyntaxTree.AcceptVisitor(visitor);

				sourceFileData.SourceFile.Contents = replacementCollector.ApplyTo(sourceFileData.Document);
			}

			return sourceFilesData.Single().SourceFile.Contents;
		}

		private static IProjectContent AddSourceFilesToProject(IEnumerable<SourceFileData> sourceFilesData, IProjectContent project)
		{
			foreach (var sourceFileData in sourceFilesData)
				project = project.AddOrUpdateFiles((sourceFileData.UnresolvedFile));

			return project;
		}

		private static List<SourceFileData> SourceFilesDataFor(string input)
		{
			var sourceFilesData = new List<SourceFileData>();

			var sourceFiles = new List<SourceFile>() {new SourceFile() {Contents = input, FileName = "bla.cs"}};

			foreach (var sourceFile in sourceFiles)
			{
				var doc = new ReadOnlyDocument(input);
				var syntaxTree = new CSharpParser().Parse(doc, sourceFile.FileName);
				var unresolvedFile = syntaxTree.ToTypeSystem();
				sourceFilesData.Add(new SourceFileData()
				                    {
					                    Document = doc,
					                    SyntaxTree = syntaxTree,
					                    UnresolvedFile = unresolvedFile,
					                    SourceFile = sourceFile
				                    });
			}
			return sourceFilesData;
		}

		private IEnumerable<ReplacingAstVisitor> BuildPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		{
			yield return new MemberReferenceReplacer(replacementCollector, resolver);
			yield return new TypeReferenceReplacer(replacementCollector, resolver);
			yield return new DepricatedComponentPropertyGetterReplacer(replacementCollector, resolver);
			yield return new PropertyUpperCaser(replacementCollector, resolver);
			yield return new StringBasedGetComponentReplacer(replacementCollector,resolver);
		}

		private IEnumerable<ReplacingAstVisitor> SmallPipeline(ReplacementCollector replacementCollector, CSharpAstResolver resolver)
		{
			yield return new PropertyUpperCaser(replacementCollector, resolver, true);
		}
	}
}