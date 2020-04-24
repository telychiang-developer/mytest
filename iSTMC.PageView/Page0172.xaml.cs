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
   /// Page0172.xaml 的互動邏輯
   /// </summary>
   [PageView("Page0172", Description = "資料填寫-個人資料-填寫個人資料172")]
   public partial class Page0172 : Page
   {
      private Page0172ViewModel _pageVM;
      private readonly IKernelService _kernelService;

      public Page0172(IKernelService kernelService)
      {
         InitializeComponent();

         _pageVM = new Page0172ViewModel(kernelService, "Page0172");

         _kernelService = kernelService;

         this.DataContext = _pageVM;

         this.Loaded += Page0172_Loaded;
      }

      private void Page0172_Loaded(object sender, RoutedEventArgs e)
      {
         _pageVM.Clear();

         if (_kernelService.TransactionDataCache.ContainsKey("__JOB_INDUSTRY"))
            _pageVM.IndustryDropdown = _kernelService.TransactionDataCache["__JOB_INDUSTRY"] as IndustryDropdown;
         else
            _kernelService.Logger.Warn("__JOB_INDUSTRY is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey("__JOB_TITLE"))
            _pageVM.JobPositionDropdown = _kernelService.TransactionDataCache["__JOB_TITLE"] as JobPositionDropdown;
         else
            _kernelService.Logger.Warn("__JOB_TITLE is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey("__ANNUAL_INCOME"))
            _pageVM.AnnualIncomeDropdown = _kernelService.TransactionDataCache["__ANNUAL_INCOME"] as AnnualIncomeDropdown;
         else
            _kernelService.Logger.Warn("__ANNUAL_INCOME is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey("__MONTHLY_AVERAGE"))
            _pageVM.MonthlyAvgDropdown = _kernelService.TransactionDataCache["__MONTHLY_AVERAGE"] as MonthlyAvgDropdown;
         else
            _kernelService.Logger.Warn("__MONTHLY_AVERAGE is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey("__OPEN_PURPOSE"))
         {
            _pageVM.PurposeDropdown = _kernelService.TransactionDataCache["__OPEN_PURPOSE"] as PurposeDropdown;
            _pageVM.PurposeSelectItems.Clear();
         }
         else
            _kernelService.Logger.Warn("__OPEN_PURPOSE is not found!");

         if (_kernelService.TransactionDataCache.ContainsKey(KioskDataCacheKey.PersonalData172))
         {
            string json = _kernelService.TransactionDataCache[KioskDataCacheKey.PersonalData172].ToString();
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            _pageVM.MajorJobName = jsonObj["MajorJobName"];
            _pageVM.CompanyTypeCode = jsonObj["CompanyTypeCode"];
            _pageVM.CompanyName = jsonObj["CompanyName"];
            _pageVM.MajorClassName = jsonObj["MajorClassName"];
            _pageVM.SubClassName = jsonObj["SubClassName"];
            _pageVM.JobPositionName = jsonObj["JobPositionName"];
            _pageVM.AnnualIncomeCode = jsonObj["AnnualIncomeCode"];
            _pageVM.MonthlyAvgCode = jsonObj["MonthlyAvgCode"];

            List<Purpose> purposes = new List<Purpose>();

            if (!string.IsNullOrEmpty(jsonObj["PurposeCode1"]))
            {
               string code = jsonObj["PurposeCode1"];
               if (_pageVM.PurposeDropdown != null && _pageVM.PurposeDropdown.Data != null)
               {
                  Purpose purpose = _pageVM.PurposeDropdown.Data.Purposes.SingleOrDefault(x => x.Code == code);
                  purposes.Add(purpose);
               }
            }

            if (!string.IsNullOrEmpty(jsonObj["PurposeCode2"]))
            {
               string code = jsonObj["PurposeCode2"];
               if (_pageVM.PurposeDropdown != null && _pageVM.PurposeDropdown.Data != null)
               {
                  Purpose purpose = _pageVM.PurposeDropdown.Data.Purposes.SingleOrDefault(x => x.Code == code);
                  purposes.Add(purpose);
               }
            }

            if (!string.IsNullOrEmpty(jsonObj["PurposeCode3"]))
            {
               string code = jsonObj["PurposeCode3"];
               if (_pageVM.PurposeDropdown != null && _pageVM.PurposeDropdown.Data != null)
               {
                  Purpose purpose = _pageVM.PurposeDropdown.Data.Purposes.SingleOrDefault(x => x.Code == code);
                  purposes.Add(purpose);
               }
            }

            _pageVM.PurposeSelectItems = purposes;
            _pageVM.OtherPurpose = jsonObj["OtherPurpose"];
            _pageVM.RequireJobData = jsonObj["RequireJobData"] == "Y";
            _pageVM.PurposeEnabled = jsonObj["PurposeEnabled"] == "Y";
            _pageVM.OtherPurposeEnabled = jsonObj["OtherPurposeEnabled"] == "Y";
            _pageVM.WarnResultA = jsonObj["WarnResultA"];
         }
         else
         {
            bool bHasOldAccount = false;
            if (_kernelService.TransactionDataCache.ContainsKey("__WARN_ACCOUNT_DATA"))
            {
               string json = _kernelService.TransactionDataCache["__WARN_ACCOUNT_DATA"].ToString();
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

               if (jsonObj.ContainsKey("oldAccResult"))
                  bHasOldAccount = jsonObj["oldAccResult"] == "Y" || jsonObj["oldAccResult"] == "P";

               if (jsonObj.ContainsKey("warnResultS"))
               {
                  string val = jsonObj["warnResultS"];
                  if (val == "Y")
                  {
                     _pageVM.Purposes.Clear();
                     var p2 = _pageVM.PurposeDropdown.Data.Purposes.SingleOrDefault(x => x.Code == "2");
                     if (p2 != null) _pageVM.Purposes.Add(p2);
                     var p6 = _pageVM.PurposeDropdown.Data.Purposes.SingleOrDefault(x => x.Code == "6");
                     if (p6 != null) _pageVM.Purposes.Add(p6);
                     var pF = _pageVM.PurposeDropdown.Data.Purposes.SingleOrDefault(x => x.Code == "F");
                     if (pF != null) _pageVM.Purposes.Add(pF);

                     if (jsonObj.ContainsKey("warnResultA"))
                     {
                        List<Purpose> purposes = new List<Purpose>();
                        List<Purpose> purposeSelected = new List<Purpose>();

                        if (jsonObj["warnResultA"] == "2")
                        {
                           purposes.Add(p2);
                           purposeSelected.Add(p2);
                        }
                        else if (jsonObj["warnResultA"] == "6")
                        {
                           purposes.Add(p6);
                           purposeSelected.Add(p6);
                        }
                        else if (jsonObj["warnResultA"] == "F")
                        {
                           purposes.Add(pF);
                           purposeSelected.Add(pF);
                        }

                        _pageVM.Purposes = purposes;
                        _pageVM.PurposeSelectItems = purposeSelected;
                     }

                     _pageVM.PurposeEnabled = false;
                  }
                  else
                  {
                     _pageVM.PurposeEnabled = true;
                     _pageVM.Purposes = _pageVM.PurposeDropdown.Data.Purposes;
                  }
               }
               else
               {
                  _pageVM.PurposeEnabled = true;
                  _kernelService.Logger.Warn("__WARN_ACCOUNT_DATA is not found!");
               }
            }
            else
            {
               _pageVM.PurposeEnabled = true;
               _kernelService.Logger.Warn("__WARN_ACCOUNT_DATA is not found!");
            }

            if (bHasOldAccount && _kernelService.TransactionDataCache.ContainsKey("__OLD_ACCOUNT_DATA"))
            {
               string oldAccountData = _kernelService.TransactionDataCache["__OLD_ACCOUNT_DATA"].ToString();
               var oaData = JsonConvert.DeserializeObject<Dictionary<string, string>>(oldAccountData);

               _kernelService.Logger.Info("Page0172: " + oldAccountData);

               List<Purpose> purposes = new List<Purpose>();

               foreach (string key in oaData.Keys)
               {
                  switch (key)
                  {
                     case "SERVE_COMPANY":         
                        {
                           _pageVM.CompanyName = oaData[key];

                           if (!string.IsNullOrEmpty(_pageVM.CompanyName))
                              _pageVM.MajorJobName = "在職中";
                        }                              
                        break;
                     case "JOB_TITLE":
                        {
                           if(!string.IsNullOrEmpty(oaData[key]))
                           {
                              string jobTitleSN = oaData[key];
                              var majorJob = _pageVM.MajorJobs.SingleOrDefault(x => x.SN == jobTitleSN);
                              if (majorJob != null)
                              {
                                 _pageVM.MajorJobName = majorJob.Name;
                                 _pageVM.JobPositionName = majorJob.Name;
                              }
                           }
                           break;
                        }
                     case "JOB_BUSINESS_CODE":        
                        {
                           string val = oaData[key];

                           if (val.Length > 2)
                           {
                              _pageVM.CompanyTypeCode = val.Substring(0, 2);

                              foreach (var mc in _pageVM.IndustryMajorClasses)
                              {
                                 var sc = mc.SubClasses.SingleOrDefault(x => x.MegaCode == val);
                                 if (sc != null)
                                 {
                                    _pageVM.MajorClassName = mc.Name;
                                    _pageVM.SubClassName = sc.Name;

                                    if (oaData.ContainsKey("JOB_TITLE"))
                                    {
                                       string jobTitleSN = oaData["JOB_TITLE"];
                                       if (!string.IsNullOrEmpty(jobTitleSN))
                                       {
                                          _pageVM.MajorJobName = "在職中";

                                          var jobPos = _pageVM.JobPositions.SingleOrDefault(x => x.SN == jobTitleSN);
                                          if (jobPos != null)
                                             _pageVM.JobPositionName = jobPos.Name;
                                       }
                                    }
                                    break;
                                 }
                              }
                           }
                        }
                        break;
                     case "ANNUAL_INCOME":         
                        _pageVM.AnnualIncomeCode = oaData[key];
                        break;
                     case "INDVI_AMT_RANGE":        
                        _pageVM.MonthlyAvgCode = oaData[key];
                        break;
                     default:
                        break;
                  }
               }

               if (_pageVM.PurposeEnabled)
                  _pageVM.PurposeSelectItems = purposes;
            }
            else
            {
               if (bHasOldAccount)
                  _kernelService.Logger.Warn("__OLD_ACCOUNT_DATA is not found!");
            }
         }

         _kernelService.Logger.Info($"MajorJobs.Count = {_pageVM.MajorJobs.Count}");

         _pageVM.ResetTimer();
         _pageVM.StartTimer();
      }

      private void btnPrevious_Click(object sender, RoutedEventArgs e)
      {
         _pageVM.StopTimer();

         var model = new
         {
            _pageVM.MajorJobName,         
            _pageVM.CompanyName,          
            _pageVM.MajorClassName,       
            _pageVM.SubClassName,         
            _pageVM.CompanyTypeCode,      
            _pageVM.JobPositionName,      
            _pageVM.AnnualIncomeCode,     
            _pageVM.MonthlyAvgCode,       
            PurposeCode1 = _pageVM.PurposeSelectItems.Count > 0 ? _pageVM.PurposeSelectItems[0].Code : "",            
            PurposeCode2 = _pageVM.PurposeSelectItems.Count > 1 ? _pageVM.PurposeSelectItems[1].Code : "",            
            PurposeCode3 = _pageVM.PurposeSelectItems.Count > 2 ? _pageVM.PurposeSelectItems[2].Code : "",            
            _pageVM.OtherPurpose,         
            RequireJobData = _pageVM.RequireJobData ? "Y" : "N",
            PurposeEnabled = _pageVM.PurposeEnabled ? "Y" : "N",
            OtherPurposeEnabled = _pageVM.OtherPurposeEnabled ? "Y" : "N",
            _pageVM.WarnResultA
         };

         string json = JsonConvert.SerializeObject(model);
         _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData172, json);

         PageResult result = new PageResult("Previous", KioskDataCacheKey.PersonalData172);
         _kernelService.NextPage(result);
      }

      private void btnConfirm_Click(object sender, RoutedEventArgs e)
      {
         List<string> errorList = new List<string>();
         PageViewUtils.GetErrors(errorList, grdForm);

         if(_pageVM.MajorJobSelected == null && string.IsNullOrEmpty(_pageVM.MajorJobName))
            errorList.Add("就業情形未填寫");

         if (_pageVM.RequireJobData && string.IsNullOrEmpty(_pageVM.CompanyName))
            errorList.Add("服務機構未填寫");

         if (_pageVM.RequireJobData && string.IsNullOrEmpty(_pageVM.CompanyTypeCode))
            errorList.Add("機構性質為必選欄位");

         if (_pageVM.RequireJobData && string.IsNullOrEmpty(_pageVM.MajorClassName))
            errorList.Add("行業主分類為必選欄位");

         if (_pageVM.RequireJobData && string.IsNullOrEmpty(_pageVM.SubClassName))
            errorList.Add("行業子分類為必選欄位");

         if(string.IsNullOrEmpty(_pageVM.AnnualIncomeCode))
            errorList.Add("年收入為必選欄位");

         if (string.IsNullOrEmpty(_pageVM.MonthlyAvgCode))
            errorList.Add("預期月平均交易金額為必選欄位");

         if (_pageVM.RequireJobData && string.IsNullOrEmpty(_pageVM.JobPositionName))
            errorList.Add("職稱為必選欄位");

         if (_pageVM.PurposeSelectItems.Count == 0)
            errorList.Add("開戶目的必須至少需勾選一項");

         if (_pageVM.OtherPurposeEnabled && string.IsNullOrEmpty(_pageVM.OtherPurpose))
            errorList.Add("其他目的未填寫");

         if (errorList.Count > 0)
         {
            ScalingModal sm = new ScalingModal(MainGrid, InnerGrid);
            var popup = new ValidationErrorDialog(errorList.ToArray(), sm);
            sm.Expand(popup, ScalingModalExpandCollapseAnimation.SlideB);
         }
         else
         {
            _pageVM.StopTimer();

            string purposeCode1 = _pageVM.PurposeSelectItems.Count > 0 ? _pageVM.PurposeSelectItems[0].Code : "";
            string purposeCode2 = _pageVM.PurposeSelectItems.Count > 1 ? _pageVM.PurposeSelectItems[1].Code : "";
            string purposeCode3 = _pageVM.PurposeSelectItems.Count > 2 ? _pageVM.PurposeSelectItems[2].Code : "";

            string purpose1Desc = "", purpose2Desc = "", purpose3Desc = "", purposeFullDesc = "";
            if (!string.IsNullOrEmpty(purposeCode1))
            {
               purpose1Desc = _pageVM.PurposeDropdown.Data.Purposes.SingleOrDefault(x => x.Code == purposeCode1).Name;

               if (purposeCode1 == "F")
                  purpose1Desc = purpose1Desc + "(" + _pageVM.OtherPurpose + ")";

               purposeFullDesc = purpose1Desc;
            }

            if (!string.IsNullOrEmpty(purposeCode2))
            {
               purpose2Desc = _pageVM.PurposeDropdown.Data.Purposes.SingleOrDefault(x => x.Code == purposeCode2).Name;

               if(purposeCode2 == "F")
                  purpose2Desc = purpose2Desc +"(" + _pageVM.OtherPurpose + ")";

               if (!string.IsNullOrEmpty(purposeFullDesc))
                  purposeFullDesc = purposeFullDesc + "; " + purpose2Desc;
               else
                  purposeFullDesc = purpose2Desc;
            }

            if (!string.IsNullOrEmpty(purposeCode3))
            {
               purpose3Desc = _pageVM.PurposeDropdown.Data.Purposes.SingleOrDefault(x => x.Code == purposeCode3).Name;

               if (purposeCode3 == "F")
                  purpose3Desc = purpose3Desc + "(" + _pageVM.OtherPurpose + ")";

               if (!string.IsNullOrEmpty(purposeFullDesc))
                  purposeFullDesc = purposeFullDesc + "; " + purpose3Desc;
               else
                  purposeFullDesc = purpose3Desc;
            }

            var model = new
            {
               _pageVM.MajorJobName,                                   
               _pageVM.CompanyName,                                    
               _pageVM.MajorClassName,                                 
               _pageVM.SubClassName,                                   
               _pageVM.CompanyTypeCode,                                
               _pageVM.JobPositionName,                                
               _pageVM.AnnualIncomeCode,                               
               _pageVM.MonthlyAvgCode,                                 
               PurposeCode1 = purposeCode1,                            
               PurposeCode2 = purposeCode2,                            
               PurposeCode3 = purposeCode3,                            
               _pageVM.OtherPurpose,                                   
               RequireJobData = _pageVM.RequireJobData ? "Y" : "N",
               PurposeEnabled = _pageVM.PurposeEnabled ? "Y" : "N",
               OtherPurposeEnabled = _pageVM.OtherPurposeEnabled ? "Y" : "N",
               _pageVM.WarnResultA,

               MajorJobDesc = _pageVM.MajorJobName,
               MajorJobKey = _pageVM.MajorJobSelected != null ?_pageVM.MajorJobSelected.SN : "",
               CompanyTitle = _pageVM.MajorJobName == "在職中" ? _pageVM.CompanyName : "",
               MajorClassTitle = _pageVM.MajorClassName,
               SubClassTitle = _pageVM.SubClassName,
               SubClassMegaCode = _pageVM.SubClassSelected != null ? _pageVM.SubClassSelected.MegaCode : "",
               CompanyTypeName = _pageVM.CompanyTypes.SingleOrDefault(x => x.Code == _pageVM.CompanyTypeCode) != null ?
                  _pageVM.CompanyTypes.SingleOrDefault(x => x.Code == _pageVM.CompanyTypeCode).Name : "",
               JobTitle = _pageVM.JobPositionName,
               JobTitleKey = _pageVM.JobPositionSelected != null ? _pageVM.JobPositionSelected.SN : "",
               JobRiskLvl = _pageVM.JobPositionSelected != null ? _pageVM.JobPositionSelected.RiskLevel : "",
               AnnualIncomeDesc = !string.IsNullOrEmpty(_pageVM.AnnualIncomeCode) ? _pageVM.AnnualIncomeDropdown.Data.AnnualIncomes.SingleOrDefault(x => x.Code == _pageVM.AnnualIncomeCode).Name : "",
               MonthlyAvgDesc = !string.IsNullOrEmpty(_pageVM.MonthlyAvgCode) ? _pageVM.MonthlyAvgDropdown.Data.MonthlyAvgs.SingleOrDefault(x => x.Code == _pageVM.MonthlyAvgCode).Name : "",
               PurposeDesc = purposeFullDesc
            };

            _pageVM.Clear();

            string json = JsonConvert.SerializeObject(model);
            _kernelService.TransactionDataCache.Set(KioskDataCacheKey.PersonalData172, json);

            PageResult result = new PageResult("Confirm", KioskDataCacheKey.PersonalData172);
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

      private void ChosenControl_ItemSelected(object sender, EventArgs e)
      {
         ItemSelectedEventArgs args = e as ItemSelectedEventArgs;
         Purpose purpose = args.DataModel as Purpose;
         if (purpose != null && purpose.Code == "F")  
         {
            _pageVM.OtherPurposeEnabled = args.IsSelected;
            if (!args.IsSelected)
               _pageVM.OtherPurpose = "";
         }
      }

      private void ChosenControl_AllItemRemoved(object sender, EventArgs e)
      {
         _pageVM.OtherPurposeEnabled = false;
         _pageVM.OtherPurpose = "";
      }
   }

   public class CompanyType
   {
      public string Code { get; set; }

      public string Name { get; set; }
   }

   public class Page0172ViewModel : PageViewModel
   {
      private IKernelService _kernelService;

      public Page0172ViewModel(IKernelService kernelService, string pageName)
          : base(kernelService, pageName)
      {
         _kernelService = kernelService;

         JobPositionDropdown = new JobPositionDropdown();
         IndustryDropdown = new IndustryDropdown();
         AnnualIncomeDropdown = new AnnualIncomeDropdown();
         MonthlyAvgDropdown = new MonthlyAvgDropdown();
         PurposeDropdown = new PurposeDropdown();
         MajorJobs = new List<JobPosition>();
         Purposes = new List<Purpose>();

         CompanyTypes = new List<CompanyType>();
         CompanyTypes.Add(new CompanyType() { Code = "01", Name = "民營" });
         CompanyTypes.Add(new CompanyType() { Code = "02", Name = "公營" });

         Clear();
      }

      public JobPositionDropdown JobPositionDropdown
      {
         get { return GetValue(() => JobPositionDropdown); }
         set
         {
            SetValue(() => JobPositionDropdown, value);

            if (JobPositionDropdown.Data != null && JobPositionDropdown.Data.IndustryPositions != null)
            {
               var majorPos = JobPositionDropdown.Data.IndustryPositions.SingleOrDefault(x => x.RiskLevel == "N");
               if (majorPos != null)
               {
                  var majorJobs = new List<JobPosition>();

                  var jp = MajorJobs.SingleOrDefault(x => x.SN == "");
                  if (jp == null)
                     majorJobs.Add(new JobPosition() { Name = "在職中", RiskLevel = "", SN = "" });

                  foreach (var pos in majorPos.JobPositions)
                     majorJobs.Add(pos);

                  foreach(var pos in MajorJobs)
                     _kernelService.Logger.Info($"Position: Name:{pos.Name}");

                  MajorJobs = majorJobs;
               }
               else
               {
                  _kernelService.Logger.Warn("Cannot find indRiskLvl == 'N' position data");
               }

               var industryPos = JobPositionDropdown.Data.IndustryPositions.SingleOrDefault(x => x.RiskLevel == "N");
            }
            else
            {
               _kernelService.Logger.Warn("JobPositionDropdown.Data is null or IndustryPositions is null.");
            }
         }
      }

      public IndustryDropdown IndustryDropdown
      {
         get { return GetValue(() => IndustryDropdown); }
         set
         {
            SetValue(() => IndustryDropdown, value);
         }
      }

      public AnnualIncomeDropdown AnnualIncomeDropdown
      {
         get { return GetValue(() => AnnualIncomeDropdown); }
         set
         {
            SetValue(() => AnnualIncomeDropdown, value);
         }
      }

      public MonthlyAvgDropdown MonthlyAvgDropdown
      {
         get { return GetValue(() => MonthlyAvgDropdown); }
         set
         {
            SetValue(() => MonthlyAvgDropdown, value);
         }
      }

      public PurposeDropdown PurposeDropdown
      {
         get { return GetValue(() => PurposeDropdown); }
         set
         {
            SetValue(() => PurposeDropdown, value);
         }
      }

      public List<JobPosition> MajorJobs
      {
         get { return GetValue(() => MajorJobs); }
         set
         {
            SetValue(() => MajorJobs, value);
         }
      }

      [Required(ErrorMessage = "就業情形為必選欄位")]
      public string MajorJobName
      {
         get { return GetValue(() => MajorJobName); }
         set
         {
            SetValue(() => MajorJobName, value);

            MajorJobSelected = MajorJobs.SingleOrDefault(x => x.Name == value);

            if (value != "在職中")
               CompanyName = "";

            if (MajorJobSelected != null)
            {
               if (MajorJobSelected.SN == "")  
               {
                  RequireJobData = true;
                  if(CompanyTypeCode == "") CompanyTypeCode = "";
                  if(MajorClassName == "") MajorClassName = "";
                  if(SubClassName == "") SubClassName = "";
                  if(JobPositionName == "") JobPositionName = "";
               }
               else
               {
                  CompanyName = "";
                  CompanyTypeCode = "";
                  MajorClassName = "";
                  SubClassName = "";
                  JobPositions = MajorJobs;
                  JobPositionName = value;
                  RequireJobData = false;      
               }
            }
            else
               RequireJobData = false;
         }
      }

      public JobPosition MajorJobSelected
      {
         get { return GetValue(() => MajorJobSelected); }
         set
         {
            SetValue(() => MajorJobSelected, value);
         }
      }

      public bool RequireJobData
      {
         get { return GetValue(() => RequireJobData); }
         set
         {
            SetValue(() => RequireJobData, value);
         }
      }

      public List<CompanyType> CompanyTypes
      {
         get { return GetValue(() => CompanyTypes); }
         set
         {
            SetValue(() => CompanyTypes, value);
         }
      }

      public string CompanyTypeCode
      {
         get { return GetValue(() => CompanyTypeCode); }
         set
         {
            SetValue(() => CompanyTypeCode, value);

            if(IndustryDropdown.Data != null && IndustryDropdown.Data.CompanyTypes != null)
            {
               var induCompType = IndustryDropdown.Data.CompanyTypes.SingleOrDefault(x => x.CompanyType == value);
               if (induCompType != null)
                  IndustryMajorClasses = induCompType.MajorClasses;
            }
         }
      }

      public string CompanyName
      {
         get { return GetValue(() => CompanyName); }
         set
         {
            SetValue(() => CompanyName, value);
         }
      }

      public List<IndustryMajorClass> IndustryMajorClasses
      {
         get { return GetValue(() => IndustryMajorClasses); }
         set
         {
            SetValue(() => IndustryMajorClasses, value);
         }
      }

      public string MajorClassName
      {
         get { return GetValue(() => MajorClassName); }
         set
         {
            SetValue(() => MajorClassName, value);

            var majorClass = IndustryMajorClasses.SingleOrDefault(x => x.Name == value);
            if(majorClass != null)
               IndustrySubClasses = majorClass.SubClasses;
         }
      }

      public List<IndustrySubClass> IndustrySubClasses
      {
         get { return GetValue(() => IndustrySubClasses); }
         set
         {
            SetValue(() => IndustrySubClasses, value);
         }
      }

      public string SubClassName
      {
         get { return GetValue(() => SubClassName); }
         set
         {
            SetValue(() => SubClassName, value);

            var subClass = IndustrySubClasses.SingleOrDefault(x => x.Name == value);
            if (subClass != null)
            {
               SubClassSelected = subClass;

               if (JobPositionDropdown.Data != null && JobPositionDropdown.Data.IndustryPositions != null)
               {
                  var induJobPos = JobPositionDropdown.Data.IndustryPositions.SingleOrDefault(x => x.RiskLevel == subClass.RiskLevel);
                  if (induJobPos != null)
                     JobPositions = induJobPos.JobPositions;
               }
            }
            else
               SubClassSelected = null;
         }
      }

      public IndustrySubClass SubClassSelected
      {
         get { return GetValue(() => SubClassSelected); }
         set
         {
            SetValue(() => SubClassSelected, value);
         }
      }

      public List<JobPosition> JobPositions
      {
         get { return GetValue(() => JobPositions); }
         set
         {
            SetValue(() => JobPositions, value);
         }
      }

      [Required(ErrorMessage = "職稱為必選欄位")]
      public string JobPositionName
      {
         get { return GetValue(() => JobPositionName); }
         set
         {
            SetValue(() => JobPositionName, value);

            JobPositionSelected = JobPositions.SingleOrDefault(x => x.Name == value);
         }
      }

      public JobPosition JobPositionSelected
      {
         get { return GetValue(() => JobPositionSelected); }
         set
         {
            SetValue(() => JobPositionSelected, value);
         }
      }

      [Required(ErrorMessage = "年收入為必選欄位")]
      public string AnnualIncomeCode
      {
         get { return GetValue(() => AnnualIncomeCode); }
         set
         {
            SetValue(() => AnnualIncomeCode, value);
         }
      }

      [Required(ErrorMessage = "預期月平均交易金額為必選欄位")]
      public string MonthlyAvgCode
      {
         get { return GetValue(() => MonthlyAvgCode); }
         set
         {
            SetValue(() => MonthlyAvgCode, value);
         }
      }

      public List<Purpose> Purposes
      {
         get { return GetValue(() => Purposes); }
         set
         {
            SetValue(() => Purposes, value);
         }
      }

      public List<Purpose> PurposeSelectItems
      {
         get { return GetValue(() => PurposeSelectItems); }
         set
         {
            SetValue(() => PurposeSelectItems, value);
         }
      }

      public string OtherPurpose
      {
         get { return GetValue(() => OtherPurpose); }
         set
         {
            SetValue(() => OtherPurpose, value);
         }
      }

      public bool OtherPurposeEnabled
      {
         get { return GetValue(() => OtherPurposeEnabled); }
         set
         {
            SetValue(() => OtherPurposeEnabled, value);
         }
      }

      public bool PurposeEnabled
      {
         get { return GetValue(() => PurposeEnabled); }
         set
         {
            SetValue(() => PurposeEnabled, value);
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

      public void Clear()
      {
         MajorJobs = new List<JobPosition>();
         JobPositions = new List<JobPosition>();
         IndustryMajorClasses = new List<IndustryMajorClass>();
         IndustrySubClasses = new List<IndustrySubClass>();
         PurposeSelectItems = new List<Purpose>();

         MajorJobName = "";
         RequireJobData = false;
         CompanyTypeCode = "";
         CompanyName = "";
         MajorClassName = "";
         SubClassName = "";
         JobPositionName = "";
         AnnualIncomeCode = "";
         MonthlyAvgCode = "";
         OtherPurpose = "";
         OtherPurposeEnabled = false;
         PurposeEnabled = true;
         WarnResultA = "";
      }
   }
}
