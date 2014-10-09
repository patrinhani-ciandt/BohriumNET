using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bohrium.Tools.SpecflowReportTool.Utils
{
    public class AssemblyLoader : IDisposable
    {
        private Assembly loadedAssembly = null;

        public Assembly LoadedAssembly
        {
            get { return loadedAssembly; }
            private set { loadedAssembly = value; }
        }

        public AssemblyLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += currentDomain_AssemblyResolve;
        }

        private Assembly loadAssemblyByPath(string assemblyPath)
        {
            Assembly assemblyLoaded;

            assemblyLoaded = Assembly.LoadFrom(assemblyPath);

            return assemblyLoaded;
        }

        public Assembly LoadAssembly(string assemblyPath)
        {
            LoadedAssembly = loadAssemblyByPath(assemblyPath);

            return LoadedAssembly;
        }

        #region Events

        private Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly missedAssembly = null;

            //This handler is called only when the common language runtime tries to bind to the assembly and fails.
            //Retrieve the list of referenced assemblies in an array of AssemblyName.

            string assemblyDllName = string.Format("{0}.dll", args.Name.Substring(0, args.Name.IndexOf(",", StringComparison.Ordinal)));

            if (LoadedAssembly == null) return missedAssembly;

            var strTempAssmbPath = Path.Combine(Path.GetDirectoryName(LoadedAssembly.Location), assemblyDllName);

            try
            {
                missedAssembly = loadAssemblyByPath(strTempAssmbPath);
            }
            catch (Exception)
            {
                string appDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                try
                {
                    missedAssembly = loadAssemblyByPath(Path.Combine(appDirectory, assemblyDllName));
                }
                catch (Exception) { }
            }

            //Return the loaded assembly.
            return missedAssembly;
        }

        #endregion Events

        #region IDisposable Implementation

        // Flag: Has Dispose already been called? 
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                LoadedAssembly = null;

                AppDomain.CurrentDomain.AssemblyResolve -= currentDomain_AssemblyResolve;
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;
        }

        #endregion IDisposable Implementation
    }
}
