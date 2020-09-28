using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace StandardUI.CodeGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine($"Usage: StandardUI.CodelGenerator.exe <path-to-repo-root> [path-to-vs-instance]");
                Environment.Exit(1);
            }

            string rootDirectory = NormalizePath(args[0]);

            VisualStudioInstance instance = GetVisualStudioInstance(args.Length == 2 ? NormalizePath(args[1]) : null);

            Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");

            // NOTE: Be sure to register an instance with the MSBuildLocator 
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            MSBuildLocator.RegisterInstance(instance);

            using MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);


            string standardUIProjectPath = Path.Combine(rootDirectory, "src", "StandardUI", "StandardUI.csproj");
            Console.WriteLine($"Loading project '{standardUIProjectPath}'");

            // Attach progress reporter so we print projects as they are loaded.
            Project project = await workspace.OpenProjectAsync(standardUIProjectPath, new ConsoleProgressReporter());
            Console.WriteLine($"Finished loading project '{standardUIProjectPath}'");

            try
            {
                GenerateClasses(rootDirectory, workspace, project);
            }
            catch (UserViewableException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.ToString()}");
                Environment.Exit(2);
            }
        }

        private static VisualStudioInstance GetVisualStudioInstance(string? vsPath)
        {
            VisualStudioInstance? instance = null;
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            if (visualStudioInstances.Length == 1)
            {
                // If there is only one instance of MSBuild on this machine, set that as the one to use.
                instance = visualStudioInstances[0];
            }
            else if (visualStudioInstances.Length > 1 && vsPath != null)
            {
                foreach (var currInstance in visualStudioInstances)
                {
                    string vsRootPath = NormalizePath(currInstance.VisualStudioRootPath);

                    if (vsPath.StartsWith(vsRootPath))
                    {
                        instance = currInstance;
                        break;
                    }
                }
            }

            if (instance == null)
            {
                // Let user select the instance to use manually
                instance = SelectVisualStudioInstance(visualStudioInstances);
            }

            return instance;
        }

        private static bool HasModelObjectAttribute(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            foreach (AttributeListSyntax attributeList in interfaceDeclaration.AttributeLists)
            {
                foreach (AttributeSyntax attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString() == "UIModelObject")
                        return true;
                }
            }

            return false;
        }

        private static void GenerateClasses(string rootDirectory, Workspace workspace, Project project)
        {
			Compilation? compilation = project.GetCompilationAsync().Result;
            if (compilation == null)
                return;

            var interfaces = new Dictionary<string, InterfaceDeclarationSyntax>();
            var attachedInterfaces = new Dictionary<string, InterfaceDeclarationSyntax>();
            foreach (SyntaxTree? tree in compilation.SyntaxTrees)
            {
                foreach (InterfaceDeclarationSyntax? intface in tree.GetRoot().DescendantNodesAndSelf().OfType<InterfaceDeclarationSyntax>())
                {
                    string name = intface.Identifier.Text;

                    if (name.EndsWith("Attached"))
                        attachedInterfaces.Add(name, intface);
                    else interfaces.Add(name, intface);
                }
            }

            foreach (InterfaceDeclarationSyntax intface in interfaces.Values)
            {
                if (!HasModelObjectAttribute(intface))
                    continue;

                InterfaceDeclarationSyntax? attachedInterface = null;
                if (attachedInterfaces.TryGetValue(intface.Identifier.Text + "Attached", out InterfaceDeclarationSyntax value))
                    attachedInterface = value;

                Console.WriteLine($"Processing {intface.Identifier.Text}");
                new Interface(new Context(workspace, rootDirectory, WpfXamlOutputType.Instance), intface, attachedInterface).Generate();
                //new SourceFileGenerator(workspace, interfaceDeclaration, rootDirectory, XamarinFormsXamlOutputType.Instance).Generate();
                //new SourceFileGenerator(workspace, interfaceDeclaration, rootDirectory, StandardModelOutputType.Instance).Generate();
            }
        }

        private static string NormalizePath(string path)
        {
            path = path.Trim();
            try
            {
                return Path.GetFullPath(path).TrimEnd('\\').ToLowerInvariant();
            }
            catch (ArgumentException)
            {
                // If invalid path, leave unmodified
                return path;
            }
        }

        private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            Console.WriteLine("Multiple installs of MSBuild detected please select one:");
            for (int i = 0; i < visualStudioInstances.Length; i++)
            {
                Console.WriteLine($"Instance {i + 1}");
                Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
                Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
                Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
            }

            while (true)
            {
                var userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out int instanceNumber) &&
                    instanceNumber > 0 &&
                    instanceNumber <= visualStudioInstances.Length)
                {
                    return visualStudioInstances[instanceNumber - 1];
                }
                Console.WriteLine("Input not accepted, try again.");
            }
        }

        private class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
        {
            public void Report(ProjectLoadProgress loadProgress)
            {
                var projectDisplay = Path.GetFileName(loadProgress.FilePath);
                if (loadProgress.TargetFramework != null)
                {
                    projectDisplay += $" ({loadProgress.TargetFramework})";
                }

                Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
            }
        }
    }
}
