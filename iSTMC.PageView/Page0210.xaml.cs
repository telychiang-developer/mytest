using iSTMC.Common;
using iSTMC.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iSTMC.PageView
{
   /// <summary>
   /// Page0210.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0210", Description = "資料填寫-確認資料-資料處理中")]
   public partial class Page0210 : Page
   {
      private readonly IKernelService _kernelService;

      public Page0210(IKernelService kernelService)
      {
         InitializeComponent();
         _kernelService = kernelService;
      }

      #region ScrollViewer Touch Manipulation
      private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
      {
         e.Handled = true;
      }

      private void ScrollViewer_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         Rectangle rect = Mouse.DirectlyOver as Rectangle;
         if (rect != null)
         {
            ScrollViewer sv = sender as ScrollViewer;
            ScrollBar scrollBar = sv.Template.FindName("PART_VerticalScrollBar", sv) as ScrollBar;
            Track track = FindVisualChild<Track>(scrollBar);

            double trackHeight = track.ActualHeight;
            double factor = sv.ScrollableHeight / trackHeight;

            Point p = e.GetPosition(track);

            sv.ScrollToVerticalOffset(p.Y * factor);

            //e.Handled = true;
         }
      }

      private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
      {
         for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
         {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is T)
               return (T)child;
            else
            {
               T childOfChild = FindVisualChild<T>(child);
               if (childOfChild != null)
                  return childOfChild;
            }
         }
         return null;
      }
      #endregion
   }
}
