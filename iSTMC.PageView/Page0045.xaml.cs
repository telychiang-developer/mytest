﻿using iSTMC.Common;
using iSTMC.Kernel;
using Newtonsoft.Json;
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
   /// Page0045.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0045", Description = "開戶認證-確認條款-稅務身分聲明確認")]
   public partial class Page0045 : Page
   {
      private Page0045ViewModel _pageVM;
      private readonly IKernelService _kernelService;
      private Vector _totalScale = new Vector(1.0, 1.0);

      public Page0045(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0045ViewModel(kernelService, "Page0045");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0045_Loaded;
      }

      private void Page0045_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.OCRConfirmData))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.OCRConfirmData].ToString();

            if(!string.IsNullOrEmpty(json))
            {
               try
               {
                  var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                  string dob = jsonObj["DateOfBirth"];
                  var dtDOB = DateTime.Parse(dob);

                  _pageVM.FullName = $"{jsonObj["ChineseName"]} {jsonObj["EnglishName"]}";
                  _pageVM.PID = jsonObj["PID"];
                  _pageVM.DateOfBirth = $"{dtDOB.Year} / {dtDOB.Month} / {dtDOB.Day}";
               }
               catch (Exception ex)
               {
                  _kernelService.Logger.Error(ex, $"Page0045_Loaded: {ex.Message}");
               }
            }
         }

         scrollViewer.ScrollToTop();

         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
      {
         e.Handled = true;
      }
      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         _pageVM.StopTimer();

         _pageVM.Clear();

         PageResult result = new PageResult("Confirm", null);
         _kernelService.NextPage(result);
      }

      private void btnHome_Click(object sender, RoutedEventArgs e)
      {
         ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
         var popup = new ConfirmBackHome(sm);

         sm.Closed += ScalingModal_Closed;
         sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);
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
      }
   }

   public class Page0045ViewModel : PageViewModel
   {
      public Page0045ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         Nationality = "中華民國 Taiwan (R.O.C)";
      }

      public string FullName
      {
         get { return GetValue(() => FullName); }
         set { SetValue(() => FullName, value); }
      }

      public string PID
      {
         get { return GetValue(() => PID); }
         set { SetValue(() => PID, value); }
      }

      public string DateOfBirth
      {
         get { return GetValue(() => DateOfBirth); }
         set { SetValue(() => DateOfBirth, value); }
      }

      public string Nationality
      {
         get { return GetValue(() => Nationality); }
         set { SetValue(() => Nationality, value); }
      }

      public void Clear()
      {
         FullName = "";
         PID = "";
         DateOfBirth = "";
         Nationality = "中華民國 Taiwan (R.O.C)";
      }
   }
}
