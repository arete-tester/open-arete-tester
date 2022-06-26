using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using OpenQA.Selenium;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;

namespace AreteTester.Actions
{
    [Serializable]
    public class Break : ActionBase
    {
        public Break()
        {
            this.ActionType = "Break";
        }

        public override void Process()
        {
            base.Process();
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = base.Validate();

            return result;
        }
    }
}
