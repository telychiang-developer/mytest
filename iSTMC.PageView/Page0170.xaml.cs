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
using Xceed.Wpf.Toolkit;

namespace iSTMC.PageView
{
   /// <summary>
   /// Page0170.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0170", Description = "資料填寫-個人資料-填寫個人資料170")]
   public partial class Page0170 : Page
   {
      private Page0170ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0170(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0170ViewModel(kernelService, "Page0170");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0170_Loaded;
      }

      private void Page0170_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey("__ADDRESS_DATA"))
            _pageVM.AddressDropdown = _kernelService.TransactionDataCache["__ADDRESS_DATA"] as AddressDropdown;
         else
            _kernelService.Logger.Warn("__ADDRESS_DATA is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey("__BANK_STATEMENT"))
         {
            _pageVM.StatementDropdown = _kernelService.TransactionDataCache["__BANK_STATEMENT"] as StatementDropdown;
            _pageVM.StatementCode = "Q";
            _pageVM.StatementEnabled = true;
         }
         else
            _kernelService.Logger.Warn("__BANK_STATEMENT is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData170))
         {
            // 已存在填寫個人資料(個人通訊資料頁)，例如：由後面頁面返回本頁時，可預填入前次輸入資料
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData170].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.HouseZipCode = jsonObj["HouseZipCode"];
            _pageVM.HouseCityName = jsonObj["HouseCityName"];
            _pageVM.HouseTownName = jsonObj["HouseTownName"];
            _pageVM.HouseLi = jsonObj["HouseLi"];
            _pageVM.HouseLin = jsonObj["HouseLin"];
            _pageVM.HouseAddress = jsonObj["HouseAddress"];
            _pageVM.IsSameWithHouse = jsonObj["IsSameWithHouse"] == "Y";
            _pageVM.CommZipCode = jsonObj["CommZipCode"];
            _pageVM.CommCityName = jsonObj["CommCityName"];
            _pageVM.CommTownName = jsonObj["CommTownName"];
            _pageVM.CommLi = jsonObj["CommLi"];
            _pageVM.CommLin = jsonObj["CommLin"];
            _pageVM.CommAddress = jsonObj["CommAddress"];
            _pageVM.HomeTelCountry = jsonObj["HomeTelCountry"];
            _pageVM.HomeTelArea = jsonObj["HomeTelArea"];
            _pageVM.HomeTelNumber = jsonObj["HomeTelNumber"];
            _pageVM.OfficeTelCountry = jsonObj["OfficeTelCountry"];
            _pageVM.OfficeTelArea = jsonObj["OfficeTelArea"];
            _pageVM.OfficeTelNumber = jsonObj["OfficeTelNumber"];
            _pageVM.OfficeExtNumber = jsonObj["OfficeExtNumber"];
            _pageVM.MobileCountry = jsonObj["MobileCountry"];
            _pageVM.MobileNumber = jsonObj["MobileNumber"];
            _pageVM.FaxTelCountry = jsonObj["FaxTelCountry"];
            _pageVM.FaxTelArea = jsonObj["FaxTelArea"];
            _pageVM.FaxTelNumber = jsonObj["FaxTelNumber"];
            _pageVM.Email = jsonObj["Email"];
            _pageVM.OldStatementCode = jsonObj["OldStatementCode"];
            _pageVM.HasOldAccount = jsonObj["HasOldAccount"] == "Y";
            _pageVM.OldEnglishAddress = jsonObj["OldEnglishAddress"];
            _pageVM.OldEnglishName = jsonObj["OldEnglishName"];

            if (_pageVM.HasOldAccount)
            {
               var existedItem = _pageVM.StatementDropdown.Data.Statements.SingleOrDefault(x => x.Code == "NA");
               if (existedItem == null)
                  _pageVM.StatementDropdown.Data.Statements.Add(new Statement() { Code = "NA", Name = "已設定寄送方式" });

               _pageVM.StatementCode = "NA";
               _pageVM.StatementEnabled = false;
            }
            else
            {
               _pageVM.StatementCode = jsonObj["StatementCode"];
               _pageVM.StatementEnabled = true;
            }

            _pageVM.ShowStatementAddr = jsonObj["ShowStatementAddr"] == "Y";
            _pageVM.StatementAddrByHouse = jsonObj["StatementAddrByHouse"] == "Y";
            _pageVM.StatementAddrByComm = jsonObj["StatementAddrByComm"] == "Y";
         }
         else
         {
            // 未存在填寫個人資料(個人通訊資料頁)，例如：第一次進入本頁面時，可預填入OCR輔助辨識資料或預處理資料

            /*
             * 依2009-32警示戶查詢電文回應的舊戶查詢oldAccResult(Y/N/P)與preReserve(Y/N)欄位判斷，戶籍地址(HouseAddress)的填值邏輯如下：
             * 1) N=未建檔(新戶)：填入OCR輔助資料
             * 2) P=預處理新戶：填入預處理資料
             * 3) Y=有建檔(舊戶+存在預處理資料preReserve=Y)：填入預處理資料
             * 4) Y=有建檔(舊戶+不存在預處理資料preReserve=N)：填入OCR輔助資料
             */

            _pageVM.HasOldAccount = false;
            if (_kernelService.TransactionDataCache.ContainsKey("__WARN_ACCOUNT_DATA"))
            {
               // __WARN_ACCOUNT_DATA：轉存警示戶資料

               string json = _kernelService.TransactionDataCache["__WARN_ACCOUNT_DATA"].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               if (jsonObj.ContainsKey("oldAccResult"))
               {
                  // 舊戶查詢(oldAccResult)：Y=有建檔(舊戶+不存在預處理資料 or 舊戶+存在預處理資料)、N=未建檔(新戶)、P=預處理新戶
                  // 是否為預處理(preReserve)：Y=預處理客戶、N=非預處理客戶

                  // 歸屬為舊戶帳戶(因為存在 __OLD_ACCOUNT_DATA 轉存舊帳戶資料的變數資料)：Y, P
                  _pageVM.HasOldAccount = jsonObj["oldAccResult"] == "Y" || jsonObj["oldAccResult"] == "P";
                  // 屬於預處理資料：P=預處理新戶、Y=有建檔(舊戶+存在預處理資料)
                  _pageVM.IsPreProcAccount = jsonObj["oldAccResult"] == "P" || (jsonObj["oldAccResult"] == "Y" && jsonObj.ContainsKey("preReserve") && jsonObj["preReserve"] == "Y");
               }
            }

            if (!_pageVM.IsPreProcAccount && _kernelService.TransactionDataCache.ContainsKey("__2004_9_BodyData"))
            {
               // 非預處理資料，且存在OCR輔助辨識資料(__2004_9_BodyData)

               string json = _kernelService.TransactionDataCache["__2004_9_BodyData"].ToString();

               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               if (jsonObj.ContainsKey("address"))
               {
                  // 戶籍地址，填入OCR輔助辨識資料
                  _pageVM.HouseAddress = jsonObj["address"];
                  _kernelService.Logger.Info($"address : {_pageVM.HouseAddress}");
               }
            }
            else
               _kernelService.Logger.Warn("__2004_9_BodyData is not found!");

            
            if (_pageVM.HasOldAccount && _kernelService.TransactionDataCache.ContainsKey("__OLD_ACCOUNT_DATA"))
            {
               // __OLD_ACCOUNT_DATA：轉存舊帳戶資料

               // 為舊戶帳戶，且有轉存舊帳戶資料
               string oldAccountData = _kernelService.TransactionDataCache["__OLD_ACCOUNT_DATA"].ToString();
               var oaData = JsonConvert.DeserializeObject<Dictionary<string, string>>(oldAccountData);

               _kernelService.Logger.Info("Page0170: " + oldAccountData);

               // 若為預處理資料，需對應填入預處理資料(戶籍地址的郵遞區號、縣市、鄉鎮、村里、鄰、街道)

               foreach (string key in oaData.Keys)
               {
                  switch (key)
                  {
                     case "REGZIPCODE":    
                        if (_pageVM.IsPreProcAccount)
                           _pageVM.HouseZipCode = oaData[key];
                        break;
                     case "REGADR":        
                        if (_pageVM.IsPreProcAccount)
                           _pageVM.HouseCityName = oaData[key];
                        break;
                     case "REGTOWN":       
                        if (_pageVM.IsPreProcAccount)
                           _pageVM.HouseTownName = oaData[key];
                        break;
                     case "REGLIN":        
                        if (_pageVM.IsPreProcAccount)
                           _pageVM.HouseLin = oaData[key];
                        break;
                     case "REGLEE":        
                        if (_pageVM.IsPreProcAccount)
                           _pageVM.HouseLi = oaData[key];
                        break;
                     case "REGADDR":       
                        if (_pageVM.IsPreProcAccount)
                           _pageVM.HouseAddress = oaData[key];
                        break;
                     case "ZIPCODE2":     
                        _pageVM.CommZipCode = oaData[key];
                        break;
                     case "ADR2":         
                        _pageVM.CommCityName = oaData[key];
                        break;
                     case "TOWN2":        
                        _pageVM.CommTownName = oaData[key];
                        break;
                     case "LEE2":         
                        _pageVM.CommLi = oaData[key];
                        break;
                     case "ADDR2":        
                        _pageVM.CommAddress = oaData[key];
                        break;
                     case "HPHNC":        
                        _pageVM.HomeTelCountry = string.IsNullOrWhiteSpace(oaData[key]) ? "886" : oaData[key];
                        break;
                     case "HPHAREA":      
                        _pageVM.HomeTelArea = oaData[key];
                        break;
                     case "HPHNO":        
                        _pageVM.HomeTelNumber = oaData[key];
                        break;
                     case "OPHNC":        
                        _pageVM.OfficeTelCountry = string.IsNullOrWhiteSpace(oaData[key]) ? "886" : oaData[key];
                        break;
                     case "OPHAREA":      
                        _pageVM.OfficeTelArea = oaData[key];
                        break;
                     case "OPHNO":        
                        _pageVM.OfficeTelNumber = oaData[key];
                        break;
                     case "OPHEXT":      
                        _pageVM.OfficeExtNumber = oaData[key];
                        break;
                     case "MPHNC":        
                        _pageVM.MobileCountry = string.IsNullOrWhiteSpace(oaData[key]) ? "886" : oaData[key];
                        break;
                     case "MPHNO":        
                        _pageVM.MobileNumber = oaData[key];
                        break;
                     case "FAXNC1":       
                        _pageVM.FaxTelCountry = string.IsNullOrWhiteSpace(oaData[key]) ? "886" : oaData[key];
                        break;
                     case "FAXAREA1":     
                        _pageVM.FaxTelArea = oaData[key];
                        break;
                     case "FAXNO1":       
                        _pageVM.FaxTelNumber = oaData[key];
                        break;
                     case "EMAIL":        
                        _pageVM.Email = oaData[key];
                        break;
                     case "PC_FLAG":      
                        {
                           _pageVM.OldStatementCode = oaData[key];

                           var existedItem = _pageVM.StatementDropdown.Data.Statements.SingleOrDefault(x => x.Code == "NA");
                           if (existedItem == null)
                              _pageVM.StatementDropdown.Data.Statements.Add(new Statement() { Code = "NA", Name = "已設定寄送方式" });

                           _pageVM.StatementCode = "NA";
                           _pageVM.StatementEnabled = false;

                        }
                        break;
                     case "engAddr":   
                        _pageVM.OldEnglishAddress = oaData[key];
                        break;
                     case "engNameO":  
                        _pageVM.OldEnglishName = oaData[key];
                        break;
                     default:
                        break;
                  }
               }
            }
            else
            {
               if(_pageVM.HasOldAccount)
                  _kernelService.Logger.Warn("__OLD_ACCOUNT_DATA is not found!");
            }
         }

         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         List<string> errorList = new List<string>();
         PageViewUtils.GetErrors(errorList, grdForm);

         if(string.IsNullOrEmpty(_pageVM.StatementCode))
            errorList.Add("對帳單必須選擇");
         else
         {
            if (_pageVM.StatementCode == "F" || _pageVM.StatementCode == "G")
            {
               if (string.IsNullOrEmpty(_pageVM.Email))
                  errorList.Add("對帳單要求Email通知但Email未填寫!");
            }
         }

         if (_pageVM.ShowStatementAddr && !_pageVM.StatementAddrByHouse && !_pageVM.StatementAddrByComm)
            errorList.Add("對帳單地址選項尚未勾選");

         if (string.IsNullOrEmpty(_pageVM.HouseCityName))
            errorList.Add("戶籍地址縣市未填寫");

         if (string.IsNullOrEmpty(_pageVM.HouseTownName))
            errorList.Add("戶籍地址鄉鎮未填寫");

         if (string.IsNullOrEmpty(_pageVM.CommCityName))
            errorList.Add("通訊地址縣市未填寫");

         if (string.IsNullOrEmpty(_pageVM.CommTownName))
            errorList.Add("通訊地址鄉鎮未填寫");

         if(!string.IsNullOrEmpty(_pageVM.OfficeTelArea) || !string.IsNullOrEmpty(_pageVM.OfficeTelNumber) || !string.IsNullOrEmpty(_pageVM.OfficeExtNumber))
         {
            if (string.IsNullOrEmpty(_pageVM.OfficeTelArea) || string.IsNullOrEmpty(_pageVM.OfficeTelNumber))
               errorList.Add("公司電話號碼不完整");
         }

         _kernelService.Logger.Info($"MobileNumber = {_pageVM.MobileNumber}");
         string mobile = _pageVM.MobileNumber.Replace("-", "").Trim();
         _kernelService.Logger.Info($"mobile = {mobile}");
         _kernelService.Logger.Info($"mobile.length = {mobile.Length}");

         if (!string.IsNullOrEmpty(mobile))
         {
            if (!mobile.StartsWith("09") || mobile.Length != 10)
            errorList.Add("行動電話號碼有誤");
         }

         if(!string.IsNullOrEmpty(_pageVM.FaxTelArea) || !string.IsNullOrEmpty(_pageVM.FaxTelNumber))
         {
            if(string.IsNullOrEmpty(_pageVM.FaxTelArea) || string.IsNullOrEmpty(_pageVM.FaxTelNumber))
               errorList.Add("傳真號碼不完整");
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

            var statement = _pageVM.StatementDropdown.Data.Statements.SingleOrDefault(x => x.Code == _pageVM.StatementCode);

            string fullHouseAddress = _pageVM.HouseCityName + _pageVM.HouseTownName + _pageVM.HouseLi + _pageVM.HouseLin + "鄰" + _pageVM.HouseAddress;
            string fullCommAddress = _pageVM.CommCityName + _pageVM.CommTownName + (string.IsNullOrEmpty(_pageVM.CommLi) ? "" : _pageVM.CommLi) + (string.IsNullOrEmpty(_pageVM.CommLin) ? "" : _pageVM.CommLin + "鄰") + _pageVM.CommAddress;

            var model = new
            {
               _pageVM.HouseZipCode,            
               _pageVM.HouseCityName,           
               _pageVM.HouseTownName,           
               _pageVM.HouseLi,                 
               _pageVM.HouseLin,                
               _pageVM.HouseAddress,            
               IsSameWithHouse = _pageVM.IsSameWithHouse ? "Y" : "N",     
               _pageVM.CommZipCode,             
               _pageVM.CommCityName,            
               _pageVM.CommTownName,            
               _pageVM.CommLi,                  
               _pageVM.CommLin,                 
               _pageVM.CommAddress,             
               _pageVM.HomeTelCountry,          
               _pageVM.HomeTelArea,             
               _pageVM.HomeTelNumber,           
               _pageVM.OfficeTelCountry,        
               _pageVM.OfficeTelArea,           
               _pageVM.OfficeTelNumber,         
               _pageVM.OfficeExtNumber,         
               _pageVM.MobileCountry,           
               _pageVM.MobileNumber,            
               _pageVM.FaxTelCountry,           
               _pageVM.FaxTelArea,              
               _pageVM.FaxTelNumber,            
               _pageVM.Email,                   
               _pageVM.StatementCode,           
               _pageVM.OldStatementCode,        
               StatementAddrByHouse = _pageVM.StatementAddrByHouse ? "Y" : "N",   
               StatementAddrByComm = _pageVM.StatementAddrByComm ? "Y" : "N",     
               ShowStatementAddr = _pageVM.ShowStatementAddr ? "Y" : "N",         
               HasOldAccount = _pageVM.HasOldAccount ? "Y" : "N",
               IsPreProcAccount = _pageVM.IsPreProcAccount ? "Y" : "N",
               _pageVM.OldEnglishAddress,       
               _pageVM.OldEnglishName,            

               HouseFullAddress = fullHouseAddress,
               CommFullAddress = fullCommAddress,
               HomeFullTel = _pageVM.HomeTelArea + '-' + _pageVM.HomeTelNumber,
               OfficeFullTel = _pageVM.OfficeTelNumber != "" ? _pageVM.OfficeTelArea + '-' + _pageVM.OfficeTelNumber + (!string.IsNullOrEmpty(_pageVM.OfficeExtNumber) ? "  分機: " + _pageVM.OfficeExtNumber : "") : "",
               MobileTel = _pageVM.MobileNumber != "" ? _pageVM.MobileNumber : "",
               FaxTel = _pageVM.FaxTelNumber != "" ? _pageVM.FaxTelArea + '-' + _pageVM.FaxTelNumber : "",
               EmailAddr = _pageVM.Email != "" ? _pageVM.Email : "",
               Statement = statement != null ? statement.Name : "",
               StatementAddr = statement.Code != "Q" && statement.Code != "NA" ? (_pageVM.StatementAddrByHouse ? "同戶籍地址;" + fullHouseAddress : "同通訊地址;" + fullCommAddress) : "",
            };

            _pageVM.Clear();

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData170, json);

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.PersonalData170);
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

