﻿using iSTMC.Common;
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
   /// Page0070.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0070", Description = "上傳證件-證件掃描-放入身分證反面朝下")]
   public partial class Page0070 : Page
   {
      private readonly IKernelService _kernelService;

      public Page0070(IKernelService kernelService)
      {
         InitializeComponent();
         _kernelService = kernelService;
      }

   }
}
