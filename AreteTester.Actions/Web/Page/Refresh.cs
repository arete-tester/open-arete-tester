using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace AreteTester.Actions
{
    [Serializable]
    public class Refresh : ActionBase
    {

        public Refresh()
        {
            this.ActionType = "Refresh";
            this.Description = "Refresh web page";
        }

        public override void  Process()
        {
            base.Process();

            Globals.Driver.Navigate().Refresh();
        }
    }
}
