using iSTMC.Common;
using iSTMC.Kernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace iSTMC.PageView
{
   /// <summary>
   /// Page0185.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0185", Description = "資料填寫-申請項目-填寫申請項目")]
   public partial class Page0185 : Page
   {
      private Page0185ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0185(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0185ViewModel(kernelService, "Page0185");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0185_Loaded;
      }

      private void Page0185_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData185))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData185].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.AccountType = jsonObj["AccountType"];
            _pageVM.HasOldAccount = jsonObj["HasOldAccount"] == "Y";

            _pageVM.ApplyATMCard = jsonObj["ApplyATMCard"] == "Y";
            _pageVM.ApplyATMCardEnabled = jsonObj["ApplyATMCardEnabled"] == "Y";
            _pageVM.ICCATMCard = jsonObj["ICCATMCard"] == "Y";
            _pageVM.NonDealTRF = jsonObj["NonDealTRF"] == "Y";
            _pageVM.VISAAtomCard = jsonObj["VISAAtomCard"] == "Y";
            _pageVM.ConsumeDeduct = jsonObj["ConsumeDeduct"] == "Y";
            _pageVM.VISALotusCard = jsonObj["VISALotusCard"] == "Y";
            _pageVM.InternationTrade = jsonObj["InternationTrade"] == "Y";
            _pageVM.ApplyUniPayment = jsonObj["ApplyUniPayment"] == "Y";
            _pageVM.ApplyTWDAccountPswd = jsonObj["ApplyTWDAccountPswd"] == "Y";
            _pageVM.ApplyFGDAccountPswd = jsonObj["ApplyFGDAccountPswd"] == "Y";
            _pageVM.ICCATMCardEnabled = jsonObj["ICCATMCardEnabled"] == "Y";
            _pageVM.NonDealTRFEnabled = jsonObj["NonDealTRFEnabled"] == "Y";
            _pageVM.VISAAtomCardEnabled = jsonObj["VISAAtomCardEnabled"] == "Y";
            _pageVM.ConsumeDeductEnabled = jsonObj["ConsumeDeductEnabled"] == "Y";
            _pageVM.VISALotusCardEnabled = jsonObj["VISALotusCardEnabled"] == "Y";
            _pageVM.InternationTradeEnabled = jsonObj["InternationTradeEnabled"] == "Y";
            _pageVM.Email = jsonObj["Email"];
            _pageVM.WarnningAccount = jsonObj["WarnningAccount"] == "Y";
            _pageVM.IsShowWarnningMsg = jsonObj["IsShowWarnningMsg"] == "Y";
            _pageVM.WarnningMsg = jsonObj["WarnningMsg"];
         }
         else
         {
            if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData170))
            {
               string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData170].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

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
                  _pageVM.HasOldAccount = jsonObj["oldAccResult"] == "Y" || jsonObj["oldAccResult"] == "P";
                  _pageVM.IsPreProcAccount = jsonObj["oldAccResult"] == "P";
               }

               if (jsonObj.ContainsKey("warnResultS"))
               {
                  string val = jsonObj["warnResultS"];

                  _pageVM.WarnningAccount = jsonObj["warnResultS"] == "Y";

                  _pageVM.ApplyATMCardEnabled = val == "N";
               }
               else
               {
                  _pageVM.ApplyATMCardEnabled = true;
               }
            }
            else
            {
               _pageVM.ApplyATMCardEnabled = true;
               _kernelService.Logger.Warn("__WARN_ACCOUNT_DATA is not found!");
            }

            if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.NewAccountType))
            {
               string json = _kernelService.TransactionDataCache[KioskDataCacheKey.NewAccountType].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               _pageVM.AccountType = jsonObj["accType"];

               if(_pageVM.AccountType == "1")
               {
                  _pageVM.ApplyFGDAccountPswd = false;
                  _pageVM.ApplyFGDAccountPswdEnabled = false;
               }
               else if(_pageVM.AccountType == "2")  
               {
                  _pageVM.ApplyATMCard = false;
                  _pageVM.ApplyATMCardEnabled = false;
                  _pageVM.ICCATMCard = false;
                  _pageVM.VISAAtomCard = false;
                  _pageVM.VISALotusCard = false;
                  _pageVM.ApplyTWDAccountPswd = false;
                  _pageVM.ApplyTWDAccountPswdEnabled = false;
                  _pageVM.IsShowWarnningMsg = true;
                  _pageVM.WarnningMsg = "*您無法申請本行金融卡";
               }
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
            ApplyATMCard = _pageVM.ApplyATMCard ? "Y" : "N",                   
            ApplyATMCardEnabled = _pageVM.ApplyATMCardEnabled ? "Y" : "N",      
            ICCATMCard = _pageVM.ICCATMCard ? "Y" : "N",                       
            NonDealTRF = _pageVM.NonDealTRF ? "Y" : "N",                       
            VISAAtomCard = _pageVM.VISAAtomCard ? "Y" : "N",                   
            ConsumeDeduct = _pageVM.ConsumeDeduct ? "Y" : "N",                 
            VISALotusCard = _pageVM.VISALotusCard ? "Y" : "N",                 
            InternationTrade = _pageVM.InternationTrade ? "Y" : "N",           
            ApplyUniPayment = _pageVM.ApplyUniPayment ? "Y" : "N",             
            ApplyTWDAccountPswd = _pageVM.ApplyTWDAccountPswd ? "Y" : "N",     
            ApplyFGDAccountPswd = _pageVM.ApplyFGDAccountPswd ? "Y" : "N",     
            ICCATMCardEnabled = _pageVM.ICCATMCardEnabled ? "Y" : "N",
            NonDealTRFEnabled = _pageVM.NonDealTRFEnabled ? "Y" : "N",
            VISAAtomCardEnabled = _pageVM.VISAAtomCardEnabled ? "Y" : "N",
            ConsumeDeductEnabled = _pageVM.ConsumeDeductEnabled ? "Y" : "N",
            VISALotusCardEnabled = _pageVM.VISALotusCardEnabled ? "Y" : "N",
            InternationTradeEnabled = _pageVM.InternationTradeEnabled ? "Y" : "N",
            _pageVM.AccountType,
            HasOldAccount = _pageVM.HasOldAccount ? "Y" : "N",
            IsPreProcAccount = _pageVM.IsPreProcAccount ? "Y" : "N",
            _pageVM.Email,
            WarnningAccount = _pageVM.WarnningAccount ? "Y" : "N",
            IsShowWarnningMsg = _pageVM.IsShowWarnningMsg ? "Y" : "N",
            _pageVM.WarnningMsg
         };

         string json = JsonConvert.SerializeObject(model);
         _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData185, json);

         _pageVM.Clear();

         PageResult result = new PageResult("Previous", KioskDataCacheKey.PersonalData185);
         _kernelService.NextPage(result);
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         List<string> errorList = new List<string>();
         PageViewUtils.GetErrors(errorList, grdForm);

         if (_pageVM.ApplyATMCard)
         {
            if (!_pageVM.ICCATMCard && !_pageVM.VISAAtomCard && !_pageVM.VISALotusCard)
               errorList.Add("勾選申請申請金融卡時必須選定一種金融卡片");            
         }         

         if (_pageVM.ApplyUniPayment)
         {
            if(_pageVM.AccountType == "1" && !_pageVM.ApplyTWDAccountPswd)
               errorList.Add("勾選申請聯行代收付台幣帳戶通提密碼必須勾選");
            else if(_pageVM.AccountType == "2" && !_pageVM.ApplyFGDAccountPswd)
               errorList.Add("勾選申請聯行代收付外幣帳戶通提密碼必須勾選");
            else if(_pageVM.AccountType == "3" && !_pageVM.ApplyTWDAccountPswd && !_pageVM.ApplyFGDAccountPswd)
               errorList.Add("勾選申請聯行代收付臺幣/外幣帳戶通提密碼必須至少勾選一項");
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

            List<string> applyATMCards = new List<string>();
            if (_pageVM.ICCATMCard) applyATMCards.Add("晶片金融卡");
            if (_pageVM.VISAAtomCard) applyATMCards.Add("VISA小金剛卡面");
            if (_pageVM.VISALotusCard) applyATMCards.Add("VISA荷花卡面");
            if (_pageVM.NonDealTRF) applyATMCards.Add("非約定轉帳");
            if (_pageVM.ConsumeDeduct) applyATMCards.Add("國內消費扣款");
            if (_pageVM.InternationTrade) applyATMCards.Add("跨國交易");

            string applyATMCardDesc = string.Join("、", applyATMCards.ToArray());

            var model = new
            {
               ApplyATMCard = _pageVM.ApplyATMCard ? "Y" : "N",                   
               ApplyATMCardEnabled = _pageVM.ApplyATMCardEnabled ? "Y" : "N",      
               ICCATMCard = _pageVM.ICCATMCard ? "Y" : "N",                       
               NonDealTRF = _pageVM.NonDealTRF ? "Y" : "N",                       
               VISAAtomCard = _pageVM.VISAAtomCard ? "Y" : "N",                   
               ConsumeDeduct = _pageVM.ConsumeDeduct ? "Y" : "N",                 
               VISALotusCard = _pageVM.VISALotusCard ? "Y" : "N",                 
               InternationTrade = _pageVM.InternationTrade ? "Y" : "N",           
               ApplyUniPayment = _pageVM.ApplyUniPayment ? "Y" : "N",             
               ApplyTWDAccountPswd = _pageVM.ApplyTWDAccountPswd ? "Y" : "N",     
               ApplyFGDAccountPswd = _pageVM.ApplyFGDAccountPswd ? "Y" : "N",     
               ICCATMCardEnabled = _pageVM.ICCATMCardEnabled ? "Y" : "N",
               NonDealTRFEnabled = _pageVM.NonDealTRFEnabled ? "Y" : "N",
               VISAAtomCardEnabled = _pageVM.VISAAtomCardEnabled ? "Y" : "N",
               ConsumeDeductEnabled = _pageVM.ConsumeDeductEnabled ? "Y" : "N",
               VISALotusCardEnabled = _pageVM.VISALotusCardEnabled ? "Y" : "N",
               InternationTradeEnabled = _pageVM.InternationTradeEnabled ? "Y" : "N",
               _pageVM.AccountType,
               HasOldAccount = _pageVM.HasOldAccount ? "Y" : "N",
               IsPreProcAccount = _pageVM.IsPreProcAccount ? "Y" : "N",
               _pageVM.Email,
               WarnningAccount = _pageVM.WarnningAccount ? "Y" : "N",
               IsShowWarnningMsg = _pageVM.IsShowWarnningMsg ? "Y" : "N",
               _pageVM.WarnningMsg,

               ApplyATMCardDesc = _pageVM.ApplyATMCard ? "是；" + applyATMCardDesc : "否",
               ApplyUniPaymentDesc = _pageVM.ApplyUniPayment ? "是" : "否",
               ApplyTWDAccountPswdDesc = _pageVM.ApplyTWDAccountPswd ? "台幣帳戶" : "",
               ApplyFGDAccountPswdDesc = _pageVM.ApplyFGDAccountPswd ? "外幣帳戶" : ""
            };

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData185, json);

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.PersonalData185);
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

   public class Page0185ViewModel : PageViewModel
   {
      public Page0185ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         Clear();
      }
    
      public bool ApplyATMCard
      {
         get { return GetValue(() => ApplyATMCard); }
         set
         {
            SetValue(() => ApplyATMCard, value);
            ICCATMCardEnabled = value;
            NonDealTRFEnabled = value;
            VISAAtomCardEnabled = value;
            ConsumeDeductEnabled = value;
            VISALotusCardEnabled = value;
            InternationTradeEnabled = value;
            if(!value)
            {
               ICCATMCard = false;
               NonDealTRF = false;
               VISAAtomCard = false;
               ConsumeDeduct = false;
               VISALotusCard = false;
               InternationTrade = false;
            }
         }
      }

      public bool ApplyATMCardEnabled
      {
         get { return GetValue(() => ApplyATMCardEnabled); }
         set
         {
            SetValue(() => ApplyATMCardEnabled, value);
         }
      }

      public bool ICCATMCard
      {
         get { return GetValue(() => ICCATMCard); }
         set
         {
            if (value)
            {
               VISAAtomCard = false;
               VISALotusCard = false;
               InternationTrade = false;
               InternationTradeEnabled = false;
            }

            SetValue(() => ICCATMCard, value);
         }
      }

      public bool ICCATMCardEnabled
      {
         get { return GetValue(() => ICCATMCardEnabled); }
         set
         {
            SetValue(() => ICCATMCardEnabled, value);
         }
      }

      public bool NonDealTRF
      {
         get { return GetValue(() => NonDealTRF); }
         set
         {
            SetValue(() => NonDealTRF, value);
         }
      }

      public bool NonDealTRFEnabled
      {
         get { return GetValue(() => NonDealTRFEnabled); }
         set
         {
            SetValue(() => NonDealTRFEnabled, value);
         }
      }

      public bool VISAAtomCard
      {
         get { return GetValue(() => VISAAtomCard); }
         set
         {
            if (value)
            {
               ICCATMCard = false;
               VISALotusCard = false;
               InternationTradeEnabled = true;
            }

            SetValue(() => VISAAtomCard, value);
         }
      }

      public bool VISAAtomCardEnabled
      {
         get { return GetValue(() => VISAAtomCardEnabled); }
         set
         {
            SetValue(() => VISAAtomCardEnabled, value);
         }
      }

      public bool ConsumeDeduct
      {
         get { return GetValue(() => ConsumeDeduct); }
         set
         {
            SetValue(() => ConsumeDeduct, value);
         }
      }

      public bool ConsumeDeductEnabled
      {
         get { return GetValue(() => ConsumeDeductEnabled); }
         set
         {
            SetValue(() => ConsumeDeductEnabled, value);
         }
      }

      public bool VISALotusCard
      {
         get { return GetValue(() => VISALotusCard); }
         set
         {
            if (value)
            {
               ICCATMCard = false;
               VISAAtomCard = false;
               InternationTradeEnabled = true;
            }

            SetValue(() => VISALotusCard, value);
         }
      }

      public bool VISALotusCardEnabled
      {
         get { return GetValue(() => VISALotusCardEnabled); }
         set
         {
            SetValue(() => VISALotusCardEnabled, value);
         }
      }

      public bool InternationTrade
      {
         get { return GetValue(() => InternationTrade); }
         set
         {
            SetValue(() => InternationTrade, value);
         }
      }

      public bool InternationTradeEnabled
      {
         get { return GetValue(() => InternationTradeEnabled); }
         set
         {
            SetValue(() => InternationTradeEnabled, value);
         }
      }

      public bool ApplyUniPayment
      {
         get { return GetValue(() => ApplyUniPayment); }
         set
         {
            SetValue(() => ApplyUniPayment, value);

            if (value)
            {
               if (AccountType == "1")
               {
                  ApplyTWDAccountPswd = true;
                  ApplyFGDAccountPswd = false;
                  ApplyTWDAccountPswdEnabled = true;
                  ApplyFGDAccountPswdEnabled = false;
               }
               else if (AccountType == "2")
               {
                  ApplyTWDAccountPswd = false;
                  ApplyFGDAccountPswd = true;
                  ApplyTWDAccountPswdEnabled = false;
                  ApplyFGDAccountPswdEnabled = true;
               }
               else if(AccountType == "3")
               {
                  ApplyTWDAccountPswd = true;
                  ApplyFGDAccountPswd = true;
                  ApplyTWDAccountPswdEnabled = true;
                  ApplyFGDAccountPswdEnabled = true;
               }
            }
            else
            {
               ApplyTWDAccountPswd = false;
               ApplyFGDAccountPswd = false;
               ApplyTWDAccountPswdEnabled = false;
               ApplyFGDAccountPswdEnabled = false;
            }
         }
      }

      public bool ApplyTWDAccountPswd
      {
         get { return GetValue(() => ApplyTWDAccountPswd); }
         set
         {
            SetValue(() => ApplyTWDAccountPswd, value);
         }
      }

      public bool ApplyTWDAccountPswdEnabled
      {
         get { return GetValue(() => ApplyTWDAccountPswdEnabled); }
         set
         {
            SetValue(() => ApplyTWDAccountPswdEnabled, value);
         }
      }

      public bool ApplyFGDAccountPswd
      {
         get { return GetValue(() => ApplyFGDAccountPswd); }
         set
         {
            SetValue(() => ApplyFGDAccountPswd, value);
         }
      }

      public bool ApplyFGDAccountPswdEnabled
      {
         get { return GetValue(() => ApplyFGDAccountPswdEnabled); }
         set
         {
            SetValue(() => ApplyFGDAccountPswdEnabled, value);
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

      public bool HasOldAccount
      {
         get { return GetValue(() => HasOldAccount); }
         set
         {
            SetValue(() => HasOldAccount, value);
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

      public bool WarnningAccount
      {
         get { return GetValue(() => WarnningAccount); }
         set
         {
            SetValue(() => WarnningAccount, value);

            if (value)
            {
               IsShowWarnningMsg = true;
               WarnningMsg = value ? "*您無法申請本行金融卡" : "";
            }
            else
            {
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
         ApplyATMCard = false;                
         ApplyATMCardEnabled = true;          
         ICCATMCard = false;                  
         ICCATMCardEnabled = false;           
         NonDealTRF = false;                  
         NonDealTRFEnabled = false;           
         VISAAtomCard = false;                
         VISAAtomCardEnabled = false;         
         ConsumeDeduct = false;               
         ConsumeDeductEnabled = false;        
         VISALotusCard = false;               
         VISALotusCardEnabled = false;        
         InternationTrade = false;            
         InternationTradeEnabled = false;     

         ApplyUniPayment = false;             
         ApplyTWDAccountPswd = false;         
         ApplyFGDAccountPswd = false;         
         AccountType = "1";                          
         HasOldAccount = false;               
         Email = "";                          
         WarnningAccount = false;             
         IsShowWarnningMsg = false;            
         WarnningMsg = "";
         IsPreProcAccount = false;
      }
   }
}