   public class Page0170ViewModel : PageViewModel
   {
      public Page0170ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         AddressDropdown = new AddressDropdown();
         StatementDropdown = new StatementDropdown();

         Clear();
      }

      public AddressDropdown AddressDropdown
      {
         get { return GetValue(() => AddressDropdown); }
         set
         {
            SetValue(() => AddressDropdown, value);
         }
      }

      public StatementDropdown StatementDropdown
      {
         get { return GetValue(() => StatementDropdown); }
         set
         {
            SetValue(() => StatementDropdown, value);
         }
      }

      public List<AddressTown> HouseAddressTowns
      {
         get { return GetValue(() => HouseAddressTowns); }
         set
         {
            SetValue(() => HouseAddressTowns, value);
         }
      }

      [Required(ErrorMessage = "戶籍地址縣市未填寫")]
      public string HouseCityName
      {
         get { return GetValue(() => HouseCityName); }
         set
         {
            SetValue(() => HouseCityName, value);

            if (AddressDropdown.Data != null && AddressDropdown.Data.Cities != null)
            {
               var city = AddressDropdown.Data.Cities.SingleOrDefault(x => x.Name == value);
               if (city != null)
                  HouseAddressTowns = city.Towns;
            }

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            IsSameWithHouse = false;
         }
      }

