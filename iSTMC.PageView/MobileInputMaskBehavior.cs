using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace iSTMC.PageView
{
   public class MobileInputMaskBehavior
   {
      public static bool GetMobileInputMask(DependencyObject obj)
      {
         return (bool)obj.GetValue(MobileInputMaskProperty);
      }

      public static void SetMobileInputMask(DependencyObject obj, bool value)
      {
         obj.SetValue(MobileInputMaskProperty, value);
      }

      public static readonly DependencyProperty MobileInputMaskProperty =
          DependencyProperty.RegisterAttached("MobileInputMask", typeof(bool), typeof(MobileInputMaskBehavior), new PropertyMetadata(false, (o, e) =>
          {
             var input = o as TextBox;
             if (input == null)
             {
                return;
             }

             if ((bool)e.NewValue)
                input.TextInput += Input_TextInput;
             else
                input.TextInput -= Input_TextInput;
          }));

      private static void Input_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
      {
         if(e.Text.Length > 4)
         {
            
         }
      }
   }
}
