using iSTMC.Common;
using iSTMC.Kernel;
using iSTMC.Packmodels;
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
   /// Page0177.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0177", Description = "資料填寫-個人資料-填寫個人資料177")]
   public partial class Page0177 : Page
   {
      private Page0177ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0177(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0177ViewModel(kernelService, "Page0177");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0177_Loaded;
      }

      private void CheckWealthySource(string val)
      {
         switch(val)
         {
            case "1":  
               _pageVM.WealthySource1 = true;
               break;
            case "2":  
               _pageVM.WealthySource2 = true;
               break;
            case "3":  
               _pageVM.WealthySource3 = true;
               break;
            case "4":  
               _pageVM.WealthySource4 = true;
               break;
            case "5":  
               _pageVM.WealthySource5 = true;
               break;
            case "6":  
               _pageVM.WealthySource6 = true;
               break;
            default:
               break;
         }
      }

      private void CheckWealthySourceSub(string wsMajor, string wsSub, string wsOther)
      {
         switch (wsMajor)
         {
            case "1":  
               switch(wsSub)
               {
                  case "A":
                     _pageVM.WealthySource1A = true;
                     break;
                  case "B":
                     _pageVM.WealthySource1B = true;
                     _pageVM.WealthySource1BDesc = wsOther;
                     break;
                  default:
                     break;
               }
               break;
            case "2":  
               switch (wsSub)
               {
                  case "A":
                     _pageVM.WealthySource2A = true;
                     break;
                  case "B":
                     _pageVM.WealthySource2B = true;
                     break;
                  case "C":
                     _pageVM.WealthySource2C = true;
                     break;
                  case "D":
                     _pageVM.WealthySource2D = true;
                     break;
                  case "E":
                     _pageVM.WealthySource2E = true;
                     _pageVM.WealthySource2EDesc = wsOther;
                     break;
                  default:
                     break;
               }
               break;
            case "3":  
               switch (wsSub)
               {
                  case "A":
                     _pageVM.WealthySource3A = true;
                     break;
                  case "B":
                     _pageVM.WealthySource3B = true;
                     break;
                  case "C":
                     _pageVM.WealthySource3C = true;
                     break;
                  case "D":
                     _pageVM.WealthySource3D = true;
                     break;
                  case "E":
                     _pageVM.WealthySource3E = true;
                     break;
                  default:
                     break;
               }
               break;
            case "6":  
               _pageVM.WealthySource6Desc = wsOther;
               break;
            default:
               break;
         }
      }


      private void Page0177_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         TerminalConfig config = _kernelService.TerminalDataCache["__TerminalConfig"] as TerminalConfig;
         _pageVM.BranchNo = config.BranchNo;

         if (_kernelService.TransactionDataCache.ContainsKey("__WEALTHY_SOURCE"))
            _pageVM.WealthySourceDropdown = _kernelService.TransactionDataCache["__WEALTHY_SOURCE"] as WealthySourceDropdown;
         else
            _kernelService.Logger.Warn("__WEALTHY_SOURCE is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData177))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData177].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.WealthySource1 = jsonObj["WealthySource1"] == "Y";
            _pageVM.WealthySource1A = jsonObj["WealthySource1A"] == "Y";
            _pageVM.WealthySource1B = jsonObj["WealthySource1B"] == "Y";
            _pageVM.WealthySource1BDesc = jsonObj["WealthySource1BDesc"];
            _pageVM.WealthySource2 = jsonObj["WealthySource2"] == "Y";
            _pageVM.WealthySource2A = jsonObj["WealthySource2A"] == "Y";
            _pageVM.WealthySource2B = jsonObj["WealthySource2B"] == "Y";
            _pageVM.WealthySource2C = jsonObj["WealthySource2C"] == "Y";
            _pageVM.WealthySource2D = jsonObj["WealthySource2D"] == "Y";
            _pageVM.WealthySource2E = jsonObj["WealthySource2E"] == "Y";
            _pageVM.WealthySource2EDesc = jsonObj["WealthySource2EDesc"];
            _pageVM.WealthySource3 = jsonObj["WealthySource3"] == "Y";
            _pageVM.WealthySource3A = jsonObj["WealthySource3A"] == "Y";
            _pageVM.WealthySource3B = jsonObj["WealthySource3B"] == "Y";
            _pageVM.WealthySource3C = jsonObj["WealthySource3C"] == "Y";
            _pageVM.WealthySource3D = jsonObj["WealthySource3D"] == "Y";
            _pageVM.WealthySource3E = jsonObj["WealthySource3E"] == "Y";
            _pageVM.WealthySource4 = jsonObj["WealthySource4"] == "Y";
            _pageVM.WealthySource5 = jsonObj["WealthySource5"] == "Y";
            _pageVM.WealthySource6 = jsonObj["WealthySource6"] == "Y";
            _pageVM.WealthySource6Desc = jsonObj["WealthySource6Desc"];
            _pageVM.StampRefNumber = jsonObj["StampRefNumber"] == "Y";
            _pageVM.StampRefNumberDesc = jsonObj["StampRefNumberDesc"];
            _pageVM.StampRefNumberEnabled = jsonObj["StampRefNumberEnabled"] == "Y";
            _pageVM.HasOldAccount = jsonObj["HasOldAccount"] == "Y";
            _pageVM.AccountType = jsonObj["HasOldAccount"];
            _pageVM.IsShowStampMsg = jsonObj["IsShowStampMsg"] == "Y";
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

               _kernelService.Logger.Info("Page0177: " + oldAccountData);

               foreach (string key in oaData.Keys)
               {
                  switch (key)
                  {
                     case "ASSET_RESOURCE_1":         
                     case "ASSET_RESOURCE_2":         
                     case "ASSET_RESOURCE_3":         
                        CheckWealthySource(oaData[key]);
                        break;
                     case "ASSET_RESOURCE_SUB_1":     
                        {
                           string wsMajor = oaData["ASSET_RESOURCE_1"];
                           string wsOther = oaData["ASSET_RESOURCE_1_SUB_OTHER"];
                           CheckWealthySourceSub(wsMajor, oaData[key], wsOther);
                        }
                        break;
                     case "ASSET_RESOURCE_SUB_2":     
                        {
                           string wsMajor = oaData["ASSET_RESOURCE_2"];
                           string wsOther = oaData["ASSET_RESOURCE_2_SUB_OTHER"];
                           CheckWealthySourceSub(wsMajor, oaData[key], wsOther);
                        }
                        break;
                     case "ASSET_RESOURCE_SUB_3":     
                        {
                           string wsMajor = oaData["ASSET_RESOURCE_3"];
                           string wsOther = oaData["ASSET_RESOURCE_3_SUB_OTHER"];
                           CheckWealthySourceSub(wsMajor, oaData[key], wsOther);
                        }
                        break;
                     case "PBTRANSF":         
                        {
                           _pageVM.StampRefNumber = false;
                           _pageVM.StampRefNumberDesc = "";
                           _pageVM.StampRefNumberEnabled = true;

                        }
                        break;
                     default:
                        break;
                  }
               }
            }
            else
            {
               _pageVM.StampRefNumberEnabled = false;
               if(_pageVM.HasOldAccount)
                  _kernelService.Logger.Warn("__OLD_ACCOUNT_DATA is not found!");
            }

            if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.NewAccountType))
            {
               string json = _kernelService.TransactionDataCache[KioskDataCacheKey.NewAccountType].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               _pageVM.AccountType = jsonObj["accType"];
               _pageVM.IsShowStampMsg = _pageVM.AccountType == "3";
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
            WealthySource1 = _pageVM.WealthySource1 ? "Y" : "N",          
            WealthySource1A = _pageVM.WealthySource1A ? "Y" : "N",          
            WealthySource1B = _pageVM.WealthySource1B ? "Y" : "N",          
            _pageVM.WealthySource1BDesc,     
            WealthySource2 = _pageVM.WealthySource2 ? "Y" : "N",          
            WealthySource2A = _pageVM.WealthySource2A ? "Y" : "N",        
            WealthySource2B = _pageVM.WealthySource2B ? "Y" : "N",        
            WealthySource2C = _pageVM.WealthySource2C ? "Y" : "N",        
            WealthySource2D = _pageVM.WealthySource2D ? "Y" : "N",        
            WealthySource2E = _pageVM.WealthySource2E ? "Y" : "N",        
            _pageVM.WealthySource2EDesc,     
            WealthySource3 = _pageVM.WealthySource3 ? "Y" : "N",          
            WealthySource3A = _pageVM.WealthySource3A ? "Y" : "N",        
            WealthySource3B = _pageVM.WealthySource3B ? "Y" : "N",        
            WealthySource3C = _pageVM.WealthySource3C ? "Y" : "N",        
            WealthySource3D = _pageVM.WealthySource3D ? "Y" : "N",        
            WealthySource3E = _pageVM.WealthySource3E ? "Y" : "N",        
            WealthySource4 = _pageVM.WealthySource4 ? "Y" : "N",          
            WealthySource5 = _pageVM.WealthySource5 ? "Y" : "N",          
            WealthySource6 = _pageVM.WealthySource6 ? "Y" : "N",          
            _pageVM.WealthySource6Desc,      
            StampRefNumber = _pageVM.StampRefNumber ? "Y" : "N",          
            _pageVM.StampRefNumberDesc,      
            StampRefNumberEnabled = _pageVM.StampRefNumberEnabled ? "Y" : "N",
            HasOldAccount = _pageVM.HasOldAccount ? "Y" : "N",
            IsPreProcAccount = _pageVM.IsPreProcAccount ? "Y" : "N",
            _pageVM.AccountType,
            IsShowStampMsg = _pageVM.IsShowStampMsg ? "Y" :"N"
         };

         string json = JsonConvert.SerializeObject(model);
         _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData177, json);

         _pageVM.Clear();

         PageResult result = new PageResult("Previous", KioskDataCacheKey.PersonalData177);
         _kernelService.NextPage(result);
      }

      private string GetWealthySourceSubCode(int majorCode, out string other)
      {
         other = "";

         if (majorCode == 1)
         {
            if (_pageVM.WealthySource1A)
               return "A";
            else if (_pageVM.WealthySource1B)
            {
               other = _pageVM.WealthySource1BDesc;
               return "B";
            }
            else
               return "";
         }
         else if (majorCode == 2)
         {
            if (_pageVM.WealthySource2A)
               return "A";
            else if (_pageVM.WealthySource2B)
               return "B";
            else if (_pageVM.WealthySource2C)
               return "C";
            else if (_pageVM.WealthySource2D)
               return "D";
            else if (_pageVM.WealthySource2E)
            {
               other = _pageVM.WealthySource2EDesc;
               return "E";
            }
            else
               return "";
         }
         else if (majorCode == 3)
         {
            if (_pageVM.WealthySource3A)
               return "A";
            else if (_pageVM.WealthySource3B)
               return "B";
            else if (_pageVM.WealthySource3C)
               return "C";
            else if (_pageVM.WealthySource3D)
               return "D";
            else if (_pageVM.WealthySource3E)
               return "E";
            else
               return "";
         }
         else if (majorCode == 4)
            return "";
         else if (majorCode == 5)
            return "";
         else if (majorCode == 6)
         {
            other = _pageVM.WealthySource6Desc;
            return "";
         }
         else
            return "";
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         List<string> errorList = new List<string>();
         PageViewUtils.GetErrors(errorList, grdForm);

         List<int> wealthySources = new List<int>();
         if (_pageVM.WealthySource1) wealthySources.Add(1);
         if (_pageVM.WealthySource2) wealthySources.Add(2);
         if (_pageVM.WealthySource3) wealthySources.Add(3);
         if (_pageVM.WealthySource4) wealthySources.Add(4);
         if (_pageVM.WealthySource5) wealthySources.Add(5);
         if (_pageVM.WealthySource6) wealthySources.Add(6);

         if (wealthySources.Count == 0)
            errorList.Add("財富來源1~6項目至少需勾選一項");

         if (_pageVM.WealthySource1 && !_pageVM.WealthySource1A && !_pageVM.WealthySource1B)
            errorList.Add("1就業所得右方項目至少需勾選一項");

         if (_pageVM.WealthySource1B && string.IsNullOrEmpty(_pageVM.WealthySource1BDesc))
            errorList.Add("勾選1B營業收入經營年期未填寫");

         if (_pageVM.WealthySource2 && !_pageVM.WealthySource2A && !_pageVM.WealthySource2B && !_pageVM.WealthySource2C && !_pageVM.WealthySource2D && !_pageVM.WealthySource2E)
            errorList.Add("2投資孳息右方項目至少需勾選一項");

         if(_pageVM.WealthySource2E && string.IsNullOrEmpty(_pageVM.WealthySource2EDesc))
            errorList.Add("勾選2E投資孳息E其他說明欄位未填寫");

         if (_pageVM.WealthySource3 && !_pageVM.WealthySource3A && !_pageVM.WealthySource3B && !_pageVM.WealthySource3C && !_pageVM.WealthySource3D && !_pageVM.WealthySource3E)
            errorList.Add("3出售右方項目至少需勾選一項");

         if (_pageVM.WealthySource6 && string.IsNullOrEmpty(_pageVM.WealthySource6Desc))
            errorList.Add("勾選6其他說明欄位未填寫");

         if(_pageVM.StampRefNumber && string.IsNullOrEmpty(_pageVM.StampRefNumberDesc))
            errorList.Add("印鑑參照號碼未填寫");

         if (_pageVM.StampRefNumber)
         {
            AccountCheck accChk = new AccountCheck();

            string actno = _pageVM.BranchNo + _pageVM.StampRefNumberDesc;

            string errMsg = accChk.Validate(actno);
            if (errMsg != "")
            {
               errorList.Add("印鑑參照帳號檢核錯誤");
               _kernelService.Logger.Error($"印鑑參照帳號({actno})檢核錯誤: {errMsg}");
            }            
         }

         if (errorList.Count > 0)
         {
            ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
            var popup = new ValidationErrorDialog(errorList.ToArray(), sm);
            sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);
         }
         else
         {
            _pageVM.StopTimer();

            List<string> wealthySourceNames = new List<string>();

            if (_pageVM.WealthySource1)
            {
               string desc = "就業所得";
               if (_pageVM.WealthySource1A)
                  desc += "-薪資";
               else if (_pageVM.WealthySource1B)
                  desc += "-營業收入(" + _pageVM.WealthySource1BDesc + ")年";

               wealthySourceNames.Add(desc);
            }
            if (_pageVM.WealthySource2)
            {
               string desc = "投資孳息";
               if (_pageVM.WealthySource2A)
                  desc += "-股利";
               else if (_pageVM.WealthySource2B)
                  desc += "-利息";
               else if (_pageVM.WealthySource2C)
                  desc += "-租賃收入";
               else if (_pageVM.WealthySource2D)
                  desc += "-藝術品";
               else if (_pageVM.WealthySource2E)
                  desc += "-其他(" + _pageVM.WealthySource2EDesc + ")";

               wealthySourceNames.Add(desc);
            }
            if (_pageVM.WealthySource3)
            {
               string desc = "出售";
               if (_pageVM.WealthySource3A)
                  desc += "-營業";
               else if (_pageVM.WealthySource3B)
                  desc += "投資";
               else if (_pageVM.WealthySource3C)
                  desc += "-財產(動產/不動產)";
               else if (_pageVM.WealthySource3D)
                  desc += "-股票";
               else if (_pageVM.WealthySource3E)
                  desc += "-保險收益結算";

               wealthySourceNames.Add(desc);
            }
            if (_pageVM.WealthySource4) wealthySourceNames.Add("退休金");
            if (_pageVM.WealthySource5) wealthySourceNames.Add("繼承/家庭贈與");
            if (_pageVM.WealthySource6) wealthySourceNames.Add($"其他({_pageVM.WealthySource6Desc})");

            string wealthySourceDesc = string.Join("、", wealthySourceNames.ToArray());

            string wealthySrc1Other = "";
            string wealthySrc1SubCode = GetWealthySourceSubCode(wealthySources[0], out wealthySrc1Other);

            string wealthySrc2Other = "";
            string wealthySrc2SubCode = wealthySources.Count > 1 ? GetWealthySourceSubCode(wealthySources[1], out wealthySrc2Other) : "";

            string wealthySrc3Other = "";
            string wealthySrc3SubCode = wealthySources.Count > 2 ? GetWealthySourceSubCode(wealthySources[2], out wealthySrc3Other) : "";

            var model = new
            {
               WealthySource1 = _pageVM.WealthySource1 ? "Y" : "N",          
               WealthySource1A = _pageVM.WealthySource1A ? "Y" : "N",          
               WealthySource1B = _pageVM.WealthySource1B ? "Y" : "N",          
               WealthySource1BDesc = _pageVM.WealthySource1B ? _pageVM.WealthySource1BDesc : "",     
               WealthySource2 = _pageVM.WealthySource2 ? "Y" : "N",          
               WealthySource2A = _pageVM.WealthySource2A ? "Y" : "N",        
               WealthySource2B = _pageVM.WealthySource2B ? "Y" : "N",        
               WealthySource2C = _pageVM.WealthySource2C ? "Y" : "N",        
               WealthySource2D = _pageVM.WealthySource2D ? "Y" : "N",        
               WealthySource2E = _pageVM.WealthySource2E ? "Y" : "N",        
               WealthySource2EDesc = _pageVM.WealthySource2E ? _pageVM.WealthySource2EDesc : "",     
               WealthySource3 = _pageVM.WealthySource3 ? "Y" : "N",          
               WealthySource3A = _pageVM.WealthySource3A ? "Y" : "N",        
               WealthySource3B = _pageVM.WealthySource3B ? "Y" : "N",        
               WealthySource3C = _pageVM.WealthySource3C ? "Y" : "N",        
               WealthySource3D = _pageVM.WealthySource3D ? "Y" : "N",        
               WealthySource3E = _pageVM.WealthySource3E ? "Y" : "N",        
               WealthySource4 = _pageVM.WealthySource4 ? "Y" : "N",          
               WealthySource5 = _pageVM.WealthySource5 ? "Y" : "N",          
               WealthySource6 = _pageVM.WealthySource6 ? "Y" : "N",          
               WealthySource6Desc = _pageVM.WealthySource6 ? _pageVM.WealthySource6Desc : "",      
               StampRefNumber = _pageVM.StampRefNumber ? "Y" : "N",          
               StampRefNumberDesc = _pageVM.StampRefNumber ? _pageVM.StampRefNumberDesc : "",      
               StampRefNumberEnabled = _pageVM.StampRefNumberEnabled ? "Y" : "N",
               HasOldAccount = _pageVM.HasOldAccount ? "Y" : "N",
               IsPreProcAccount = _pageVM.IsPreProcAccount ? "Y" : "N",
               _pageVM.AccountType,
               IsShowStampMsg = _pageVM.IsShowStampMsg ? "Y" : "N",

               WealthySource1Code = wealthySources[0].ToString(),
               WealthySource1SubCode = wealthySrc1SubCode,
               WealthySource1Other = wealthySrc1Other,
               WealthySource2Code = wealthySources.Count > 1 ? wealthySources[1].ToString() : "",
               WealthySource2SubCode = wealthySrc2SubCode,
               WealthySource2Other = wealthySrc2Other,
               WealthySource3Code = wealthySources.Count > 2 ? wealthySources[2].ToString() : "",
               WealthySource3SubCode = wealthySrc3SubCode,
               WealthySource3Other = wealthySrc3Other,
               WealthySourceDesc = wealthySourceDesc,
               StampRefAccNo = _pageVM.StampRefNumber ? _pageVM.BranchNo + _pageVM.StampRefNumberDesc : ""
            };

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData177, json);

            _pageVM.Clear();

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.PersonalData177);
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

   public class Page0177ViewModel : PageViewModel
   {
      public Page0177ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         WealthySourceDropdown = new WealthySourceDropdown();

         Clear();
      }

      public WealthySourceDropdown WealthySourceDropdown
      {
         get { return GetValue(() => WealthySourceDropdown); }
         set
         {
            SetValue(() => WealthySourceDropdown, value);
         }
      }

      public List<int> SelectedWealthySources
      {
         get { return GetValue(() => SelectedWealthySources); }
         set
         {
            SetValue(() => SelectedWealthySources, value);
         }
      }

      public bool WealthySource1
      {
         get { return GetValue(() => WealthySource1); }
         set
         {
            if (value)
            {
               if (SelectedWealthySources.Count < 3)
               {
                  SetValue(() => WealthySource1, value);
                  SelectedWealthySources.Add(0x01);
               }
               else
               {
                  SetValue(() => WealthySource1, !value);
               }
            }
            else
            {
               SetValue(() => WealthySource1, value);

               if (SelectedWealthySources.Contains(0x01))
                  SelectedWealthySources.Remove(0x01);

               WealthySource1A = false;
               WealthySource1B = false;
               WealthySource1BDesc = "";
            }
         }
      }

      public bool WealthySource1A
      {
         get { return GetValue(() => WealthySource1A); }
         set
         {
            SetValue(() => WealthySource1A, value);

            if (value)
               WealthySource1B = false;
         }
      }

      public bool WealthySource1B
      {
         get { return GetValue(() => WealthySource1B); }
         set
         {
            SetValue(() => WealthySource1B, value);

            if (value)
               WealthySource1A = false;
            else
               WealthySource1BDesc = "";
         }
      }

      public string WealthySource1BDesc
      {
         get { return GetValue(() => WealthySource1BDesc); }
         set
         {
            SetValue(() => WealthySource1BDesc, value);
         }
      }

      public bool WealthySource2
      {
         get { return GetValue(() => WealthySource2); }
         set
         {
            if (value)
            {
               if (SelectedWealthySources.Count < 3)
               {
                  SetValue(() => WealthySource2, value);
                  SelectedWealthySources.Add(0x02);
               }
               else
               {
                  SetValue(() => WealthySource2, !value);
               }
            }
            else
            {
               SetValue(() => WealthySource2, value);

               if (SelectedWealthySources.Contains(0x02))
                  SelectedWealthySources.Remove(0x02);

               WealthySource2A = false;
               WealthySource2B = false;
               WealthySource2C = false;
               WealthySource2C = false;
               WealthySource2D = false;
               WealthySource2E = false;
               WealthySource2EDesc = "";
            }
         }
      }

      public bool WealthySource2A
      {
         get { return GetValue(() => WealthySource2A); }
         set
         {
            SetValue(() => WealthySource2A, value);

            if(value)
            {
               WealthySource2B = false;
               WealthySource2C = false;
               WealthySource2D = false;
               WealthySource2E = false;
            }
         }
      }

      public bool WealthySource2B
      {
         get { return GetValue(() => WealthySource2B); }
         set
         {
            SetValue(() => WealthySource2B, value);

            if (value)
            {
               WealthySource2A = false;
               WealthySource2C = false;
               WealthySource2D = false;
               WealthySource2E = false;
            }
         }
      }

      public bool WealthySource2C
      {
         get { return GetValue(() => WealthySource2C); }
         set
         {
            SetValue(() => WealthySource2C, value);

            if (value)
            {
               WealthySource2A = false;
               WealthySource2B = false;
               WealthySource2D = false;
               WealthySource2E = false;
            }
         }
      }

      public bool WealthySource2D
      {
         get { return GetValue(() => WealthySource2D); }
         set
         {
            SetValue(() => WealthySource2D, value);

            if (value)
            {
               WealthySource2A = false;
               WealthySource2B = false;
               WealthySource2C = false;
               WealthySource2E = false;
            }
         }
      }

      public bool WealthySource2E
      {
         get { return GetValue(() => WealthySource2E); }
         set
         {
            SetValue(() => WealthySource2E, value);

            if (value)
            {
               WealthySource2A = false;
               WealthySource2B = false;
               WealthySource2C = false;
               WealthySource2D = false;
            }
            else
               WealthySource2EDesc = "";
         }
      }

      public string WealthySource2EDesc
      {
         get { return GetValue(() => WealthySource2EDesc); }
         set
         {
            SetValue(() => WealthySource2EDesc, value);
         }
      }

      public bool WealthySource3
      {
         get { return GetValue(() => WealthySource3); }
         set
         {
            if (value)
            {
               if (SelectedWealthySources.Count < 3)
               {
                  SetValue(() => WealthySource3, value);
                  SelectedWealthySources.Add(0x03);
               }
               else
               {
                  SetValue(() => WealthySource3, !value);
               }
            }
            else
            {
               SetValue(() => WealthySource3, value);

               if (SelectedWealthySources.Contains(0x03))
                  SelectedWealthySources.Remove(0x03);

               WealthySource3A = false;
               WealthySource3B = false;
               WealthySource3C = false;
               WealthySource3D = false;
               WealthySource3E = false;
            }
         }
      }

      public bool WealthySource3A
      {
         get { return GetValue(() => WealthySource3A); }
         set
         {
            SetValue(() => WealthySource3A, value);

            if(value)
            {
               WealthySource3B = false;
               WealthySource3C = false;
               WealthySource3D = false;
               WealthySource3E = false;
            }
         }
      }

      public bool WealthySource3B
      {
         get { return GetValue(() => WealthySource3B); }
         set
         {
            SetValue(() => WealthySource3B, value);

            if (value)
            {
               WealthySource3A = false;
               WealthySource3C = false;
               WealthySource3D = false;
               WealthySource3E = false;
            }
         }
      }

      public bool WealthySource3C
      {
         get { return GetValue(() => WealthySource3C); }
         set
         {
            SetValue(() => WealthySource3C, value);

            if (value)
            {
               WealthySource3A = false;
               WealthySource3B = false;
               WealthySource3D = false;
               WealthySource3E = false;
            }
         }
      }

      public bool WealthySource3D
      {
         get { return GetValue(() => WealthySource3D); }
         set
         {
            SetValue(() => WealthySource3D, value);

            if (value)
            {
               WealthySource3A = false;
               WealthySource3B = false;
               WealthySource3C = false;
               WealthySource3E = false;
            }
         }
      }

      public bool WealthySource3E
      {
         get { return GetValue(() => WealthySource3E); }
         set
         {
            SetValue(() => WealthySource3E, value);

            if (value)
            {
               WealthySource3A = false;
               WealthySource3B = false;
               WealthySource3C = false;
               WealthySource3D = false;
            }
         }
      }

      public bool WealthySource4
      {
         get { return GetValue(() => WealthySource4); }
         set
         {
            if (value)
            {
               if (SelectedWealthySources.Count < 3)
               {
                  SetValue(() => WealthySource4, value);
                  SelectedWealthySources.Add(0x04);
               }
               else
               {
                  SetValue(() => WealthySource4, !value);
               }
            }
            else
            {
               SetValue(() => WealthySource4, value);

               if (SelectedWealthySources.Contains(0x04))
                  SelectedWealthySources.Remove(0x04);
            }
         }
      }

      public bool WealthySource5
      {
         get { return GetValue(() => WealthySource5); }
         set
         {
            if (value)
            {
               if (SelectedWealthySources.Count < 3)
               {
                  SetValue(() => WealthySource5, value);
                  SelectedWealthySources.Add(0x05);
               }
               else
               {
                  SetValue(() => WealthySource5, !value);
               }
            }
            else
            {
               SetValue(() => WealthySource5, value);

               if (SelectedWealthySources.Contains(0x05))
                  SelectedWealthySources.Remove(0x05);
            }
         }
      }

      public bool WealthySource6
      {
         get { return GetValue(() => WealthySource6); }
         set
         {
            if (value)
            {
               if (SelectedWealthySources.Count < 3)
               {
                  SetValue(() => WealthySource6, value);
                  SelectedWealthySources.Add(0x06);
               }
               else
               {
                  SetValue(() => WealthySource6, !value);
               }
            }
            else
            {
               SetValue(() => WealthySource6, value);

               if (SelectedWealthySources.Contains(0x06))
                  SelectedWealthySources.Remove(0x06);

               WealthySource6Desc = "";
            }
         }
      }

      public string WealthySource6Desc
      {
         get { return GetValue(() => WealthySource6Desc); }
         set
         {
            SetValue(() => WealthySource6Desc, value);
         }
      }

      public bool StampRefNumber
      {
         get { return GetValue(() => StampRefNumber); }
         set
         {
            SetValue(() => StampRefNumber, value);

         }
      }

      public string StampRefNumberDesc
      {
         get { return GetValue(() => StampRefNumberDesc); }
         set
         {
            SetValue(() => StampRefNumberDesc, value);
         }
      }

      public bool StampRefNumberEnabled
      {
         get { return GetValue(() => StampRefNumberEnabled); }
         set
         {
            SetValue(() => StampRefNumberEnabled, value);
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

      public string AccountType
      {
         get { return GetValue(() => AccountType); }
         set
         {
            SetValue(() => AccountType, value);
         }
      }

      public bool IsShowStampMsg
      {
         get { return GetValue(() => IsShowStampMsg); }
         set
         {
            SetValue(() => IsShowStampMsg, value);
         }
      }

      public string BranchNo
      {
         get { return GetValue(() => BranchNo); }
         set
         {
            SetValue(() => BranchNo, value);
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
         SelectedWealthySources = new List<int>();
         WealthySource1 = false;
         WealthySource1A = false;
         WealthySource1B = false;
         WealthySource1BDesc = "";
         WealthySource2 = false;
         WealthySource2A = false;
         WealthySource2B = false;
         WealthySource2C = false;
         WealthySource2D = false;
         WealthySource2E = false;
         WealthySource2EDesc = "";
         WealthySource3 = false;
         WealthySource3A = false;
         WealthySource3B = false;
         WealthySource3C = false;
         WealthySource3D = false;
         WealthySource3E = false;
         WealthySource4 = false;
         WealthySource5 = false;
         WealthySource6 = false;
         WealthySource6Desc = "";
         StampRefNumber = false;
         StampRefNumberDesc = "";
         StampRefNumberEnabled = false;
         HasOldAccount = false;
         AccountType = "1";                          
         IsShowStampMsg = false;
         BranchNo = "";
         IsPreProcAccount = false;
      }
   }
}
