
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace iSTMC.PageView
{
   public class STMMaskedTextBox : MaskedTextBox
   {
      private bool _justGotFocus = false;

      protected override void OnGotTouchCapture(TouchEventArgs e)
      {
         base.OnGotTouchCapture(e);
         _justGotFocus = true;
      }

      protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
      {
         base.OnGotKeyboardFocus(e);
         _justGotFocus = true;
      }

      protected override void OnGotMouseCapture(MouseEventArgs e)
      {
         base.OnGotMouseCapture(e);
         _justGotFocus = true;
      }

      protected override void OnSelectionChanged(RoutedEventArgs e)
      {
         base.OnSelectionChanged(e);

         if (_justGotFocus)
         {
            _justGotFocus = false;

            if ((this.MaskedTextProvider.AssignedEditPositionCount == 0)
            && (this.CaretIndex != 0))
            {
               this.CaretIndex = 0;
            }
         }
      }
   }
}
