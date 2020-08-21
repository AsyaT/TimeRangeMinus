using System;

namespace BentleyOttman
{
    public class MainDicStructure
    {
        public MainDicStructure(Guid? guid, bool isIsOpen, bool isRule)
        {
            this.IsOpen = isIsOpen;
            this.RuleExclusion = isRule;
            this.Guid = guid;
        }
        public Guid? Guid { get; set; }

        public bool IsOpen { get; set; } // Open = true time interval or close = false

        public bool RuleExclusion { get; set; } // Rule = true or Exclusion = false
    }
}
