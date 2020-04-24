using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace iSTMC.PageView
{
   public class TapTip
   {
      [DllImport("user32.dll")]
      public static extern IntPtr FindWindow(string ClassName, string WindowName);

      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll", SetLastError = true)]
      public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);

      public static bool GetEnabled(DependencyObject obj)
      {
         return (bool)obj.GetValue(EnabledProperty);
      }

      public static void SetEnabled(DependencyObject obj, bool value)
      {
         obj.SetValue(EnabledProperty, value);
      }

      public static readonly DependencyProperty EnabledProperty =
          DependencyProperty.RegisterAttached("Eanbled", typeof(bool),
          typeof(TapTip), new UIPropertyMetadata(false, EnabledChanged));

      private static void EnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var ctrl = d as System.Windows.UIElement;
         if (ctrl == null) return;

         bool bEnabled = (bool)e.NewValue;

         if (bEnabled)
         {
            ctrl.PreviewMouseDown += Ctrl_PreviewMouseDown;
            ctrl.PreviewKeyDown += Ctrl_PreviewKeyDown;
         }
         else
         {
            ctrl.PreviewMouseDown -= Ctrl_PreviewMouseDown;
            ctrl.PreviewKeyDown -= Ctrl_PreviewKeyDown;
         }
      }

      private static void Ctrl_PreviewKeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
            CloseKeyboard();
      }

      private static void Ctrl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
      {
         OpenKeyboard();
      }

      private static void OpenKeyboard()
      {
         ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
         startInfo.WindowStyle = ProcessWindowStyle.Hidden;
         Process.Start(startInfo);
      }

      private static void CloseKeyboard()
      {
         uint WM_SYSCOMMAND = 274;
         uint SC_CLOSE = 61536;
         IntPtr KeyboardWnd = FindWindow("IPTip_Main_Window", null);
         PostMessage(KeyboardWnd.ToInt32(), WM_SYSCOMMAND, (int)SC_CLOSE, 0);
      }
   }
}