      [Required(ErrorMessage = "戶籍地址鄉鎮未填寫")]
      public string HouseTownName
      {
         get { return GetValue(() => HouseTownName); }
         set
         {
            SetValue(() => HouseTownName, value);
            if(HouseAddressTowns != null)
            {
               var town = HouseAddressTowns.SingleOrDefault(x => x.Name == value);
               if (town != null)
                  HouseZipCode = town.ZipCode;
            }

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            IsSameWithHouse = false;
         }
      }

      [Required(ErrorMessage = "戶籍地址郵遞區號未填寫")]
      public string HouseZipCode
      {
         get { return GetValue(() => HouseZipCode); }
         set
         {
            SetValue(() => HouseZipCode, value);

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            IsSameWithHouse = false;
         }
      }

      [Required(ErrorMessage = "戶籍地址村里為未填寫")]
      public string HouseLi
      {
         get { return GetValue(() => HouseLi); }
         set
         {
            SetValue(() => HouseLi, value);

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            IsSameWithHouse = false;
         }
      }

      [Required(ErrorMessage = "戶籍地址鄰為未填寫")]
      public string HouseLin
      {
         get { return GetValue(() => HouseLin); }
         set
         {
            SetValue(() => HouseLin, value);

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            IsSameWithHouse = false;
         }
      }

