using CommonServiceLocator;
using NLog;
using iSTMC.Common;
using iSTMC.Kernel;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.ServiceLocation;

namespace iSTMC.Activity
{
   [Description("該類別為基礎類別，只能繼承不可使用")]
   public class KioskCodeActivityBase : CodeActivity
   {
      [Description("註冊型別建立容器")]
      public IUnityContainer Container { get; private set; }
      [Description("系統暫存快取物件")]
      public Dictionary<string, object> TerminalDataCache { get; private set; }
      [Description("交易暫存快取物件(交易結束就會被清空)")]
      public Dictionary<string, object> TransactionDataCache { get; private set; }
      [Description("設備暫存快取物件(程式重啟就會被清空)")]
      public TerminalConfig TerminalConfig { get; private set; }
      [Description("核心服務")]
      public IKernelService KernelService { get; private set; }

      public KioskCodeActivityBase() : base() { }

      protected IServiceLocator ServiceLocator { get { return CommonServiceLocator.ServiceLocator.Current; } }

      protected ILogger Logger { get; private set; }

      protected override void Execute(CodeActivityContext context)
      {
         Logger = (ILogger)context.GetExtension<ILogger>();

         KernelService = (IKernelService)context.GetExtension<IKernelService>();

         TerminalDataCache = KernelService.TerminalDataCache;
         TransactionDataCache = KernelService.TransactionDataCache;

         WorkflowDataContext dataContext = context.DataContext;
         PropertyDescriptorCollection propertyDescriptorCollection = dataContext.GetProperties();
         foreach (PropertyDescriptor propertyDesc in propertyDescriptorCollection)
         {
            if (propertyDesc.Name == "TerminalConfig")
            {
               TerminalConfig = propertyDesc.GetValue(dataContext) as TerminalConfig;
            }
         }
      }
   }
}
