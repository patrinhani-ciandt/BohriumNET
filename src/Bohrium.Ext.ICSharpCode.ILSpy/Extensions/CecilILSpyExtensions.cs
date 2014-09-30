using System;
using ICSharpCode.Decompiler;
using ICSharpCode.ILSpy;
using Mono.Cecil;

namespace Bohrium.Ext.ICSharpCode.ILSpy.Extensions
{
    /// <summary>
    /// Extension Method for ILSpy and Cecil Types
    /// </summary>
    public static class CecilILSpyExtensions
    {
        /// <summary>
        /// Decompile the source code for the LoadedAssembly
        /// </summary>
        /// <param name="assemblyDefinition"></param>
        /// <returns></returns>
        public static string GetSourceCode(this LoadedAssembly assemblyDefinition)
        {
            try
            {
                var csharpLanguage = new CSharpLanguage();
                var textOutput = new PlainTextOutput();
                var decompilationOptions = new DecompilationOptions();
                decompilationOptions.FullDecompilation = true;
                csharpLanguage.DecompileAssembly(assemblyDefinition, textOutput, decompilationOptions);

                return textOutput.ToString();
            }
            catch (Exception exception)
            {
                return ("Error in creating source code from IL: " + exception.Message);
            }
        }

        /// <summary>
        /// Decompile the source code for the TypeDefinition
        /// </summary>
        /// <param name="typeDefinition"></param>
        /// <returns></returns>
        public static string GetSourceCode(this TypeDefinition typeDefinition)
        {
            try
            {
                var csharpLanguage = new CSharpLanguage();
                var textOutput = new PlainTextOutput();
                var decompilationOptions = new DecompilationOptions();
                decompilationOptions.FullDecompilation = true;
                csharpLanguage.DecompileType(typeDefinition, textOutput, decompilationOptions);

                return textOutput.ToString();
            }
            catch (Exception exception)
            {
                return ("Error in creating source code from IL: " + exception.Message);
            }
        }

        /// <summary>
        /// Decompile the source code for the PropertyDefinition
        /// </summary>
        /// <param name="propertyDefinition"></param>
        /// <returns></returns>
        public static string GetSourceCode(this PropertyDefinition propertyDefinition)
        {
            try
            {
                var csharpLanguage = new CSharpLanguage();
                var textOutput = new PlainTextOutput();
                var decompilationOptions = new DecompilationOptions();
                decompilationOptions.FullDecompilation = true;
                csharpLanguage.DecompileProperty(propertyDefinition, textOutput, decompilationOptions);

                return textOutput.ToString();
            }
            catch (Exception exception)
            {
                return ("Error in creating source code from IL: " + exception.Message);
            }
        }

        /// <summary>
        /// Decompile the source code for the FieldDefinition
        /// </summary>
        /// <param name="fielDefinition"></param>
        /// <returns></returns>
        public static string GetSourceCode(this FieldDefinition fielDefinition)
        {
            try
            {
                var csharpLanguage = new CSharpLanguage();
                var textOutput = new PlainTextOutput();
                var decompilationOptions = new DecompilationOptions();
                decompilationOptions.FullDecompilation = true;
                csharpLanguage.DecompileField(fielDefinition, textOutput, decompilationOptions);

                return textOutput.ToString();
            }
            catch (Exception exception)
            {
                return ("Error in creating source code from IL: " + exception.Message);
            }
        }

        /// <summary>
        /// Decompile the source code for the FieldDefinition
        /// </summary>
        /// <param name="methodDefinition"></param>
        /// <returns></returns>
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