      [Required(ErrorMessage = "戶籍地址街道未填寫")]
      public string HouseAddress
      {
         get { return GetValue(() => HouseAddress); }
         set
         {
            SetValue(() => HouseAddress, value);

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            IsSameWithHouse = false;
         }
      }

      public List<AddressTown> CommAddressTowns
      {
         get { return GetValue(() => CommAddressTowns); }
         set
         {
            SetValue(() => CommAddressTowns, value);
         }
      }

      [Required(ErrorMessage = "通訊地址縣市未填寫")]
      public string CommCityName
      {
         get { return GetValue(() => CommCityName); }
         set
         {
            SetValue(() => CommCityName, value);

            if (AddressDropdown.Data != null && AddressDropdown.Data.Cities != null)
            {
               var city = AddressDropdown.Data.Cities.SingleOrDefault(x => x.Name == value);
               if (city != null)
                  CommAddressTowns = city.Towns;
            }

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            if (value != null && IsSameWithHouse && HouseCityName != value)
               IsSameWithHouse = false;
         }
      }

      [Required(ErrorMessage = "通訊地址鄉鎮未填寫")]
      public string CommTownName
      {
         get { return GetValue(() => CommTownName); }
         set
         {
            SetValue(() => CommTownName, value);
            if (CommAddressTowns != null)
            {
               var town = CommAddressTowns.SingleOrDefault(x => x.Name == value);
               if (town != null)
                  CommZipCode = town.ZipCode;
            }

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            if (value != null && IsSameWithHouse && HouseTownName != value)
               IsSameWithHouse = false;
         }
      }

