using iSTMC.Common;
using iSTMC.Kernel;
using iSTMC.Packmodels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using WpfChosenControl;

namespace iSTMC.PageView
{
   /// <summary>
   /// Page0175.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0175", Description = "資料填寫-個人資料-填寫個人資料175")]
   public partial class Page0175 : Page
   {
      private Page0175ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0175(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0175ViewModel(kernelService, "Page0175");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0175_Loaded;
      }

      private void CheckFundSource(string val)
      {
         switch(val)
         {
            case "1":  
               _pageVM.FundSource1 = true;
               break;
            case "2":  
               _pageVM.FundSource2 = true;
               break;
            case "3":  
               _pageVM.FundSource3 = true;
               break;
            case "4":  
               _pageVM.FundSource4 = true;
               break;
            case "5":  
               _pageVM.FundSource5 = true;
               break;
            case "6":  
               _pageVM.FundSource6 = true;
               break;
            case "7":  
               _pageVM.FundSource7 = true;
               break;
            case "8":  
               _pageVM.FundSource8 = true;
               break;
            case "9":  
               _pageVM.FundSource9 = true;
               break;
            case "A":  
               _pageVM.FundSourceA = true;
               break;
            case "B":  
               _pageVM.FundSourceB = true;
               break;
            default:
               break;
         }
      }

