using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace iSTMC.PageView
{
   public class InputIMESwitch
   {
      public static InputScopeNameValue GetIME(DependencyObject obj)
      {
         return (InputScopeNameValue)obj.GetValue(IMEProperty);
      }

      public static void SetIME(DependencyObject obj, InputScopeNameValue value)
      {
         obj.SetValue(IMEProperty, value);
      }

      public static readonly DependencyProperty IMEProperty =
          DependencyProperty.RegisterAttached("IME", typeof(InputScopeNameValue),
          typeof(InputIMESwitch), new UIPropertyMetadata(InputScopeNameValue.Default, IMEChanged));

      static void IMEChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var ctrlText = d as System.Windows.Controls.TextBox;
         var ctrlPswd = d as System.Windows.Controls.PasswordBox;
         if (ctrlText == null && ctrlPswd == null) return;

         InputScope scope = new InputScope();
         InputScopeNameValue value = (InputScopeNameValue)e.NewValue;
         InputScopeName scopeName = new InputScopeName(value);
         scope.Names.Add(scopeName);

         if (ctrlText != null)
            ctrlText.InputScope = scope;

         if (ctrlPswd != null)
            ctrlPswd.InputScope = scope;
      }
   }
}
