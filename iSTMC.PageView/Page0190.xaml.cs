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
using System.Windows.Controls.Primitives;
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
   /// Page0190.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0190", Description = "資料填寫-確認資料-確認輸入資料")]
   public partial class Page0190 : Page
   {
      private Page0190ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      private Dictionary<string, string> PersonalData170 = new Dictionary<string, string>();
      private Dictionary<string, string> PersonalData172 = new Dictionary<string, string>();
      private Dictionary<string, string> PersonalData175 = new Dictionary<string, string>();
      private Dictionary<string, string> PersonalData177 = new Dictionary<string, string>();
      private Dictionary<string, string> PersonalData185 = new Dictionary<string, string>();
      private Dictionary<string, string> PersonalData186 = new Dictionary<string, string>();
      private Dictionary<string, string> PersonalData187 = new Dictionary<string, string>();

      public Page0190(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0190ViewModel(kernelService, "Page0190");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0190_Loaded;
      }

      private void Page0190_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData170))
         {
            _pageVM.Page0170Data = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData170].ToString();
            PersonalData170 = JsonConvert.DeserializeObject<Dictionary<string, string>>(_pageVM.Page0170Data);

            _pageVM.HouseFullAddress = PersonalData170["HouseFullAddress"];
            _pageVM.CommFullAddress = PersonalData170["CommFullAddress"];
            _pageVM.HomeFullTel = PersonalData170["HomeFullTel"];
            _pageVM.OfficeFullTel = PersonalData170["OfficeFullTel"];
            _pageVM.MobileTel = PersonalData170["MobileTel"];
            _pageVM.FaxTel = PersonalData170["FaxTel"];
            _pageVM.EmailAddr = PersonalData170["EmailAddr"];
            _pageVM.Statement = PersonalData170["Statement"];
            _pageVM.StatementAddr = PersonalData170["StatementAddr"];
         }
         else
            _kernelService.Logger.Warn("PersonalData170 is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData172))
         {
            _pageVM.Page0172Data = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData172].ToString();
            PersonalData172 = JsonConvert.DeserializeObject<Dictionary<string, string>>(_pageVM.Page0172Data);

            _pageVM.MajorJobDesc = PersonalData172["MajorJobDesc"];
            _pageVM.MajorJobKey = PersonalData172["MajorJobKey"];
            _pageVM.CompanyTitle = PersonalData172["CompanyTitle"];
            _pageVM.MajorClassTitle = PersonalData172["MajorClassTitle"];
            _pageVM.SubClassTitle = PersonalData172["SubClassTitle"];
            _pageVM.CompanyTypeName = PersonalData172["CompanyTypeName"];
            _pageVM.JobTitle = PersonalData172["JobTitle"];
            _pageVM.JobTitleKey = PersonalData172["JobTitleKey"];
            _pageVM.JobRiskLvl = PersonalData172["JobRiskLvl"];
            _pageVM.AnnualIncomeDesc = PersonalData172["AnnualIncomeDesc"];
            _pageVM.MonthlyAvgDesc = PersonalData172["MonthlyAvgDesc"];
            _pageVM.PurposeDesc = PersonalData172["PurposeDesc"];
         }
         else
            _kernelService.Logger.Warn("PersonalData172 is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData175))
         {
            _pageVM.Page0175Data = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData175].ToString();
            PersonalData175 = JsonConvert.DeserializeObject<Dictionary<string, string>>(_pageVM.Page0175Data);
            _pageVM.FundSourceDesc = PersonalData175["FundSourceDesc"];
         }
         else
            _kernelService.Logger.Warn("PersonalData175 is not found!");
         
         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData177))
         {
            _pageVM.Page0177Data = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData177].ToString();
            PersonalData177 = JsonConvert.DeserializeObject<Dictionary<string, string>>(_pageVM.Page0177Data);

            _pageVM.WealthySourceDesc = PersonalData177["WealthySourceDesc"];
            _pageVM.StampRefAccNo = PersonalData177["StampRefAccNo"];
         }
         else
            _kernelService.Logger.Warn("PersonalData177 is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData185))
         {
            _pageVM.Page0185Data = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData185].ToString();
            PersonalData185 = JsonConvert.DeserializeObject<Dictionary<string, string>>(_pageVM.Page0185Data);

            _pageVM.ApplyATMCardDesc = PersonalData185["ApplyATMCardDesc"];

            string desc = "";
            if (!string.IsNullOrEmpty(PersonalData185["ApplyTWDAccountPswdDesc"]))
               desc = PersonalData185["ApplyTWDAccountPswdDesc"];

            if (!string.IsNullOrEmpty(PersonalData185["ApplyFGDAccountPswdDesc"]))
            {
               if (desc == "")
                  desc = PersonalData185["ApplyFGDAccountPswdDesc"];
               else
                  desc = desc + "、" + PersonalData185["ApplyFGDAccountPswdDesc"];
            }

            _pageVM.ApplyUniPaymentDesc = desc;
         }
         else
            _kernelService.Logger.Warn("PersonalData185 is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData186))
         {
            _pageVM.Page0186Data = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData186].ToString();
            PersonalData186 = JsonConvert.DeserializeObject<Dictionary<string, string>>(_pageVM.Page0186Data);

            _pageVM.ApplyEBankDesc = PersonalData186["ApplyEBankDesc"];

            if (PersonalData186["ApplyEMobileService"] == "Y")
               _pageVM.eMobileTel = PersonalData186["eMobile"];
            else
               _pageVM.eMobileTel = "";
         }
         else
            _kernelService.Logger.Warn("PersonalData186 is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData187))
         {
            _pageVM.Page0187Data = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData187].ToString();
            PersonalData187 = JsonConvert.DeserializeObject<Dictionary<string, string>>(_pageVM.Page0187Data);
            _pageVM.PromotionTermDesc = PersonalData187["PromotionTermDesc"];
         }
         else
            _kernelService.Logger.Warn("PersonalData187 is not found!");

         scrollViewer.ScrollToTop();

         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      #region ScrollViewer Touch Manipulation
      private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
      {
         e.Handled = true;
      }

      private void ScrollViewer_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         Rectangle rect = Mouse.DirectlyOver as Rectangle;
         if (rect != null)
         {
            ScrollViewer sv = sender as ScrollViewer;
            ScrollBar scrollBar = sv.Template.FindName("PART_VerticalScrollBar", sv) as ScrollBar;
            Track track = FindVisualChild<Track>(scrollBar);

            double trackHeight = track.ActualHeight;
            double factor = sv.ScrollableHeight / trackHeight;

            Point p = e.GetPosition(track);

            sv.ScrollToVerticalOffset(p.Y * factor);

         }
      }

      private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
      {
         for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
         {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is T)
               return (T)child;
            else
            {
               T childOfChild = FindVisualChild<T>(child);
               if (childOfChild != null)
                  return childOfChild;
            }
         }
         return null;
      }
      #endregion

      private void btnModify_Click(object sender, RoutedEventArgs e)
      {
         _pageVM.StopTimer();

         PageResult result = new PageResult("Modify", null);
         _kernelService.NextPage(result);
      }

      private string[] GetFundSource()
      {
         List<string> fundSources = new List<string>();
         if (PersonalData175["FundSource1"] == "Y") fundSources.Add("1");
         if (PersonalData175["FundSource2"] == "Y") fundSources.Add("2");
         if (PersonalData175["FundSource3"] == "Y") fundSources.Add("3");
         if (PersonalData175["FundSource4"] == "Y") fundSources.Add("4");
         if (PersonalData175["FundSource5"] == "Y") fundSources.Add("5");
         if (PersonalData175["FundSource6"] == "Y") fundSources.Add("6");
         if (PersonalData175["FundSource7"] == "Y") fundSources.Add("7");
         if (PersonalData175["FundSource8"] == "Y") fundSources.Add("8");
         if (PersonalData175["FundSource9"] == "Y") fundSources.Add("9");
         if (PersonalData175["FundSourceA"] == "Y") fundSources.Add("A");
         if (PersonalData175["FundSourceB"] == "Y") fundSources.Add("B");

         return fundSources.ToArray();
      }

      private string Json2Base64(string json)
      {
         byte[] bytData = Encoding.UTF8.GetBytes(json);

         return Convert.ToBase64String(bytData);
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         _pageVM.StopTimer();

         bool bSameWithHouse = PersonalData170["IsSameWithHouse"] == "Y";

         string[] fundSrcs = GetFundSource();

         string pcFlagadd = "";
         if(PersonalData170["ShowStatementAddr"] == "Y")
            pcFlagadd = PersonalData170["StatementAddrByHouse"] == "Y" ? "A" : "B";

         var model = new
         {
            regzipcode = PersonalData170["HouseZipCode"],                                  
            regadr = PersonalData170["HouseCityName"],                                     
            regtown = PersonalData170["HouseTownName"],                                    
            reglin = PersonalData170["HouseLin"],                                          
            reglee = PersonalData170["HouseLi"],                                           
            regaddr = PersonalData170["HouseAddress"],                                     
            zipcode2 = PersonalData170["CommZipCode"],                                     
            adr2 = PersonalData170["CommCityName"],                                        
            town2 = PersonalData170["CommTownName"],                                       
            lee2 = PersonalData170["CommLi"],                                              
            addr2 = PersonalData170["CommAddress"],                                        
            zipcode1 = pcFlagadd == "A" ? PersonalData170["HouseZipCode"] : PersonalData170["CommZipCode"],     
            adr1 = pcFlagadd == "A" ? PersonalData170["HouseCityName"] : PersonalData170["CommCityName"],       
            town1 = pcFlagadd == "A" ? PersonalData170["HouseTownName"] : PersonalData170["CommTownName"],      
            lee1 = pcFlagadd == "A" ? PersonalData170["HouseLi"] : PersonalData170["CommLi"],                   
            addr1 = pcFlagadd == "A" ? PersonalData170["HouseAddress"] : PersonalData170["CommAddress"],        
            hphnc = PersonalData170["HomeTelCountry"],                                     
            hpharea = PersonalData170["HomeTelArea"],                                      
            hphno = PersonalData170["HomeTelNumber"],                                      
            ophnc = PersonalData170["OfficeTelCountry"],                                   
            opharea = PersonalData170["OfficeTelArea"],                                    
            ophno = PersonalData170["OfficeTelNumber"],                                    
            ophext = PersonalData170["OfficeExtNumber"],                                   
            mphnc = PersonalData170["MobileCountry"],                                      
            mphno = PersonalData170["MobileNumber"].Replace("-", "").Trim(),                                       
            faxnc1 = PersonalData170["FaxTelCountry"],                                     
            faxarea1 = PersonalData170["FaxTelArea"],                                      
            faxno1 = PersonalData170["FaxTelNumber"],                                      
            email = PersonalData170["Email"],                                              
            pcFlag = PersonalData170["StatementCode"],                                     
            statement = PersonalData170["OldStatementCode"],                               
            jobTitle = PersonalData172["JobTitleKey"],                                     
            serveCompany = PersonalData172["CompanyName"],                                 
            jobBusinessCode = PersonalData172["SubClassMegaCode"],                         
            annualIncome = PersonalData172["AnnualIncomeCode"],                            
            indviAmtRange = PersonalData172["MonthlyAvgCode"],                             
            purpose1 = PersonalData172["PurposeCode1"],                                    
            purpose2 = PersonalData172["PurposeCode2"],                                    
            purpose3 = PersonalData172["PurposeCode3"],                                    
            purpose1Remark = PersonalData172["OtherPurpose"],                              
            incomeResource1 = fundSrcs.Length > 0 ? fundSrcs[0] : "",                      
            incomeResource2 = fundSrcs.Length > 1 ? fundSrcs[1] : "",                      
            incomeResource3 = fundSrcs.Length > 2 ? fundSrcs[2] : "",                      
            incomeResourceIncome = PersonalData175["FundSourceADesc"],                     
            incomeResourceOther = PersonalData175["FundSourceBDesc"],                      
            assetResource1 = PersonalData177["WealthySource1Code"],                        
            assetResourceSub1 = PersonalData177["WealthySource1SubCode"],                  
            assetResourceSub1Other = PersonalData177["WealthySource1Other"],               
            assetResource2 = PersonalData177["WealthySource2Code"],                        
            assetResourceSub2 = PersonalData177["WealthySource2SubCode"],                  
            assetResourceSub2Other = PersonalData177["WealthySource2Other"],               
            assetResource3 = PersonalData177["WealthySource3Code"],                        
            assetResourceSub3 = PersonalData177["WealthySource3SubCode"],                  
            assetResourceSub3Other = PersonalData177["WealthySource3Other"],               
            pbtransf = PersonalData177["StampRefAccNo"],                                   
            pbatmfg = PersonalData185["ApplyATMCard"],                                     
            wfCard = PersonalData185["ICCATMCard"],                                        
            lkkCard = PersonalData185["VISAAtomCard"],                                     
            lotusCard = PersonalData185["VISALotusCard"],                                  
            pb11nofg = PersonalData185["NonDealTRF"],                                      
            pb11stfg = PersonalData185["ConsumeDeduct"],                                   
            pb11trfg = PersonalData185["InternationTrade"],                                
            appWebbank = PersonalData186["ApplyEBank"],                                    
            actfNbSsl = PersonalData186["ApplySSL"] == "Y" ? "1" : "?",                    
            actfNbToSelf = PersonalData186["TRFSameNameAccount"] == "Y" ? "1" : "?",       
            actfNbFree = PersonalData186["EBankNonDealTRF"] == "Y" ? "1" : "?",            
            actfNbFxFlag = PersonalData186["ExchangeClaimService"] == "Y" ? "1" : "?",     
            actfNbRgFlag = PersonalData186["OnlineTRFAccount"] == "Y" ? "1" : "?",         
            actfNbDvPhid = PersonalData186["MobileBank"] == "Y" ? "5" : "?",               
            ecode = PersonalData186["ApplyEMobileService"],                                
            mobile = PersonalData186["eMobile"].Replace("-", "").Trim(),                   
            appPwd = PersonalData185["ApplyUniPayment"],                                   
            twAppPwd = PersonalData185["ApplyTWDAccountPswd"],                             
            fappPwd = PersonalData185["ApplyFGDAccountPswd"],                              
            commarkterm = PersonalData187["PromotionTerm"],                                
            pcFlagadd,                                                                        
            engAddr = PersonalData170["OldEnglishAddress"],                                
            engName = PersonalData170["OldEnglishName"],                                   

            PersonalData170 = Json2Base64(_pageVM.Page0170Data),
            PersonalData172 = Json2Base64(_pageVM.Page0172Data),
            PersonalData175 = Json2Base64(_pageVM.Page0175Data),
            PersonalData177 = Json2Base64(_pageVM.Page0177Data),
            PersonalData185 = Json2Base64(_pageVM.Page0185Data),
            PersonalData186 = Json2Base64(_pageVM.Page0186Data),
            PersonalData187 = Json2Base64(_pageVM.Page0187Data)
         };

         string json = JsonConvert.SerializeObject(model);
         _kernelService.TransactionDataCache.Set(KioskDataCacheKey.ApplyConfirmData, json);

         PageResult result = new PageResult("Confirm", KioskDataCacheKey.ApplyConfirmData);
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

   public class Page0190ViewModel : PageViewModel
   {
      public Page0190ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         Clear();
      }

      public string HouseFullAddress
      {
         get { return GetValue(() => HouseFullAddress); }
         set { SetValue(() => HouseFullAddress, value); }
      }

      public string CommFullAddress
      {
         get { return GetValue(() => CommFullAddress); }
         set { SetValue(() => CommFullAddress, value); }
      }

      public string HomeFullTel
      {
         get { return GetValue(() => HomeFullTel); }
         set { SetValue(() => HomeFullTel, value); }
      }

      public string OfficeFullTel
      {
         get { return GetValue(() => OfficeFullTel); }
         set { SetValue(() => OfficeFullTel, value); }
      }

      public string MobileTel
      {
         get { return GetValue(() => MobileTel); }
         set { SetValue(() => MobileTel, value); }
      }

      public string FaxTel
      {
         get { return GetValue(() => FaxTel); }
         set { SetValue(() => FaxTel, value); }
      }

      public string EmailAddr
      {
         get { return GetValue(() => EmailAddr); }
         set { SetValue(() => EmailAddr, value); }
      }

      public string Statement
      {
         get { return GetValue(() => Statement); }
         set { SetValue(() => Statement, value); }
      }

      public string StatementAddr
      {
         get { return GetValue(() => StatementAddr); }
         set { SetValue(() => StatementAddr, value); }
      }

      public string MajorJobDesc
      {
         get { return GetValue(() => MajorJobDesc); }
         set { SetValue(() => MajorJobDesc, value); }
      }

      public string MajorJobKey
      {
         get { return GetValue(() => MajorJobKey); }
         set { SetValue(() => MajorJobKey, value); }
      }

      public string CompanyTitle
      {
         get { return GetValue(() => CompanyTitle); }
         set { SetValue(() => CompanyTitle, value); }
      }

      public string MajorClassTitle
      {
         get { return GetValue(() => MajorClassTitle); }
         set { SetValue(() => MajorClassTitle, value); }
      }

      public string SubClassTitle
      {
         get { return GetValue(() => SubClassTitle); }
         set { SetValue(() => SubClassTitle, value); }
      }

      public string CompanyTypeName
      {
         get { return GetValue(() => CompanyTypeName); }
         set { SetValue(() => CompanyTypeName, value); }
      }

      public string JobTitle
      {
         get { return GetValue(() => JobTitle); }
         set { SetValue(() => JobTitle, value); }
      }

      public string JobTitleKey
      {
         get { return GetValue(() => JobTitleKey); }
         set { SetValue(() => JobTitleKey, value); }
      }

      public string JobRiskLvl
      {
         get { return GetValue(() => JobRiskLvl); }
         set { SetValue(() => JobRiskLvl, value); }
      }

      public string AnnualIncomeDesc
      {
         get { return GetValue(() => AnnualIncomeDesc); }
         set { SetValue(() => AnnualIncomeDesc, value); }
      }

      public string MonthlyAvgDesc
      {
         get { return GetValue(() => MonthlyAvgDesc); }
         set { SetValue(() => MonthlyAvgDesc, value); }
      }

      public string PurposeDesc
      {
         get { return GetValue(() => PurposeDesc); }
         set { SetValue(() => PurposeDesc, value); }
      }

      public string FundSourceDesc
      {
         get { return GetValue(() => FundSourceDesc); }
         set { SetValue(() => FundSourceDesc, value); }
      }

      public string WealthySourceDesc
      {
         get { return GetValue(() => WealthySourceDesc); }
         set { SetValue(() => WealthySourceDesc, value); }
      }

      public string StampRefAccNo
      {
         get { return GetValue(() => StampRefAccNo); }
         set { SetValue(() => StampRefAccNo, value); }
      }

      public string ApplyATMCardDesc
      {
         get { return GetValue(() => ApplyATMCardDesc); }
         set { SetValue(() => ApplyATMCardDesc, value); }
      }

      public string ApplyEBankDesc
      {
         get { return GetValue(() => ApplyEBankDesc); }
         set { SetValue(() => ApplyEBankDesc, value); }
      }

      public string eMobileTel
      {
         get { return GetValue(() => eMobileTel); }
         set
         {
            SetValue(() => eMobileTel, value);

            if (string.IsNullOrEmpty(eMobileTel))
               eMobileTelColor = Brushes.Black;
            else
               eMobileTelColor = Brushes.Red;
         }
      }

      public SolidColorBrush eMobileTelColor
      {
         get { return GetValue(() => eMobileTelColor); }
         set { SetValue(() => eMobileTelColor, value); }
      }

      public string ApplyUniPaymentDesc
      {
         get { return GetValue(() => ApplyUniPaymentDesc); }
         set { SetValue(() => ApplyUniPaymentDesc, value); }
      }

      public string PromotionTermDesc
      {
         get { return GetValue(() => PromotionTermDesc); }
         set { SetValue(() => PromotionTermDesc, value); }
      }

      public string Page0170Data { get; set; }
      public string Page0172Data { get; set; }
      public string Page0175Data { get; set; }
      public string Page0177Data { get; set; }
      public string Page0185Data { get; set; }
      public string Page0186Data { get; set; }
      public string Page0187Data { get; set; }

      public void Clear()
      {
         HouseFullAddress = "";
         CommFullAddress = "";
         HomeFullTel = "";
         OfficeFullTel = "";
         MobileTel = "";
         FaxTel = "";
         EmailAddr = "";
         Statement = "";
         StatementAddr = "";
         MajorJobDesc = "";
         MajorJobKey = "";
         CompanyTitle = "";
         MajorClassTitle = "";
         SubClassTitle = "";
         CompanyTypeName = "";
         JobTitle = "";
         JobTitleKey = "";
         JobRiskLvl = "";
         AnnualIncomeDesc = "";
         MonthlyAvgDesc = "";
         PurposeDesc = "";
         FundSourceDesc = "";
         WealthySourceDesc = "";
         StampRefAccNo = "";
         ApplyATMCardDesc = "";
         eMobileTel = "";
         eMobileTelColor = Brushes.Black;
         ApplyEBankDesc = "";
         ApplyUniPaymentDesc = "";
         PromotionTermDesc = "";

         Page0170Data = "";
         Page0172Data = "";
         Page0175Data = "";
         Page0177Data = "";
         Page0185Data = "";
         Page0186Data = "";
         Page0187Data = "";
      }
   }
}