      private void Page0175_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData175))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData175].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.FundSource1 = jsonObj["FundSource1"] == "Y";
            _pageVM.FundSource2 = jsonObj["FundSource2"] == "Y";
            _pageVM.FundSource3 = jsonObj["FundSource3"] == "Y";
            _pageVM.FundSource4 = jsonObj["FundSource4"] == "Y";
            _pageVM.FundSource5 = jsonObj["FundSource5"] == "Y";
            _pageVM.FundSource6 = jsonObj["FundSource6"] == "Y";
            _pageVM.FundSource7 = jsonObj["FundSource7"] == "Y";
            _pageVM.FundSource8 = jsonObj["FundSource8"] == "Y";
            _pageVM.FundSource9 = jsonObj["FundSource9"] == "Y";
            _pageVM.FundSourceA = jsonObj["FundSourceA"] == "Y";
            _pageVM.FundSourceB = jsonObj["FundSourceB"] == "Y";

            _pageVM.FundSourceADesc = jsonObj["FundSourceADesc"];
            _pageVM.FundSourceBDesc = jsonObj["FundSourceBDesc"];
            _pageVM.HasOldAccount = jsonObj["HasOldAccount"] == "Y";
         }
         else
         {
            _pageVM.HasOldAccount = false;
            if (_kernelService.TransactionDataCache.ContainsKey("__WARN_ACCOUNT_DATA"))
            {
               string json = _kernelService.TransactionDataCache["__WARN_ACCOUNT_DATA"].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               if (jsonObj.ContainsKey("oldAccResult"))
               {
                  _pageVM.HasOldAccount = jsonObj["oldAccResult"] == "Y" || jsonObj["oldAccResult"] == "P";
                  _pageVM.IsPreProcAccount = jsonObj["oldAccResult"] == "P";
               }
            }

            if (_pageVM.HasOldAccount && _kernelService.TransactionDataCache.ContainsKey("__OLD_ACCOUNT_DATA"))
            {
               string oldAccountData = _kernelService.TransactionDataCache["__OLD_ACCOUNT_DATA"].ToString();
               var oaData = JsonConvert.DeserializeObject<Dictionary<string, string>>(oldAccountData);

               _kernelService.Logger.Info("Page0175: " + oldAccountData);

               foreach (string key in oaData.Keys)
               {
                  switch (key)
                  {
                     case "INCOME_RESOURCE_1":      
                     case "INCOME_RESOURCE_2":      
                     case "INCOME_RESOURCE_3":      
                        {
                           string val = oaData[key];
                           CheckFundSource(val);
                        }
                        break;
                     case "INCOME_BUSINESS_INCOME":  
                        _pageVM.FundSourceADesc = oaData[key];
                        break;
                     case "INCOME_RESOURCE_OTHER":  
                        _pageVM.FundSourceBDesc = oaData[key];
                        break;
                     default:
                        break;
                  }
               }
            }
            else
            {
               if (_pageVM.HasOldAccount)
                  _kernelService.Logger.Warn("__OLD_ACCOUNT_DATA is not found!");
            }
         }

         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      private void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         _pageVM.StopTimer();

         var model = new
         {
            FundSource1 = _pageVM.FundSource1 ? "Y" : "N",          
            FundSource2 = _pageVM.FundSource2 ? "Y" : "N",          
            FundSource3 = _pageVM.FundSource3 ? "Y" : "N",          
            FundSource4 = _pageVM.FundSource4 ? "Y" : "N",          
            FundSource5 = _pageVM.FundSource5 ? "Y" : "N",          
            FundSource6 = _pageVM.FundSource6 ? "Y" : "N",          
            FundSource7 = _pageVM.FundSource7 ? "Y" : "N",          
            FundSource8 = _pageVM.FundSource8 ? "Y" : "N",          
            FundSource9 = _pageVM.FundSource9 ? "Y" : "N",          
            FundSourceA = _pageVM.FundSourceA ? "Y" : "N",          
            _pageVM.FundSourceADesc,                                
            FundSourceB = _pageVM.FundSourceB ? "Y" : "N",          
            _pageVM.FundSourceBDesc,
            HasOldAccount = _pageVM.HasOldAccount ? "Y" : "N",
            IsPreProcAccount = _pageVM.IsPreProcAccount ? "Y" : "N",
         };

         string json = JsonConvert.SerializeObject(model);
         _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData175, json);

         PageResult result = new PageResult("Previous", KioskDataCacheKey.PersonalData175);
         _kernelService.NextPage(result);
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         List<string> errorList = new List<string>();
         PageViewUtils.GetErrors(errorList, grdForm);

         if(_pageVM.FundSourceB && string.IsNullOrEmpty(_pageVM.FundSourceBDesc))
            errorList.Add("勾選資金來源B其他內容未填寫");

         int fundSourceCount = 0;
         if (_pageVM.FundSource1) fundSourceCount++;
         if (_pageVM.FundSource2) fundSourceCount++;
         if (_pageVM.FundSource3) fundSourceCount++;
         if (_pageVM.FundSource4) fundSourceCount++;
         if (_pageVM.FundSource5) fundSourceCount++;
         if (_pageVM.FundSource6) fundSourceCount++;
         if (_pageVM.FundSource7) fundSourceCount++;
         if (_pageVM.FundSource8) fundSourceCount++;
         if (_pageVM.FundSource9) fundSourceCount++;
         if (_pageVM.FundSourceA) fundSourceCount++;
         if (_pageVM.FundSourceB) fundSourceCount++;

         if(fundSourceCount == 0)
            errorList.Add("資金來源必須至少需勾選一項");

         if (_pageVM.FundSourceA && string.IsNullOrEmpty(_pageVM.FundSourceADesc))
            errorList.Add("主要交易對象及商品未填寫");

         if (errorList.Count > 0)
         {
            ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
            var popup = new ValidationErrorDialog(errorList.ToArray(), sm);
            sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);
         }
         else
         {
            _pageVM.StopTimer();

            List<string> fundSources = new List<string>();
            if (_pageVM.FundSource1) fundSources.Add("薪資");
            if(_pageVM.FundSource2) fundSources.Add("獎金");
            if (_pageVM.FundSource3) fundSources.Add("投資孳息/紅利");
            if (_pageVM.FundSource4) fundSources.Add("退休金");
            if (_pageVM.FundSource5) fundSources.Add("遺產");
            if (_pageVM.FundSource6) fundSources.Add("保險金所得");
            if (_pageVM.FundSource7) fundSources.Add("房租或物業出售");
            if (_pageVM.FundSource8) fundSources.Add("信託收入");
            if (_pageVM.FundSource9) fundSources.Add("投資出售");
            if (_pageVM.FundSourceA) fundSources.Add($"營業收入({_pageVM.FundSourceADesc})");
            if (_pageVM.FundSourceB) fundSources.Add($"其他({_pageVM.FundSourceBDesc})");

            string fundSourceDesc = string.Join("、", fundSources.ToArray());

            var model = new
            {
               FundSource1 = _pageVM.FundSource1 ? "Y" : "N",          
               FundSource2 = _pageVM.FundSource2 ? "Y" : "N",          
               FundSource3 = _pageVM.FundSource3 ? "Y" : "N",          
               FundSource4 = _pageVM.FundSource4 ? "Y" : "N",          
               FundSource5 = _pageVM.FundSource5 ? "Y" : "N",          
               FundSource6 = _pageVM.FundSource6 ? "Y" : "N",          
               FundSource7 = _pageVM.FundSource7 ? "Y" : "N",          
               FundSource8 = _pageVM.FundSource8 ? "Y" : "N",          
               FundSource9 = _pageVM.FundSource9 ? "Y" : "N",          
               FundSourceA = _pageVM.FundSourceA ? "Y" : "N",          
               _pageVM.FundSourceADesc,                                
               FundSourceB = _pageVM.FundSourceB ? "Y" : "N",          
               _pageVM.FundSourceBDesc,                                
               HasOldAccount = _pageVM.HasOldAccount ? "Y" : "N",
               IsPreProcAccount = _pageVM.IsPreProcAccount ? "Y" : "N",

               FundSourceDesc = fundSourceDesc
            };

            _pageVM.Clear();

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData175, json);

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.PersonalData175);
            _kernelService.NextPage(result);
         }
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

   public class Page0175ViewModel : PageViewModel
   {
      private IKernelService _kernelService;

      public Page0175ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         _kernelService = kernelService;
         Clear();
      }

      public List<int> SelectedFundSources
      {
         get { return GetValue(() => SelectedFundSources); }
         set
         {
            SetValue(() => SelectedFundSources, value);
         }
      }

      public bool FundSource1
      {
         get { return GetValue(() => FundSource1); }
         set
         {
            if (value)
            {
               if(SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSource1, value);
                  SelectedFundSources.Add(0x01);
               }
               else
               {
                  SetValue(() => FundSource1, !value);
               }
            }
            else
            {
               SetValue(() => FundSource1, value);

               if (SelectedFundSources.Contains(0x01))
                  SelectedFundSources.Remove(0x01);
            }
         }
      }

      public bool FundSource2
      {
         get { return GetValue(() => FundSource2); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSource2, value);
                  SelectedFundSources.Add(0x02);
               }
               else
               {
                  SetValue(() => FundSource2, !value);
               }
            }
            else
            {
               SetValue(() => FundSource2, value);

               if (SelectedFundSources.Contains(0x02))
                  SelectedFundSources.Remove(0x02);
            }
         }
      }

      public bool FundSource3
      {
         get { return GetValue(() => FundSource3); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSource3, value);
                  SelectedFundSources.Add(0x03);
               }
               else
               {
                  SetValue(() => FundSource3, !value);
               }
            }
            else
            {
               SetValue(() => FundSource3, value);

               if (SelectedFundSources.Contains(0x03))
                  SelectedFundSources.Remove(0x03);
            }
         }
      }

      public bool FundSource4
      {
         get { return GetValue(() => FundSource4); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSource4, value);
                  SelectedFundSources.Add(0x04);
               }
               else
               {
                  SetValue(() => FundSource4, !value);
               }
            }
            else
            {
               SetValue(() => FundSource4, value);

               if (SelectedFundSources.Contains(0x04))
                  SelectedFundSources.Remove(0x04);
            }
         }
      }

      public bool FundSource5
      {
         get { return GetValue(() => FundSource5); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSource5, value);
                  SelectedFundSources.Add(0x05);
               }
               else
               {
                  SetValue(() => FundSource5, !value);
               }
            }
            else
            {
               SetValue(() => FundSource5, value);

               if (SelectedFundSources.Contains(0x05))
                  SelectedFundSources.Remove(0x05);
            }
         }
      }

      public bool FundSource6
      {
         get { return GetValue(() => FundSource6); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSource6, value);
                  SelectedFundSources.Add(0x06);
               }
               else
               {
                  SetValue(() => FundSource6, !value);
               }
            }
            else
            {
               SetValue(() => FundSource6, value);

               if (SelectedFundSources.Contains(0x06))
                  SelectedFundSources.Remove(0x06);
            }
         }
      }

      public bool FundSource7
      {
         get { return GetValue(() => FundSource7); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSource7, value);
                  SelectedFundSources.Add(0x07);
               }
               else
               {
                  SetValue(() => FundSource7, !value);
               }
            }
            else
            {
               SetValue(() => FundSource7, value);

               if (SelectedFundSources.Contains(0x07))
                  SelectedFundSources.Remove(0x07);
            }
         }
      }

      public bool FundSource8
      {
         get { return GetValue(() => FundSource8); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSource8, value);
                  SelectedFundSources.Add(0x08);
               }
               else
               {
                  SetValue(() => FundSource8, !value);
               }
            }
            else
            {
               SetValue(() => FundSource8, value);

               if (SelectedFundSources.Contains(0x08))
                  SelectedFundSources.Remove(0x08);
            }
         }
      }

      public bool FundSource9
      {
         get { return GetValue(() => FundSource9); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSource9, value);
                  SelectedFundSources.Add(0x09);
               }
               else
               {
                  SetValue(() => FundSource9, !value);
               }
            }
            else
            {
               SetValue(() => FundSource9, value);

               if (SelectedFundSources.Contains(0x09))
                  SelectedFundSources.Remove(0x09);
            }
         }
      }

      public bool FundSourceA
      {
         get { return GetValue(() => FundSourceA); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSourceA, value);
                  SelectedFundSources.Add(0x0A);
               }
               else
               {
                  SetValue(() => FundSourceA, !value);
               }
            }
            else
            {
               SetValue(() => FundSourceA, value);

               if (SelectedFundSources.Contains(0x0A))
                  SelectedFundSources.Remove(0x0A);

               FundSourceADesc = "";
            }
         }
      }

      public string FundSourceADesc
      {
         get { return GetValue(() => FundSourceADesc); }
         set
         {
            SetValue(() => FundSourceADesc, value);
         }
      }

      public bool FundSourceB
      {
         get { return GetValue(() => FundSourceB); }
         set
         {
            if (value)
            {
               if (SelectedFundSources.Count < 3)
               {
                  SetValue(() => FundSourceB, value);
                  SelectedFundSources.Add(0x0B);
               }
               else
               {
                  SetValue(() => FundSourceB, !value);
               }
            }
            else
            {
               SetValue(() => FundSourceB, value);

               if (SelectedFundSources.Contains(0x0B))
                  SelectedFundSources.Remove(0x0B);

               FundSourceBDesc = "";
            }
         }
      }

      public string FundSourceBDesc
      {
         get { return GetValue(() => FundSourceBDesc); }
         set
         {
            SetValue(() => FundSourceBDesc, value);
         }
      }

      public bool HasOldAccount
      {
         get { return GetValue(() => HasOldAccount); }
         set
         {
            SetValue(() => HasOldAccount, value);
         }
      }

      public string WarnResultA
      {
         get { return GetValue(() => WarnResultA); }
         set
         {
            SetValue(() => WarnResultA, value);
         }
      }

      public bool IsPreProcAccount
      {
         get { return GetValue(() => IsPreProcAccount); }
         set
         {
            SetValue(() => IsPreProcAccount, value);
         }
      }

      public void Clear()
      {
         SelectedFundSources = new List<int>();
         FundSource1 = false;
         FundSource2 = false;
         FundSource3 = false;
         FundSource4 = false;
         FundSource5 = false;
         FundSource6 = false;
         FundSource7 = false;
         FundSource8 = false;
         FundSource9 = false;
         FundSourceA = false;
         FundSourceB = false;
         FundSourceADesc = "";
         FundSourceBDesc = "";
         HasOldAccount = false;
         WarnResultA = "";
         IsPreProcAccount = false;
      }
   }
}
