using iSTMC.Common;
using iSTMC.Kernel;
using iSTMC.MousePointerProtocol;
using iSTMC.Packmodels;
using iSTMC.PDFPrinter;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unity;
using Unity.ServiceLocation;

namespace iSTMC.Business
{
   public class BusinessServiceContext : KernelServiceBase
   {
      protected PageViewConfig PageViewConfig { get; private set; }

      private PageViewConfig GetPageViewConfig()
      {
         string asmdir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         string exedir = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(asmdir));

         string pageviewcfgfile = System.IO.Path.Combine(exedir, @"Config\PageViewConfig.xml");

         if (!System.IO.File.Exists(pageviewcfgfile))
            return null;

         PageViewConfig config = ConfigSerializer<PageViewConfig>.ReadFile(pageviewcfgfile);

         return config;
      }

      public BusinessServiceContext()
      {
         PageViewConfig = GetPageViewConfig();
      }

      protected override void OnHostConnected()
      {
         base.OnHostConnected();
      }

      protected override void OnHostDisconnected(string msg)
      {
         base.OnHostDisconnected(msg);
      }

      protected override void OnTimeoutMessage(string json)
      {
         base.OnTimeoutMessage(json);
      }

      protected override void OnError(Exception ex)
      {
         base.OnError(ex);
      }

      protected override void OnHostMessage(string msg)
      {
         base.OnHostMessage(msg);

         try
         {
            BusinessRep rep = BusinessRep.FromJson(msg);
            this.TransactionDataCache.Set("__Response", rep);
         }
         catch (Exception ex)
         {
            OnError(ex);
         }
      }

      protected override void OnHostNotification(string notifyName, string json)
      {
         base.OnHostNotification(notifyName, json);

         try
         {
            BusinessReq req = BusinessReq.FromJson(json);
            if (req != null)
            {
               if (req.Head.MessageType == "2007")
               {
                  if (req.Head.SubType == "0")  
                  {
                     this.CurrentPageName = "Home";

                     if (this.BusinessWorkflowApp != null)
                        this.BusinessWorkflowApp.Cancel();
                  }
                  else if (req.Head.SubType == "7" || req.Head.SubType == "9")   
                  {
                     var serviceLocator = CommonServiceLocator.ServiceLocator.Current;
                     if (serviceLocator != null)
                     {
                        var pdfPrinter = serviceLocator.GetInstance<IPDFPrinter>();
                        if (pdfPrinter != null)
                        {
                           pdfPrinter.Logger = Logger;

                           string asmdir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                           string businessdir = System.IO.Path.GetDirectoryName(asmdir);
                           string destDir = System.IO.Path.Combine(businessdir, "Outputs");
                           string fullpath = System.IO.Path.Combine(destDir, $"PDF_{req.Head.SubType}.pdf");

                           if (System.IO.File.Exists(fullpath))
                           {
                              Logger.Info($"PDF file has found and send to printer..., Path: {fullpath}");
                              pdfPrinter.PrintPDF(fullpath);
                              Logger.Info($"PDF file has sent to printer.");
                           }
                           else
                              Logger.Error($"PDF file is not found. Path: {fullpath}");
                        }
                        else
                           Logger.Error($"PDFPrinter service load failed.");
                     }
                  }
                  else if (req.Head.SubType == "33")  
                  {
                     var serviceLocator = CommonServiceLocator.ServiceLocator.Current;
                     if (serviceLocator != null)
                     {
                        var mousePointer = serviceLocator.GetInstance<IMousePointer>();
                        if (mousePointer != null)
                        {
                           mousePointer.Logger = Logger;

                           string asmdir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                           string businessdir = System.IO.Path.GetDirectoryName(asmdir);
                           string exedir = System.IO.Path.GetDirectoryName(businessdir);

                           Process[] procs = Process.GetProcessesByName("MousePointer");
                           if (procs == null || procs.Length == 0)
                           {
                              string exePath = System.IO.Path.Combine(exedir, @"Library\MousePointer\MousePointer.exe");
                              mousePointer.Start(exePath);
                           }
                           else
                           {
                              string exePath = System.IO.Path.Combine(exedir, @"Library\MousePointer\MousePointerRelease.exe");
                              mousePointer.Stop(exePath);
                           }
                        }
                     }
                  }
                  else
                  {
                     Logger.Warn($"Received an unsupported {req.Head.MessageType} command with SubType = {req.Head.SubType}");
                  }
               }
            }
         }
         catch (Exception ex)
         {
            OnError(ex);
         }
      }

      protected override bool InnerInitialize()
      {
         return base.InnerInitialize();
      }

      protected override bool InnerLogin(string caller, string id)
      {
         return base.InnerLogin(caller, id);
      }

      protected override bool InnerLogout()
      {
         Logger.Info("Logout to AP Server.");

         return base.InnerLogout();
      }

      protected override KernelServiceReturnCode InnerStart(string json)
      {
         Logger.Info("Checking if needs to start/resume service...");

         return base.InnerStart(json);
      }

      protected override KernelServiceReturnCode InnerStop(string json)
      {
         Logger.Info("Checking if needs to stop service...");

         return base.InnerStop(json);
      }

      protected override KernelServiceReturnCode InnerOpModeOn(string json)
      {
         Logger.Info("Checking if needs to switch to OpMode On...");

         return base.InnerOpModeOn(json);
      }

      protected override KernelServiceReturnCode InnerOpModeOff(string json)
      {
         Logger.Info("Checking if needs to switch to OpMode Off...");

         return base.InnerOpModeOff(json);
      }

      protected override KernelServiceReturnCode InnerReboot(string json)
      {
         Logger.Info("Checking if needs to Reboot...");

         return base.InnerReboot(json);
      }

      protected override KernelServiceReturnCode InnerShutdown(string json)
      {
         Logger.Info("Checking if needs to Shutdown...");

         return base.InnerShutdown(json);
      }

      protected override KernelServiceReturnCode InnerStatusReport(string json)
      {
         Logger.Info("Checking if needs to StatusReport...");

         return base.InnerStatusReport(json);
      }

      protected override KernelServiceReturnCode InnerVersionUpgrade(string json)
      {
         Logger.Info("Checking if needs to VersionUpgrade...");

         return base.InnerVersionUpgrade(json);
      }

      protected override KernelServiceReturnCode InnerHostCommand(string json)
      {
         Logger.Info("Kernel is asked to execute HostCommand...");

         return base.InnerHostCommand(json);
      }

      public override int GetPageTimeout(string pageName)
      {
         if (PageViewConfig == null)
         {
            Logger.Warn($"Page configuration of Page {pageName} was not found and using default 0.");
            return 0;
         }

         var pageCfg = PageViewConfig.PageConfigs.SingleOrDefault(p => p.PageName == pageName);

         if (pageCfg == null)
            Logger.Warn($"Page configuration of Page {pageName} was not found and using default 180.");
         else
            Logger.Info($"Page {pageName} timeout is {pageCfg.Timeout} seconds");

         return pageCfg != null ? pageCfg.Timeout : 180;
      }
   }
}
