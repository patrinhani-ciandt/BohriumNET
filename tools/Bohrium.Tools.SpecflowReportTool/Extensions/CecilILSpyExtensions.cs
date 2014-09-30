using System;
using ICSharpCode.Decompiler;
using ICSharpCode.ILSpy;
using Mono.Cecil;

namespace Bohrium.Tools.SpecflowReportTool.Extensions
{
    public static class CecilILSpyExtensions
    {
        public static string GetSourceCode(this MethodDefinition methodDefinition)
        {
            try
            {
                var csharpLanguage = new CSharpLanguage();
                var textOutput = new PlainTextOutput();
                var decompilationOptions = new DecompilationOptions();
                decompilationOptions.FullDecompilation = true;
                csharpLanguage.DecompileMethod(methodDefinition, textOutput, decompilationOptions);

                return textOutput.ToString();
            }
            catch (Exception exception)
            {
                return ("Error in creating source code from IL: " + exception.Message);
            }
        }
    }
}
