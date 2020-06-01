using System;
using System.Collections.Generic;
using System.Linq;
using AlgoBentleyOttman;

namespace BentleyOttman
{
    public class BentleyOttmanAlgorithm
    {
        private Dictionary<DateTime, MainDicStructure> MainDictionary = new Dictionary<DateTime, MainDicStructure>();
        private List<Tuple<DateTime, DateTime>> result = new List<Tuple<DateTime, DateTime>>();

        private long MaxOffsetTicks;

        public BentleyOttmanAlgorithm(long ticks)
        {
            MaxOffsetTicks = ticks;
        }

        public void AddRule(IBaseRule rule)
        {
            bool isRule = true;
            if (rule.GetType() == typeof(RepairRule))
                isRule = true;
            else if (rule.GetType() == typeof(RepairExclusion))
                isRule = false;

            MainDictionary.Add(rule.Start, new MainDicStructure(true, isRule));
            MainDictionary.Add(rule.End, new MainDicStructure(false, isRule));

            DateTime offset = new DateTime(0);

            while (offset.Ticks <= MaxOffsetTicks)
            {
                switch (rule.OffsetUom.ToLower())
                {
                    case "минут" :
                        offset = offset.AddMinutes(rule.Offset);
                        break;
                    case "дней":
                        offset = offset.AddDays(rule.Offset);
                        break;
                    case "часов":
                        offset = offset.AddHours(rule.Offset);
                        break;
                }

                MainDictionary.Add(rule.Start.AddTicks(offset.Ticks), new MainDicStructure(true, isRule));
                MainDictionary.Add(rule.End.AddTicks(offset.Ticks), new MainDicStructure(false, isRule));
            }
        }

        public List<Tuple<DateTime, DateTime>> GetResult()
        {
            bool isExclusionInAction = false;
            bool isRuleInAction = false;
            Tuple<DateTime, DateTime> resultCandidate = null;

            foreach (KeyValuePair<DateTime, MainDicStructure> timeEvent in MainDictionary.OrderBy(x => x.Key))
            {
                if (timeEvent.Value.RuleExclusion == false) //This is exclusion
                {
                    if (timeEvent.Value.IsOpen == true) // open
                    {
                        isExclusionInAction = true;

                        if (isRuleInAction && resultCandidate!=null && resultCandidate.Item1.Equals(timeEvent.Key) == false)
                        {
                            resultCandidate = new Tuple<DateTime, DateTime>(resultCandidate.Item1, timeEvent.Key);
                            result.Add(resultCandidate);
                            resultCandidate = null;
                        }
                    }
                    else //close
                    {
                        isExclusionInAction = false;
                        if (isRuleInAction)
                        {
                            resultCandidate = new Tuple<DateTime, DateTime>(timeEvent.Key, new DateTime(0));
                        }
                    }

                }
                else if (timeEvent.Value.RuleExclusion == true) //This is rule
                {
                    if (timeEvent.Value.IsOpen == true) //open
                    {
                        isRuleInAction = true;

                        if (isExclusionInAction == false)
                        {
                            resultCandidate = new Tuple<DateTime, DateTime>(timeEvent.Key, new DateTime(0));
                        }
                    }
                    else //close
                    {
                        isRuleInAction = false;
                        if (isExclusionInAction)
                        {
                            resultCandidate = null;
                        }
                        else if(resultCandidate !=null && resultCandidate.Item1.Equals(timeEvent.Key) == false)
                        {
                            resultCandidate = new Tuple<DateTime, DateTime>(resultCandidate.Item1, timeEvent.Key);
                            result.Add(resultCandidate);
                            resultCandidate = null;
                        }
                    }
                }
            }

            return result;
        }
    }
}
