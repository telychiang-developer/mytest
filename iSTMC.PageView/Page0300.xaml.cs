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
   /// Error.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0300", Description = "不符合開戶條件，交易中止")]
   public partial class Page0300 : Page
   {
      private readonly IKernelService _kernelService;

      public Page0300(IKernelService kernelService)
      {
         InitializeComponent();

         _kernelService = kernelService;
      }
   }
}