      [Required(ErrorMessage = "通訊地址郵遞區號未填寫")]
      public string CommZipCode
      {
         get { return GetValue(() => CommZipCode); }
         set
         {
            SetValue(() => CommZipCode, value);

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            if (value != null && IsSameWithHouse && HouseZipCode != value)
               IsSameWithHouse = false;
         }
      }

      public string CommLi
      {
         get { return GetValue(() => CommLi); }
         set
         {
            SetValue(() => CommLi, value);

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            if (value != null && IsSameWithHouse && HouseLi != value)
               IsSameWithHouse = false;
         }
      }

      public string CommLin
      {
         get { return GetValue(() => CommLin); }
         set
         {
            SetValue(() => CommLin, value);

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            if (value != null && IsSameWithHouse && HouseLin != value)
               IsSameWithHouse = false;
         }
      }

      [Required(ErrorMessage = "通訊地址街道未填寫")]
      public string CommAddress
      {
         get { return GetValue(() => CommAddress); }
         set
         {
            SetValue(() => CommAddress, value);

            StatementAddrByHouse = false;
            StatementAddrByComm = false;

            if (value != null && IsSameWithHouse && HouseAddress != value)
               IsSameWithHouse = false;
         }
      }

      public bool IsSameWithHouse
      {
         get { return GetValue(() => IsSameWithHouse); }
         set
         {
            SetValue(() => IsSameWithHouse, value);

            IsNotSameWithHouse = !value;

            if(value)
            {
               CommCityName = HouseCityName;
               CommTownName = HouseTownName;
               CommZipCode = HouseZipCode;
               CommLi = HouseLi;
               CommLin = HouseLin;
               CommAddress = HouseAddress;
            }
         }
      }

