using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSTMC.Packmodels
{
   public enum MessageType
   {
      ConnectSendCancel = 1000,
      DeliveryCode = 1003,
      TranBeginEnd = 2001,
      StepInfo = 2002,
      IdentityData = 2004,
      InfoCheck = 2006,
      ActionNotify = 2007,
      HostData = 2009
   }
}
