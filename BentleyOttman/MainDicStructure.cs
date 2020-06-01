namespace BentleyOttman
{
    public class MainDicStructure
    {
        public MainDicStructure(bool isIsOpen, bool isRule)
        {
            this.IsOpen = isIsOpen;
            this.RuleExclusion = isRule;
        }
        public bool IsOpen { get; set; } // Open = true time interval or close = false

        public bool RuleExclusion { get; set; } // Rule = true or Exclusion = false
    }
}
