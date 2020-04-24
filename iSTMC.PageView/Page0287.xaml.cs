using iSTMC.Common;
using iSTMC.Kernel;
using System;
using System.Collections.Generic;
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
   /// Page0287.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0287", Description = "開戶設定-文件簽署-資料處理中")]
   public partial class Page0287 : Page
   {
      private readonly IKernelService _kernelService;

      public Page0287(IKernelService kernelService)
      {
         InitializeComponent();
         _kernelService = kernelService;
      }
   }
}
