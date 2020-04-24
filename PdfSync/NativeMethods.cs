using System;
using System.Runtime.InteropServices;

namespace PdfSync
{
   internal class NativeMethods
   {
      public delegate int StdioCallBack(IntPtr handle, IntPtr strptr, int len);
      [DllImport(@"gsdll32.dll", EntryPoint = "gsapi_delete_instance")]
      public static extern void gsapi_delete_instance(IntPtr instance);

      [DllImport(@"gsdll32.dll", EntryPoint = "gsapi_exit")]
      public static extern int gsapi_exit(IntPtr instance);

      [DllImport(@"gsdll32.dll", EntryPoint = "gsapi_init_with_args")]
      public static extern int gsapi_init_with_args(IntPtr instance, int argc, IntPtr argv);

      [DllImport(@"gsdll32", EntryPoint = "gsapi_new_instance")]
      public static extern int gsapi_new_instance(out IntPtr pinstance, IntPtr caller_handle);

      [DllImport(@"gsdll32.dll", EntryPoint = "gsapi_revision")]
      public static extern int gsapi_revision(out NativeStructs.GS_Revision pr, int len);

      [DllImport("gsdll32.dll", EntryPoint = "gsapi_run_file")]
      public static extern int gsapi_run_file(IntPtr instance, string file_name, int user_errors, int pexit_code);

      [DllImport("gsdll32.dll", EntryPoint = "gsapi_set_stdio")]
      public static extern int gsapi_set_stdio(IntPtr instance, StdioCallBack stdin_fn, StdioCallBack stdout_fn, StdioCallBack stderr_fn);

   }
}
