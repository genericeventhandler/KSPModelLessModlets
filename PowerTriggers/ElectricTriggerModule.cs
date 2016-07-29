﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerTriggers
{
    public class ElectricTriggerModule : PartModule
    {
        [KSPEvent(guiActive =true, guiName = "Click Me!", active =true)]
        public void ClickMe()
        {
        }

        public override string GetInfo()
        {
            return "GE ElectricTrigger Module";
        }
    }
}
