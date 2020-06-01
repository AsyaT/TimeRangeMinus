using System;
using System.Collections.Generic;
using System.Linq;

namespace BentleyOttman
{
    public class BentleyOttmanAlgorithm
    {
        private List<KeyValuePair<DateTime, MainDicStructure>> MainDictionary = new List<KeyValuePair<DateTime, MainDicStructure>>();
        private List<Tuple<DateTime, DateTime>> result = new List<Tuple<DateTime, DateTime>>();

        private readonly long MaxOffsetTicks;

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

            MainDictionary.Add(new KeyValuePair<DateTime, MainDicStructure>(rule.Start, new MainDicStructure(true, isRule)));
            MainDictionary.Add(new KeyValuePair<DateTime, MainDicStructure>(rule.End, new MainDicStructure(false, isRule)));

            if (rule.OffsetUom != TimeMeasure.None && rule.Offset > 0)
            {
                DateTime offset = new DateTime(0);

                while (offset.Ticks <= MaxOffsetTicks)
                {
                    switch (rule.OffsetUom)
                    {
                        case TimeMeasure.Minutes:
                            offset = offset.AddMinutes(rule.Offset);
                            break;
                        case TimeMeasure.Days:
                            offset = offset.AddDays(rule.Offset);
                            break;
                        case TimeMeasure.Hours:
                            offset = offset.AddHours(rule.Offset);
                            break;
                        default: break;
                    }

                    MainDictionary.Add(new KeyValuePair<DateTime, MainDicStructure>(
                        rule.Start.AddTicks(offset.Ticks), new MainDicStructure(true, isRule)));
                    MainDictionary.Add(new KeyValuePair<DateTime, MainDicStructure>(
                        rule.End.AddTicks(offset.Ticks), new MainDicStructure(false, isRule)));
                }
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
