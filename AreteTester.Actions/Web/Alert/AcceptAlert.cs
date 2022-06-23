using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AreteTester.Actions
{
    [Serializable]
    public class AcceptAlert : ActionBase
    {
        public AcceptAlert()
        {
            this.ActionType = "AcceptAlert";
        }

        public override void Process()
        {
            base.Process();

            Globals.Driver.SwitchTo().Alert().Accept();

            NotifyOutput(new Output()
            {
                ActionType = this.ActionType,
                Description = this.Description,
                IsAssertion = false,
                DoNotLog = false
            });
        }
    }
}