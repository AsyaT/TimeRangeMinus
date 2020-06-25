using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BentleyOttman
{
    public class BentleyOttmanAlgorithm
    {
        private List<KeyValuePair<DateTime, MainDicStructure>> MainDictionary = new List<KeyValuePair<DateTime, MainDicStructure>>();
        private List<Tuple<DateTime, DateTime>> result = new List<Tuple<DateTime, DateTime>>();

        private readonly DateTime? StartDateTime;
        private readonly DateTime? EndDateTime;

        public BentleyOttmanAlgorithm(DateTime? minStartDateTime, DateTime? maxEndDateTime)
        {
            StartDateTime = minStartDateTime;
            EndDateTime = maxEndDateTime;
        }

        private bool IsInInterval(DateTime dateTimeEvent)
        {
            if( EndDateTime.HasValue)
            {
                return DateTime.Compare(dateTimeEvent, EndDateTime.Value) <= 0;
            }

            return true;
        }

        

        public void AddRule(IBaseRule rule)
        {
            bool isRule = true;
            if (rule.GetType() == typeof(RepairRule))
                isRule = true;
            else if (rule.GetType() == typeof(RepairExclusion))
                isRule = false;

            if (IsInInterval(rule.Start) || IsInInterval(rule.End))
            {

                MainDictionary.Add(
                    new KeyValuePair<DateTime, MainDicStructure>(rule.Start, new MainDicStructure(true, isRule)));
                MainDictionary.Add(
                    new KeyValuePair<DateTime, MainDicStructure>(rule.End, new MainDicStructure(false, isRule)));
            }

            if (rule.OffsetUom != TimeMeasure.None && rule.Offset > 0)
            {
                DateTime offset = new DateTime(0);

                offset = offset.CalculateOffset(rule.Offset, rule.OffsetUom);

                while (IsInInterval(rule.Start.AddTicks(offset.Ticks)) || IsInInterval(rule.End.AddTicks(offset.Ticks)))
                {
                    MainDictionary.Add(new KeyValuePair<DateTime, MainDicStructure>(
                        rule.Start.AddTicks(offset.Ticks), new MainDicStructure(true, isRule)));
                    MainDictionary.Add(new KeyValuePair<DateTime, MainDicStructure>(
                        rule.End.AddTicks(offset.Ticks), new MainDicStructure(false, isRule)));

                    offset = offset.CalculateOffset(rule.Offset, rule.OffsetUom);
                }
            }
            
        }

        public List<Tuple<DateTime, DateTime>> GetResult(bool includeCutIntervals = false)
        {
            bool isExclusionInAction = false;
            IList<bool> isRuleInAction = new List<bool>();
            Tuple<DateTime, DateTime> resultCandidate = null;

            IOrderedEnumerable<KeyValuePair<DateTime, MainDicStructure>> sortedCollection = MainDictionary.OrderBy(x => x.Key);
            
            foreach (KeyValuePair<DateTime, MainDicStructure> timeEvent in sortedCollection)
            {
                if (timeEvent.Value.RuleExclusion == false) //This is exclusion
                {
                    if (timeEvent.Value.IsOpen == true) // open
                    {
                        isExclusionInAction = true;

                        if (isRuleInAction.Any() && resultCandidate!=null && resultCandidate.Item1.Equals(timeEvent.Key) == false)
                        {
                            resultCandidate = new Tuple<DateTime, DateTime>(resultCandidate.Item1, timeEvent.Key);
                            result.Add(resultCandidate);
                            resultCandidate = null;
                        }
                    }
                    else //close
                    {
                        isExclusionInAction = false;
                        if (isRuleInAction.Any())
                        {
                            resultCandidate = new Tuple<DateTime, DateTime>(timeEvent.Key, new DateTime(0));
                        }
                    }

                }
                else if (timeEvent.Value.RuleExclusion == true) //This is rule
                {
                    if (timeEvent.Value.IsOpen == true) //open
                    {
                        isRuleInAction.Add(true);

                        if (isExclusionInAction == false && isRuleInAction.Count == 1)
                        {
                            resultCandidate = new Tuple<DateTime, DateTime>(timeEvent.Key, new DateTime(0));
                        }
                    }
                    else //close
                    {
                        isRuleInAction.Remove(true);
                        if (isExclusionInAction)
                        {
                            resultCandidate = null;
                        }
                        else if(resultCandidate !=null && resultCandidate.Item1.Equals(timeEvent.Key) == false && isRuleInAction.Count == 0)
                        {
                            resultCandidate = new Tuple<DateTime, DateTime>(resultCandidate.Item1, timeEvent.Key);
                            result.Add(resultCandidate);
                            resultCandidate = null;
                        }
                    }
                }
            }

            if (StartDateTime.HasValue)
            {
                if(includeCutIntervals)
                {
                    result = result.Where(x => DateTime.Compare(StartDateTime.Value, x.Item2) <=0 ).ToList();
                }
                else
                {
                    result = result.Where(x => DateTime.Compare(StartDateTime.Value, x.Item1) <= 0 ).ToList();
                }
            }

            return result;
        }
    }
}
