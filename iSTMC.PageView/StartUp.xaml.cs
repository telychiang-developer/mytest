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
   /// StartUp.xaml 的互動邏輯
   /// </summary>
   [PageView("StartUp", Description = "智慧櫃員機STM系統啟動")]
   public partial class StartUp : Page
   {
      private readonly IKernelService _kernelService;

      public StartUp(IKernelService kernelService)
      {
         InitializeComponent();
         _kernelService = kernelService;
      }
   }
}
