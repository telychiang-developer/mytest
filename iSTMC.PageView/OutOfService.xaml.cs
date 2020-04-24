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
   /// OutOfService.xaml 的互動邏輯
   /// </summary>
   [PageView("OutOfService", Description = "智慧櫃員機STM系統暫停服務")]
   public partial class OutOfService : Page
   {
      private int secretCount = 0;

      private DateTime? beginSecretClick;

      private readonly IKernelService _kernelService;

      public OutOfService(IKernelService kernelService)
      {
         InitializeComponent();
         _kernelService = kernelService;
      }

      private void Image_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (!_kernelService.TestMode)
            return;

         if (secretCount == 0)
            beginSecretClick = DateTime.Now;

         secretCount++;

         if (secretCount >= 5)
         {
            secretCount = 0;

            if ((DateTime.Now - beginSecretClick.Value).TotalSeconds <= 3)
            {
               if (MessageBox.Show("是否結束系統", "結束系統", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
               {
                  var window = this.Parent as NavigationWindow;
                  if (window != null)
                     window.Close();
               }
            }
         }
      }
   }
}
