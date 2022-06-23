using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace AreteTester.Actions
{
    [Serializable]
    public class Wait : ActionBase
    {
        [XmlAttribute]
        [Category("Properties")]
        public int Duration { get; set; }

        public Wait()
        {
            this.ActionType = "Wait";

            this.Duration = 3;
        }

        public override void  Process()
        {
            base.Process();

            System.Threading.Thread.Sleep(this.Duration * 1000);
        }
    }
}
