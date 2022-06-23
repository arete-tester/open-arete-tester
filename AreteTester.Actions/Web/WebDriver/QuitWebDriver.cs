using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AreteTester.Actions
{
    [Serializable]
    public class QuitWebDriver : ActionBase
    {
        public QuitWebDriver()
        {
            this.ActionType = "QuitWebDriver";
        }

        public override void  Process()
        {
            base.Process();

            Globals.Driver.Quit();

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
