using System;

namespace CSCore.Utils
{
    internal class UnhandledException
    {
        public static void AddHandler()
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            Context.Current.Logger.Info("Registered Exceptionhandler(Unhandled)");
        }

        private static void UnhandledExceptionHandler(System.Object sender, UnhandledExceptionEventArgs args)
        {
            Context.Current.Logger.Error((Exception)args.ExceptionObject, "UnknownLocation", false);
            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}