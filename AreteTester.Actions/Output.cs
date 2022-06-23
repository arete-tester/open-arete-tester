using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AreteTester.Actions
{
    public class Output
    {
        public Output()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DoNotLog = false;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ActionType { get; set; }

        public string Description { get; set; }

        public string Message { get; set; }

        public bool IsAssertion { get; set; }

        public bool Success { get; set; }

        public string Expected { get; set; }

        public string Actual { get; set; }

        public bool DoNotLog { get; set; }
    }
}
