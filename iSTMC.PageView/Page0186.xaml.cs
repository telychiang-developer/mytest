using iSTMC.Common;
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
using Xceed.Wpf.Toolkit;

namespace iSTMC.PageView
{
   /// <summary>
   /// Page0186.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0186", Description = "資料填寫-申請項目-填寫申請項目")]
   public partial class Page0186 : Page
   {
      private Page0186ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0186(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0186ViewModel(kernelService, "Page0186");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0186_Loaded;
      }

      private void Page0186_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData186))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData186].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.AccountType = jsonObj["AccountType"];
            _pageVM.Email = jsonObj["Email"];
            _pageVM.HasOldAccount = jsonObj["HasOldAccount"] == "Y";
            _pageVM.ApplyEBank = jsonObj["ApplyEBank"] == "Y";
            _pageVM.ApplyEBankEnabled = jsonObj["ApplyEBankEnabled"] == "Y";
            _pageVM.ApplySSL = jsonObj["ApplySSL"] == "Y";
            _pageVM.TRFSameNameAccount = jsonObj["TRFSameNameAccount"] == "Y";
            _pageVM.EBankNonDealTRF = jsonObj["EBankNonDealTRF"] == "Y";
            _pageVM.ExchangeClaimService = jsonObj["ExchangeClaimService"] == "Y";
            _pageVM.OnlineTRFAccount = jsonObj["OnlineTRFAccount"] == "Y";
            _pageVM.MobileBank = jsonObj["MobileBank"] == "Y";
            _pageVM.ApplyEMobileService = jsonObj["ApplyEMobileService"] == "Y";
            _pageVM.ApplyEMobileServiceEnabled = jsonObj["ApplyEMobileServiceEnabled"] == "Y";
            _pageVM.OldAccountEBankApplied = jsonObj["OldAccountEBankApplied"] == "Y";
            _pageVM.OldAccountEMobileCodeApplied = jsonObj["OldAccountEMobileCodeApplied"] == "Y";
            _pageVM.eMobileReserved = jsonObj["eMobileReserved"];
            if (string.IsNullOrEmpty(_pageVM.eMobileReserved))
               _pageVM.eMobile = jsonObj["eMobile"];
            else
               _pageVM.eMobile = _pageVM.eMobileReserved;

            _pageVM.WarnningAccount = jsonObj["WarnningAccount"] == "Y";
            _pageVM.IsShowWarnningMsg = jsonObj["IsShowWarnningMsg"] == "Y";
            _pageVM.WarnningMsg = jsonObj["WarnningMsg"];
            _pageVM.eMobileEnabled = jsonObj["eMobileEnabled"] == "Y";

            if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData170))
            {
               string json170 = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData170].ToString();
               var jsonObj170 = JsonConvert.DeserializeObject<Dictionary<string, string>>(json170);

               _pageVM.Mobile = jsonObj170["MobileNumber"];

               string mobile = _pageVM.eMobile.Replace("-", "").Trim();
               if (string.IsNullOrEmpty(mobile) && string.IsNullOrEmpty(_pageVM.eMobileReserved))
                  _pageVM.eMobile = _pageVM.Mobile;

               if(string.IsNullOrEmpty(_pageVM.Email))
                  _pageVM.Email = jsonObj170["Email"];

               if (_pageVM.IsShowWarnningMsg || _pageVM.OldAccountEMobileCodeApplied)
                  _pageVM.eMobile = "";
            }
            else
               _kernelService.Logger.Warn("PersonalData170 is not found!");

            if (!_pageVM.eMobileEnabled || !_pageVM.ApplyEMobileService)
               _pageVM.eMobile = "";
         }
         else
         {
            if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData170))
            {
               string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData170].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               _pageVM.Mobile = jsonObj["MobileNumber"];
               _pageVM.eMobile = _pageVM.Mobile;
               _pageVM.Email = jsonObj["Email"];
            }
            else
               _kernelService.Logger.Warn("PersonalData170 is not found!");

            if (_kernelService.TransactionDataCache.ContainsKey("__WARN_ACCOUNT_DATA"))
            {
               string json = _kernelService.TransactionDataCache["__WARN_ACCOUNT_DATA"].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               if (jsonObj.ContainsKey("oldAccResult"))
               {
                  _pageVM.HasOldAccount = _pageVM.HasOldAccount = jsonObj["oldAccResult"] == "Y" || jsonObj["oldAccResult"] == "P";
                  _pageVM.IsPreProcAccount = jsonObj["oldAccResult"] == "P";

                  if (_pageVM.HasOldAccount && _kernelService.TransactionDataCache.ContainsKey("__OLD_ACCOUNT_DATA"))
                  {
                     string oldAccountData = _kernelService.TransactionDataCache["__OLD_ACCOUNT_DATA"].ToString();
                     var oaData = JsonConvert.DeserializeObject<Dictionary<string, string>>(oldAccountData);

                     if (oaData.ContainsKey("ynApplyWebBank") && oaData["ynApplyWebBank"] == "Y")
                     {
                        _pageVM.ApplyEBankEnabled = false; 
                        _pageVM.OldAccountEBankApplied = true;
                        _pageVM.eMobile = "";
                     }
                     else
                     {
                        _pageVM.ApplyEBankEnabled = true;
                        _pageVM.OldAccountEBankApplied = false;
                     }

                     if (oaData.ContainsKey("ynApplyECode") && oaData["ynApplyECode"] == "Y")
                     {
                        _pageVM.OldAccountEMobileCodeApplied = true;

                        if (_pageVM.ApplyEBankEnabled)
                        {
                           _pageVM.ApplyEMobileService = true;
                           _pageVM.ApplyEMobileServiceEnabled = false;
                           _pageVM.eMobile = "";
                        }
                     }
                     else
                     {
                        _pageVM.OldAccountEMobileCodeApplied = false;

                        if (_pageVM.ApplyEBankEnabled)
                        {
                           _pageVM.ApplyEMobileService = false;
                           _pageVM.ApplyEMobileServiceEnabled = false;
                        }
                     }
                  }
                  else
                  {
                     if (_pageVM.HasOldAccount)
                        _kernelService.Logger.Warn("__OLD_ACCOUNT_DATA is not found!");
                  }
               }
               else
                  _pageVM.HasOldAccount = false;

               if (jsonObj.ContainsKey("warnResultS"))
               {
                  string val = jsonObj["warnResultS"];

                  _pageVM.WarnningAccount = jsonObj["warnResultS"] == "Y";

                  if (_pageVM.ApplyEBankEnabled)
                     _pageVM.ApplyEBankEnabled = val == "N";

                  if(_pageVM.WarnningAccount)
                     _pageVM.eMobile = "";
               }
            }
            else
            {
               _pageVM.ApplyEBankEnabled = true;
               _kernelService.Logger.Warn("__WARN_ACCOUNT_DATA is not found!");
            }

            if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.NewAccountType))
            {
               string json = _kernelService.TransactionDataCache[KioskDataCacheKey.NewAccountType].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               _pageVM.AccountType = jsonObj["accType"];

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
            ApplyEBank = _pageVM.ApplyEBank ? "Y" : "N",                       
            ApplyEBankEnabled = _pageVM.ApplyEBankEnabled ? "Y" : "N",         
            ApplySSL = _pageVM.ApplySSL ? "Y" : "N",                           
            TRFSameNameAccount = _pageVM.TRFSameNameAccount ? "Y" : "N",       
            EBankNonDealTRF = _pageVM.EBankNonDealTRF ? "Y" : "N",             
            ExchangeClaimService = _pageVM.ExchangeClaimService ? "Y" : "N",   
            OnlineTRFAccount = _pageVM.OnlineTRFAccount ? "Y" : "N",           
            MobileBank = _pageVM.MobileBank ? "Y" : "N",                       
            ApplyEMobileService = _pageVM.ApplyEMobileService ? "Y" : "N",      
            ApplyEMobileServiceEnabled = _pageVM.ApplyEMobileServiceEnabled ? "Y" : "N",
            _pageVM.eMobile,                                                   
            _pageVM.Mobile,
            _pageVM.eMobileReserved,
            OldAccountEBankApplied = _pageVM.OldAccountEBankApplied ? "Y" : "N",
            OldAccountEMobileCodeApplied = _pageVM.OldAccountEMobileCodeApplied ? "Y" : "N",
            _pageVM.Email,
            HasOldAccount = _pageVM.HasOldAccount ? "Y" : "N",
            IsPreProcAccount = _pageVM.IsPreProcAccount ? "Y" : "N",
            _pageVM.AccountType,
            WarnningAccount = _pageVM.WarnningAccount ? "Y" : "N",
            IsShowWarnningMsg = _pageVM.IsShowWarnningMsg ? "Y" : "N",
            _pageVM.WarnningMsg,
            eMobileEnabled = _pageVM.eMobileEnabled ? "Y" : "N",
         };

         string json = JsonConvert.SerializeObject(model);
         _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData186, json);

         _pageVM.Clear();

         PageResult result = new PageResult("Previous", KioskDataCacheKey.PersonalData186);
         _kernelService.NextPage(result);
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         List<string> errorList = new List<string>();
         PageViewUtils.GetErrors(errorList, grdForm);

         string mobile = _pageVM.Mobile.Replace("-", "").Trim();
         if (_pageVM.ApplyEBank && string.IsNullOrEmpty(mobile))
            errorList.Add("勾選申請網路銀行時個人行動電話必須填寫，請回前頁重新確認");
         else
         {
            string eMobile = _pageVM.eMobile.Replace("-", "").Trim();
            if (_pageVM.ApplyEMobileService && string.IsNullOrEmpty(eMobile) && !_pageVM.OldAccountEMobileCodeApplied)
               errorList.Add("勾選行動e碼服務行動電話必須填寫");

            if (_pageVM.EBankNonDealTRF && !_pageVM.ApplyEMobileService)
               errorList.Add("勾選新臺幣非約定轉帳時行動e碼服務必須勾選");

            if (_pageVM.EBankNonDealTRF && !_pageVM.ApplySSL)
               errorList.Add("勾選新臺幣非約定轉帳時SSL機制必須勾選");

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

            List<string> applyEBanks = new List<string>();
            if (_pageVM.ApplySSL) applyEBanks.Add("SSL機制");
            if (_pageVM.TRFSameNameAccount) applyEBanks.Add("轉入本人本行新臺幣帳戶");
            if (_pageVM.EBankNonDealTRF) applyEBanks.Add("新臺幣非約定轉帳");
            if (_pageVM.ExchangeClaimService) applyEBanks.Add("網路外匯申報服務");
            if (_pageVM.OnlineTRFAccount) applyEBanks.Add("新臺幣線上設定轉入帳號");
            if (_pageVM.MobileBank) applyEBanks.Add("行動銀行服務");
            if (_pageVM.ApplyEMobileService) applyEBanks.Add("行動e碼");

            string applyEBankDesc = string.Join("、", applyEBanks.ToArray());

            var model = new
            {
               ApplyEBank = _pageVM.ApplyEBank ? "Y" : "N",                       
               ApplyEBankEnabled = _pageVM.ApplyEBankEnabled ? "Y" : "N",         
               ApplySSL = _pageVM.ApplySSL ? "Y" : "N",                           
               TRFSameNameAccount = _pageVM.TRFSameNameAccount ? "Y" : "N",       
               EBankNonDealTRF = _pageVM.EBankNonDealTRF ? "Y" : "N",             
               ExchangeClaimService = _pageVM.ExchangeClaimService ? "Y" : "N",   
               OnlineTRFAccount = _pageVM.OnlineTRFAccount ? "Y" : "N",           
               MobileBank = _pageVM.MobileBank ? "Y" : "N",                       
               ApplyEMobileService = _pageVM.ApplyEMobileService ? "Y" : "N",     
               ApplyEMobileServiceEnabled = _pageVM.ApplyEMobileServiceEnabled ? "Y" : "N",
               _pageVM.Mobile,                                                    
               _pageVM.eMobile,                                                   
               _pageVM.eMobileReserved,
               OldAccountEBankApplied = _pageVM.OldAccountEBankApplied ? "Y" : "N",
               OldAccountEMobileCodeApplied = _pageVM.OldAccountEMobileCodeApplied ? "Y" : "N",
               _pageVM.Email,
               HasOldAccount = _pageVM.HasOldAccount ? "Y" : "N",
               IsPreProcAccount = _pageVM.IsPreProcAccount ? "Y" : "N",
               _pageVM.AccountType,
               WarnningAccount = _pageVM.WarnningAccount ? "Y" : "N",
               IsShowWarnningMsg = _pageVM.IsShowWarnningMsg ? "Y" : "N",
               _pageVM.WarnningMsg,
               eMobileEnabled = _pageVM.eMobileEnabled ? "Y" : "N",

               ApplyEBankDesc = _pageVM.ApplyEBank ? "是；" + applyEBankDesc : "否"
            };

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData186, json);

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.PersonalData186);
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

   public class Page0186ViewModel : PageViewModel
   {
      public Page0186ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         Clear();
      }

      public bool ApplyEBank
      {
         get { return GetValue(() => ApplyEBank); }
         set
         {
            SetValue(() => ApplyEBank, value);
            ApplySSLEnabled = value;
            TRFSameNameAccountEnabled = value & HasOldAccount;
            // 檢核帳戶類別是否只勾選「外幣帳戶」
            if (string.Compare(AccountType, "2", true) == 0)
            {
               TRFSameNameAccountEnabled = false;
            }
            EBankNonDealTRFEnabled = value;
            ExchangeClaimServiceEnabled = value;
            OnlineTRFAccountEnabled = value;
            MobileBankEnabled = value;
            if (OldAccountEMobileCodeApplied)
               ApplyEMobileServiceEnabled = false;
            else
               ApplyEMobileServiceEnabled = value;

            if(!value)
            {
               ApplySSL = false;
               TRFSameNameAccount = false;
               EBankNonDealTRF = false;
               ExchangeClaimService = false;
               OnlineTRFAccount = false;
               MobileBank = false;
               ApplyEMobileServiceEnabled = false;

               if (OldAccountEMobileCodeApplied)
                  ApplyEMobileService = true;
               else
                  ApplyEMobileService = false;
            }
         }
      }

      public bool ApplyEBankEnabled
      {
         get { return GetValue(() => ApplyEBankEnabled); }
         set
         {
            SetValue(() => ApplyEBankEnabled, value);

            if(!value)
            {
               ApplySSL = false;
               ApplySSLEnabled = false;
               TRFSameNameAccount = false;
               TRFSameNameAccountEnabled = false;
               ExchangeClaimService = false;
               ExchangeClaimServiceEnabled = false;
               OnlineTRFAccount = false;
               OnlineTRFAccountEnabled = false;
               MobileBank = false;
               MobileBankEnabled = false;
               ApplyEMobileService = false;
               ApplyEMobileServiceEnabled = false;
            }
         }
      }

      public bool ApplySSL
      {
         get { return GetValue(() => ApplySSL); }
         set
         {
            SetValue(() => ApplySSL, value);
         }
      }

      public bool ApplySSLEnabled
      {
         get { return GetValue(() => ApplySSLEnabled); }
         set
         {
            SetValue(() => ApplySSLEnabled, value);
         }
      }

      public bool TRFSameNameAccount
      {
         get { return GetValue(() => TRFSameNameAccount); }
         set
         {
            SetValue(() => TRFSameNameAccount, value);
         }
      }

      public bool TRFSameNameAccountEnabled
      {
         get { return GetValue(() => TRFSameNameAccountEnabled); }
         set
         {
            SetValue(() => TRFSameNameAccountEnabled, value);
         }
      }

      public bool EBankNonDealTRF
      {
         get { return GetValue(() => EBankNonDealTRF); }
         set
         {
            SetValue(() => EBankNonDealTRF, value);

            if (value)
            {
               if (!OldAccountEMobileCodeApplied)
               {
                  ApplyEMobileService = true;
                  ApplyEMobileServiceEnabled = false;
               }

               ApplySSL = true;
               ApplySSLEnabled = false;
            }
            else
            {
               if (!OldAccountEMobileCodeApplied)
                  ApplyEMobileServiceEnabled = true;

               if (ApplyEBank)
                  ApplySSLEnabled = true;
            }
         }
      }

      public bool EBankNonDealTRFEnabled
      {
         get { return GetValue(() => EBankNonDealTRFEnabled); }
         set
         {
            SetValue(() => EBankNonDealTRFEnabled, value);
         }
      }

      public bool ExchangeClaimService
      {
         get { return GetValue(() => ExchangeClaimService); }
         set
         {
            SetValue(() => ExchangeClaimService, value);
         }
      }

      public bool ExchangeClaimServiceEnabled
      {
         get { return GetValue(() => ExchangeClaimServiceEnabled); }
         set
         {
            SetValue(() => ExchangeClaimServiceEnabled, value);
         }
      }

      public bool OnlineTRFAccount
      {
         get { return GetValue(() => OnlineTRFAccount); }
         set
         {
            SetValue(() => OnlineTRFAccount, value);
         }
      }

      public bool OnlineTRFAccountEnabled
      {
         get { return GetValue(() => OnlineTRFAccountEnabled); }
         set
         {
            SetValue(() => OnlineTRFAccountEnabled, value);
         }
      }

      public bool MobileBank
      {
         get { return GetValue(() => MobileBank); }
         set
         {
            SetValue(() => MobileBank, value);
         }
      }

      public bool MobileBankEnabled
      {
         get { return GetValue(() => MobileBankEnabled); }
         set
         {
            SetValue(() => MobileBankEnabled, value);
         }
      }

      public bool ApplyEMobileService
      {
         get { return GetValue(() => ApplyEMobileService); }
         set
         {
            SetValue(() => ApplyEMobileService, value);
            eMobileReadonly = !value;

            if(value)
            {
               if (HasOldAccount && (OldAccountEBankApplied || OldAccountEMobileCodeApplied))
                  eMobileEnabled = false;
               else
               {
                  eMobileEnabled = true;

                  eMobile = eMobileReserved;
               }
            }
            else
            {
               eMobile = "";
            }
         }
      }

      public bool ApplyEMobileServiceEnabled
      {
         get { return GetValue(() => ApplyEMobileServiceEnabled); }
         set
         {
            SetValue(() => ApplyEMobileServiceEnabled, value);
         }
      }

      public string Mobile
      {
         get { return GetValue(() => Mobile); }
         set
         {
            SetValue(() => Mobile, value);
         }
      }

      public string eMobile
      {
         get { return GetValue(() => eMobile); }
         set
         {
            SetValue(() => eMobile, value);

            if(!string.IsNullOrEmpty(value))
               eMobileReserved = value;
         }
      }

      public string eMobileReserved
      {
         get { return GetValue(() => eMobileReserved); }
         set
         {
            SetValue(() => eMobileReserved, value);
         }
      }

      public bool eMobileReadonly
      {
         get { return GetValue(() => eMobileReadonly); }
         set
         {
            SetValue(() => eMobileReadonly, value);
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

      public bool OldAccountEBankApplied
      {
         get { return GetValue(() => OldAccountEBankApplied); }
         set
         {
            SetValue(() => OldAccountEBankApplied, value);

            if(HasOldAccount & value)
               eMobileEnabled = false;
         }
      }

      public bool OldAccountEMobileCodeApplied
      {
         get { return GetValue(() => OldAccountEMobileCodeApplied); }
         set
         {
            SetValue(() => OldAccountEMobileCodeApplied, value);

            if(HasOldAccount & value)
               eMobileEnabled = false;
         }
      }

      public string Email
      {
         get { return GetValue(() => Email); }
         set
         {
            SetValue(() => Email, value);
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

      public bool WarnningAccount
      {
         get { return GetValue(() => WarnningAccount); }
         set
         {
            SetValue(() => WarnningAccount, value);

            if (value)
            {
               eMobileEnabled = false;

               IsShowWarnningMsg = value;
               WarnningMsg = value ? "*您無法申請本行網路銀行服務" : "";
            }
            else
            {
               if(OldAccountEBankApplied)
               {
                  eMobileEnabled = false;

                  IsShowWarnningMsg = true;
                  WarnningMsg = "*已申請本行網路銀行服務";
               }
               else if(OldAccountEMobileCodeApplied)
               {
                  eMobileEnabled = false;
               }
               if(!OldAccountEBankApplied && OldAccountEMobileCodeApplied)
               {
                  eMobileEnabled = false;

                  IsShowWarnningMsg = true;
                  WarnningMsg = "*已申請本行行動e碼服務";
               }
            }
         }
      }

      public bool IsShowWarnningMsg
      {
         get { return GetValue(() => IsShowWarnningMsg); }
         set
         {
            SetValue(() => IsShowWarnningMsg, value);
         }
      }

      public string WarnningMsg
      {
         get { return GetValue(() => WarnningMsg); }
         set
         {
            SetValue(() => WarnningMsg, value);
         }
      }

      public bool eMobileEnabled
      {
         get { return GetValue(() => eMobileEnabled); }
         set
         {
            SetValue(() => eMobileEnabled, value);
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
         ApplyEBank = false;                  
         ApplyEBankEnabled = true;            
         ApplySSL = false;                    
         TRFSameNameAccount = false;          
         EBankNonDealTRF = false;             
         ExchangeClaimService = false;        
         OnlineTRFAccount = false;            
         MobileBank = false;                  
         ApplyEMobileService = false;         
         Mobile = "";                         
         ApplySSLEnabled = false;             
         TRFSameNameAccountEnabled = false;   
         EBankNonDealTRFEnabled = false;      
         ExchangeClaimServiceEnabled = false; 
         OnlineTRFAccountEnabled = false;     
         MobileBankEnabled = false;           
         ApplyEMobileServiceEnabled = false;  
         eMobileReadonly = false;             
         HasOldAccount = false;               
         OldAccountEBankApplied = false;      
         OldAccountEMobileCodeApplied = false;  
         Email = "";                          
         AccountType = "1";                   
         WarnningAccount = false;             
         IsShowWarnningMsg = false;            
         WarnningMsg = "";                    
         eMobileEnabled = false;              
         eMobileReserved = "";
         IsPreProcAccount = false;
      }
   }
}