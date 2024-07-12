﻿using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Result.SourceGenerator;

[Generator]
public class ErrorsGenerator : IIncrementalGenerator
{
    private const string ErrorClassSuffix = "Error";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(predicate: Predicate, transform: Transform);
        var compilation = context.CompilationProvider.Combine(provider.Collect());
        context.RegisterSourceOutput(compilation, Execute);
    }

    private static void Execute(
        SourceProductionContext context,
        (Compilation Compilation, ImmutableArray<ClassDeclarationSyntax> List) input)
    {
        var (compilation, list) = input;

        if (!list.Any())
        {
            return;
        }

        var namespaceList = list
            .Select(node => compilation.GetSemanticModel(node.SyntaxTree).GetDeclaredSymbol(node))
            .Select(symbol => symbol?.ContainingNamespace.ToString())
            .Distinct()
            .ToList();

        var namespaces = string.Join("\n", namespaceList.Select(ns => $"using {ns};"));

        var classBody = string.Join("\n        ", list.Select(node =>
        {
            var className = node.Identifier.Text;
            var methodName = className.EndsWith(ErrorClassSuffix)
                ? className.Substring(0, className.Length - ErrorClassSuffix.Length)
                : className;

            var primaryConstructorParameters = node.ParameterList?.Parameters
                .Select(parameter => parameter.ToString())
                .ToList() ?? [];

            var constructorParameters = node.DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>()
                .Select(constructor => constructor.ParameterList.Parameters.Select(parameter => parameter.ToString()).ToList())
                .Concat(new[] { primaryConstructorParameters })
                .Where(parameters => parameters.Any())
                .ToList();

            return string.Join("\n        ", constructorParameters.Select(parameters =>
                {
                    var methodParametersString = string.Join(", ", parameters);
                    var passedParametersString = string.Join(", ", parameters.Select(parameter => parameter.Split(' ')[1]));

                    return $"public static {className} {methodName}({methodParametersString}) => new {className}({passedParametersString});";
                }));
        }));

        var source = $$"""
           // <auto-generated/>
           {{namespaces}}

           namespace DaeNe.Result
           {
               public partial class Errors
               {
                   {{classBody}}
               }
           }
           """;

        context.AddSource("Errors.g.cs", source);
    }

    private static bool Predicate(SyntaxNode node, CancellationToken cancellationToken)
    {
        return node is ClassDeclarationSyntax classDeclaration &&
               classDeclaration.BaseList?.Types
                   .Any(baseType => baseType.Type.ToString() == "ErrorBase") == true;
    }

    private static ClassDeclarationSyntax Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        return (ClassDeclarationSyntax)context.Node;
    }
}