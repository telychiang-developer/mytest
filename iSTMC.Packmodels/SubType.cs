using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSTMC.Packmodels
{
   public enum SubType1000
   {
      Apply = 0,
      Cancel = 1
   }

   public enum SubType2001
   {
      Begin = 0,
      InProcess = 1,
      BusinessSucceeded = 2,
      BusinessTerminated = 3,
      UserTerminateOrTimeout = 4,
      Error = 99
   }

   public enum SubType2002
   {
      Welcome = 0,
      PIDScan = 10,
      PIDOCR = 11,
      AgreementConfirm = 12,
      TaxClaimConfirm = 13,
      PIDScanFront = 201,
      PIDScanBack = 202,
      ID2Scan = 203,
      ID2TakeBack = 204,
      TakePhoto = 21,
      IDDataConfirm = 22,
      PersonalData1 = 301,
      PasswordInput = 401,
      ApplyDataPass = 410,
      DocumentSign = 421,
      DocumentRetract = 422,
      AccountCreated = 423,
   }

   public enum SubType2004
   {
      SaveOpenType = 1,
      SavePIDFrontVisibeAndOCR = 2,
      SavePIDFrontUV = 3,
      SavePIDFrontTransparent = 4,
      SavePersonalOCRFields = 5,
      SaveAccount = 6,
      SavePreOpenReviewResult = 7,
      SaveAgreementAndFatcaMemo = 8,
      SavePIDBackVisibleAndOCR = 9,
      SavePIDBackUV = 10,
      SaveID2AndOCR = 11,
      SavePhoto = 12,
      SaveTwoIDData = 13,
      SaveManualFillTWAccount = 14,
      SaveMiscData = 15,
      SavePriorCustPIDAndRecord = 16,
      SaveZ21MemoAndResultImage = 17,
      SaveOralInquiry = 18,
      SaveAntiFraudMemo = 19,
      SaveConfirmAboveMemo = 20,
      SaveCancelReason = 21,
      SaveNotFATCA = 22,
      SavePIDFrontErrReason = 23,
      SavePIDBackErrReason = 24,
      SavePassword = 25,
      StampCardScanFile = 26,
      EnglishAddress = 27,
      SaveJointCredit = 28,
      TypeAndInterest = 29,
      negitivePerson = 30
   }

   public enum SubType2006
   {
      NoNeedManualReview = 0,
      PIDFrontPicReview = 1,
      PIDBackPicReview = 4,
      ID2Review = 6,
      PhotoReview = 7,
      IDDataConfirm = 8,
      IDDataReview = 9,
      PersonalDataReview = 10,
      PrintReview = 11,
      ApplySucceedReview = 12,
      RemoteReview = 13,
      PreCheckReject = 14,
      IDCheckReject = 15,
      StatementReview = 16,
      SignedDocReview = 17
   }

   public enum SubType2007
   {
      CancelBusiness = 0,
      IsTakePhoto = 1,
      IsTakePortrait = 4,
      SuspendVNC = 5,
      ResumeVNC = 6,
      PrintDocument = 7,
      PrintChecklist = 9,
      EnhancedCursor = 33,
      PrintEBankUserNotification = 34,
      MobileENotification = 35,
      PrintStampCard = 36,
      ScanStampCard = 37
   }

   public enum SubType2009
   {
      ADLogin = 1,
      WaitBeginWording = 201,
      IntroPutInPIDWording = 202,
      IntroConfirmDataWording = 203,
      IntroConfirmAgreementWording = 204,
      IntroScanPIDWording = 205,
      IntroScanID2Wording = 206,
      IntroTakePhotoWording = 207,
      IntroConfirmTwoIDDataWording = 208,
      IntroFillPersonalDataWording = 209,
      IntroInputPasswordWording = 210,
      CheckCustomerFeedbackWording = 211,
      IntroAntiFraudWording = 212,
      SupervisorReviewResult = 213,
      ChineseToEnglish = 4,
      PhoneticToHant = 5,
      SeldomWordSheet = 6,
      PreAccountOpenReview = 7,
      GetNewAccountData = 8,
      GetAgreement = 9,
      GetAccountForSTMA = 11,
      GetOldAccountData = 12,
      GetIDBackOCRResult = 13,
      AccountOpenReview = 14,
      DepositAccountInquiry = 15,
      RiskAccessment = 16,
      SupervisorReview = 17,
      HostAccountData = 18,
      IDAndPhotoUpload = 19,
      GetAddresses = 22,
      GetStatements = 23,
      GetPreCheckResult = 24,
      GetIndustries = 25,
      GetJobPositions = 26,
      GetAnnualIncomes = 27,
      GetExpectMonthlyAvg = 28,
      GetPurpose = 29,
      GetSourceOfFund = 30,
      GetSourceOfWealthy = 31,
      WarningAccount = 32,
      PDFPrintData = 33,
      DownloadApplicationPDF = 34,
      GetIsReserved = 35,
      GetReservedData = 36
   }
}
