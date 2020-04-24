using iSTMC.Common;
using iSTMC.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace iSTMC.PageView
{
   public class PageViewModel : PropertyChangedNotification
   {
      private string _pageName;
      private DispatcherTimer _timer;
      private readonly IKernelService _kernelService;

      private int _defaultTimeout = 0;

      public PageViewModel(IKernelService kernelService, string pageName)
      {
         _kernelService = kernelService;

         _pageName = pageName;
         _defaultTimeout = _kernelService.GetPageTimeout(pageName);
         Timeout = _defaultTimeout;

         _timer = new DispatcherTimer(DispatcherPriority.Normal)
         {
            Interval = TimeSpan.FromSeconds(1)
         };

         _timer.Tick += (s, e) =>
         {
            if (Timeout > 0)
               Timeout--;
            else
            {
               _timer.Stop();

               _kernelService.Logger.Info($"{pageName} is timeout and go back to Home page...");

               PageResult result = new PageResult("Home");
               _kernelService.NextPage(result);
            }
         };
      }

      public int Timeout
      {
         get { return GetValue(() => Timeout); }
         set { SetValue(() => Timeout, value); }
      }

      public void StartTimer()
      {
         if (Timeout > 0)
         {
            _timer.Start();
            _kernelService.Logger.Info($"{_pageName} timer is started.");
         }
      }

      public void StopTimer()
      {
         if (Timeout > 0)
         {
            _timer.Stop();
            _kernelService.Logger.Info($"{_pageName} timer is stopped.");
         }
      }

      public void ResetTimer()
      {
         if (_timer.IsEnabled)
            _timer.Stop();

         Timeout = _defaultTimeout;

         _kernelService.Logger.Info($"{_pageName} timer has been reset.");
      }
   }
}
