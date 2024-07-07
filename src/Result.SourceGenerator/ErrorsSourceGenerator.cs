﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Result.SourceGenerator
{
    [Generator]
    public sealed class ErrorsSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var errors = context.Compilation.SyntaxTrees
                .SelectMany(tree => tree.GetRoot().DescendantNodes())
                .OfType<ClassDeclarationSyntax>()
                .Where(classDeclaration => classDeclaration.BaseList?.Types
                    .Any(baseType => baseType.Type.ToString() == "ErrorBase") == true)
                .ToList();

            if (errors.Count == 0)
            {
                return;
            }

            var namespaces = errors
                .Select(error => context.Compilation.GetSemanticModel(error.SyntaxTree).GetDeclaredSymbol(error))
                .Select(error => error?.ContainingNamespace.ToString())
                .Where(ns => ns != null)
                .Distinct()
                .ToList();

            var source = $@"// <auto-generated/>
{GetNamespaces(namespaces)}

namespace DaeNe.Result
{{
  public partial class Errors
  {{
{GetClassBody(errors)}
  }}
}}";
            context.AddSource("Errors.g.cs", source);
        }

        private static string GetNamespaces(IEnumerable<string> namespaces) =>
            string.Join("\n", namespaces.Select(ns => $"using {ns};"));

        private static string GetClassBody(IEnumerable<ClassDeclarationSyntax> errors) =>
            $@"{string.Join("\n", errors.Select(error =>
            {
                const string suffixToRemove = "Error";

                var errorName = error.Identifier.Text;
                var methodName = errorName.EndsWith(suffixToRemove)
                    ? errorName.Substring(0, errorName.Length - suffixToRemove.Length)
                    : errorName;

                var constructorParameters = error.ParameterList.Parameters
                    .Select(parameter => parameter.ToString())
                    .ToList();

                var methodParametersString = string.Join(", ", constructorParameters);
                var passedParametersString = string.Join(", ", constructorParameters.Select(parameter => parameter.Split(' ')[1]));

                return $"    public static {errorName} {methodName}({methodParametersString}) => new {errorName}({passedParametersString});";
            }))}";
    }
}