      public bool IsNotSameWithHouse
      {
         get { return GetValue(() => IsNotSameWithHouse); }
         set
         {
            SetValue(() => IsNotSameWithHouse, value);
         }
      }

      [Required(ErrorMessage = "住家電話國碼未填寫")]
      public string HomeTelCountry
      {
         get { return GetValue(() => HomeTelCountry); }
         set
         {
            SetValue(() => HomeTelCountry, value);
         }
      }

      [Required(ErrorMessage = "住家電話區碼未填寫")]
      public string HomeTelArea
      {
         get { return GetValue(() => HomeTelArea); }
         set
         {
            SetValue(() => HomeTelArea, value);
         }
      }

      [Required(ErrorMessage = "住家電話號碼未填寫")]
      public string HomeTelNumber
      {
         get { return GetValue(() => HomeTelNumber); }
         set
         {
            SetValue(() => HomeTelNumber, value);
         }
      }

      public string OfficeTelCountry
      {
         get { return GetValue(() => OfficeTelCountry); }
         set
         {
            SetValue(() => OfficeTelCountry, value);
         }
      }

      public string OfficeTelArea
      {
         get { return GetValue(() => OfficeTelArea); }
         set
         {
            SetValue(() => OfficeTelArea, value);
         }
      }

      public string OfficeTelNumber
      {
         get { return GetValue(() => OfficeTelNumber); }
         set
         {
            SetValue(() => OfficeTelNumber, value);
         }
      }

