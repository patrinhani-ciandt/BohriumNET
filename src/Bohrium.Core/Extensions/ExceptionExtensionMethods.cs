namespace Bohrium.Core.Extensions
{
    using System;
    using System.Diagnostics;

    public static class ExceptionExtensionMethods
    {
        // Extension for Exception
        internal static void DebugOutput(this Exception ex)
        {
#if DEBUG
            Debug.WriteLine(ex.Message);
#endif
        }
    }
}
