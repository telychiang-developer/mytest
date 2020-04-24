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
   /// Page0200.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0200", Description = "開戶設定-文件簽署-開戶申請書列印完成")]
   public partial class Page0200 : Page
   {
      private readonly IKernelService _kernelService;

      public Page0200(IKernelService kernelService)
      {
         InitializeComponent();
         _kernelService = kernelService;
      }
   }
}
