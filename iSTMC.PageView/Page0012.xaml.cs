using iSTMC.Common;
using iSTMC.Kernel;
using System;
using System.Collections.Generic;
using System.IO;
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
   /// Page0012.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0012", Description = "開戶認證-掃描身分證-放入身分證正面朝下")]
   public partial class Page0012 : Page
   {
      private readonly IKernelService _kernelService;

      public Page0012(IKernelService kernelService)
      {
         InitializeComponent();
         _kernelService = kernelService;
      }
   }
}
