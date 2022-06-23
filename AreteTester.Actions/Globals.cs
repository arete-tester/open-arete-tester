using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace AreteTester.Actions
{
    public class Globals
    {
        public static IWebDriver Driver { get; set; }

        public static DateTime RunnerDateTime { get; set; }

        public static RunnerStatusType RunnerStatus { get; set; }

        public static ActionBase PausedAction { get; set; }

        public static List<ActionBase> BreakpointActionsToIgnore { get; set; }

        public static Project CurrentProject { get; set; }
    }
}