      public string OfficeExtNumber
      {
         get { return GetValue(() => OfficeExtNumber); }
         set
         {
            SetValue(() => OfficeExtNumber, value);
         }
      }

      public string FaxTelCountry
      {
         get { return GetValue(() => FaxTelCountry); }
         set
         {
            SetValue(() => FaxTelCountry, value);
         }
      }

      public string FaxTelArea
      {
         get { return GetValue(() => FaxTelArea); }
         set
         {
            SetValue(() => FaxTelArea, value);
         }
      }

      public string FaxTelNumber
      {
         get { return GetValue(() => FaxTelNumber); }
         set
         {
            SetValue(() => FaxTelNumber, value);
         }
      }

      public string MobileCountry
      {
         get { return GetValue(() => MobileCountry); }
         set
         {
            SetValue(() => MobileCountry, value);
         }
      }

      public string MobileNumber
      {
         get { return GetValue(() => MobileNumber); }
         set
         {
            SetValue(() => MobileNumber, value);
         }
      }

      [EmailAddress(ErrorMessage = "電子信箱格式有誤")]
      public string Email
      {
         get { return GetValue(() => Email); }
         set
         {
            var val = value == "" ? null : value;
            SetValue(() => Email, val);
         }
      }

      [Required(ErrorMessage = "對帳單選單未填寫")]
      public string StatementCode
      {
         get { return GetValue(() => StatementCode); }
         set
         {
            SetValue(() => StatementCode, value);

            if (value == "R" || value == "G")
               ShowStatementAddr = true;
            else
               ShowStatementAddr = false;
         }
      }

      public bool StatementEnabled
      {
         get { return GetValue(() => StatementEnabled); }
         set
         {
            SetValue(() => StatementEnabled, value);
         }
      }

      public string OldStatementCode
      {
         get { return GetValue(() => OldStatementCode); }
         set
         {
            SetValue(() => OldStatementCode, value);
         }
      }

      public bool StatementAddrByHouse
      {
         get { return GetValue(() => StatementAddrByHouse); }
         set
         {
            SetValue(() => StatementAddrByHouse, value);
         }
      }

      public bool StatementAddrByComm
      {
         get { return GetValue(() => StatementAddrByComm); }
         set
         {
            SetValue(() => StatementAddrByComm, value);
         }
      }

      public bool ShowStatementAddr
      {
         get { return GetValue(() => ShowStatementAddr); }
         set
         {
            SetValue(() => ShowStatementAddr, value);
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

      public string OldEnglishAddress
      {
         get { return GetValue(() => OldEnglishAddress); }
         set
         {
            SetValue(() => OldEnglishAddress, value);
         }
      }

      public string OldEnglishName
      {
         get { return GetValue(() => OldEnglishName); }
         set
         {
            SetValue(() => OldEnglishName, value);
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
         HouseAddressTowns = new List<AddressTown>();
         CommAddressTowns = new List<AddressTown>();

         HouseCityName = "";
         HouseTownName = "";
         HouseZipCode = "";
         HouseLi = "";
         HouseLin = "";
         HouseAddress = "";
         CommCityName = "";
         CommTownName = "";
         CommZipCode = "";
         CommLi = "";
         CommLin = "";
         CommAddress = "";
         IsSameWithHouse = false;
         HomeTelCountry = "886";
         HomeTelArea = "";
         HomeTelNumber = "";
         OfficeTelCountry = "886";
         OfficeTelArea = "";
         OfficeTelNumber = "";
         OfficeExtNumber = "";
         FaxTelCountry = "886";
         FaxTelArea = "";
         FaxTelNumber = "";
         MobileCountry = "886";
         MobileNumber = "";
         Email = "";
         StatementCode = "";
         OldStatementCode = "";
         StatementEnabled = true;
         StatementAddrByHouse = false;
         StatementAddrByComm = false;
         ShowStatementAddr = false;
         HasOldAccount = false;
         OldEnglishAddress = "";
         OldEnglishName = "";
         IsPreProcAccount = false;
      }
   }
}
