using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add : 
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;



namespace dllCoreBB
{
    public class Builder
    {
            // TODO : Extraer los codigos base de un recurso externo, revisar las opciones de concatenado y formatting $""
            //          Ademas, pensar en un proceso de encriptado - desencriptado de las piezas durante la construccion.
        private string _testCode = @"
              using System;
              using System.Windows.Forms;

              namespace Ejemplo
              {
                static class Program
                {
                  static void Main()
                  {
                    MessageBox.Show(""It's Enjoy"");
                  }
                }
              }
        ";
        public Builder()
        {
            var syntaxTree = CreateSyntaxTree();
            CompileCode(syntaxTree);
        }
        private SyntaxTree CreateSyntaxTree()
        {
            return CSharpSyntaxTree.ParseText(_testCode);
        }

        private void CompileCode(SyntaxTree syntaxTree)
        {
            var compiler = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } }); // Specify CorMainDLL compiler

            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;  // Generate an EXE
            //parameters.CompilerOptions = "/target:exe";  // Explicitly target EXE - Consola 
            parameters.CompilerOptions = "/target:winexe";

            // Ensure appropriate references for CorMainDll
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");

            // Inherit assembly references (if needed)
            var assembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = assembly.GetReferencedAssemblies();
            foreach (var referencedAssemblyName in referencedAssemblies)
            {
                parameters.ReferencedAssemblies.Add($"{referencedAssemblyName.Name}.dll");
            }

            parameters.MainClass = "Ejemplo.Program";
            parameters.OutputAssembly = "prueba.exe";

            var results = compiler.CompileAssemblyFromSource(parameters, _testCode);

            if (results.Errors.Count == 0)
            {
               // MessageBox.Show("Programa compilado exitosamente!");
            }
            else
            {
                foreach (var error in results.Errors)
                {
                   // MessageBox.Show($"Error de compilación: {error}");
                }
            }
        }
    }
}
