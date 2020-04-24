using iSTMC.Common;
using iSTMC.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
   /// Timeout.xaml 的互動邏輯
   /// </summary>
   [PageView("Timeout", Description = "交易逾時")]
   public partial class Timeout : Page
   {
      private PageViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Timeout(IKernelService kernelService)
      {
         InitializeComponent();

         _kernelService = kernelService;

         _pageVM = new PageViewModel(kernelService, "Timeout");

         this.DataContext = _pageVM;

         this.Loaded += Timeout_Loaded;
      }

      private void Timeout_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      private void btnCancel_Click(object sender, RoutedEventArgs e)
      {
         ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
         var popup = new ConfirmBackHome(sm);

         sm.Closed += ScalingModal_Closed;
         sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);
      }

      private void btnRetry_Click(object sender, RoutedEventArgs e)
      {
         _pageVM.StopTimer();

         PageResult result = new PageResult("Retry", "");
         _kernelService.NextPage(result);
      }

      private void ScalingModal_Closed(object sender, EventArgs e)
      {
         ConfirmBackHome popup = sender as ConfirmBackHome;

         if (popup.IsConfirmed)
         {
            _pageVM.StopTimer();

            PageResult result = new PageResult("Home");
            _kernelService.NextPage(result);
         }
         else
         {
            _pageVM.ResetTimer();
         }
      }
   }
}
