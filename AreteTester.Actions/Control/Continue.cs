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
    public class Continue : ActionBase
    {
        public Continue()
        {
            this.ActionType = "Continue";
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
