using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BentleyOttman
{
    public class BentleyOttmanAlgorithm
    {
        private List<KeyValuePair<(DateTime, int), MainDicStructure>> MainDictionary = new List<KeyValuePair<(DateTime,int), MainDicStructure>>();
        private List<ResultStructure> result = new List<ResultStructure>();

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
            if (rule.GetType() == typeof(Rule))
                isRule = true;
            else if (rule.GetType() == typeof(Exclusion))
                isRule = false;

            if (IsInInterval(rule.Start) || IsInInterval(rule.End))
            {
                MainDictionary.Add(
                    new KeyValuePair<(DateTime, int), MainDicStructure>((rule.Start,rule.Priority), new MainDicStructure(rule.Guid, true, isRule)));
                MainDictionary.Add(
                    new KeyValuePair<(DateTime, int), MainDicStructure>((rule.End, rule.Priority), new MainDicStructure(rule.Guid, false, isRule)));
            }

            if (rule.OffsetUom != TimeMeasure.None && rule.Offset > 0)
            {
                DateTime offset = new DateTime(0);

                offset = offset.CalculateOffset(rule.Offset, rule.OffsetUom);

                while (IsInInterval(rule.Start.AddTicks(offset.Ticks)) || IsInInterval(rule.End.AddTicks(offset.Ticks)))
                {
                    MainDictionary.Add(new KeyValuePair<(DateTime, int), MainDicStructure>(
                        (rule.Start.AddTicks(offset.Ticks), rule.Priority), new MainDicStructure(rule.Guid, true, isRule)));
                    MainDictionary.Add(new KeyValuePair<(DateTime, int), MainDicStructure>(
                        (rule.End.AddTicks(offset.Ticks), rule.Priority), new MainDicStructure(rule.Guid, false, isRule)));

                    offset = offset.CalculateOffset(rule.Offset, rule.OffsetUom);
                }
            }
            
        }

        public List<ResultStructure> GetResult(bool includeCutIntervals = false)
        {
            bool isExclusionInAction = false;
            IList<bool> isRuleInAction = new List<bool>();
            ResultStructure resultCandidate = null;
            Guid? lastOpenGuid = null;
            //Группировка по работе с приоритетом
            foreach(var item in MainDictionary.GroupBy(i => i.Key.Item2))
            { 
                IOrderedEnumerable<KeyValuePair<(DateTime,int), MainDicStructure>> sortedCollection = item.OrderBy(x => x.Key.Item1);
                foreach (KeyValuePair<(DateTime, int), MainDicStructure> timeEvent in sortedCollection)
                {
                    if (timeEvent.Value.RuleExclusion == false) //This is exclusion
                    {
                        if (timeEvent.Value.IsOpen == true) // open
                        {
                            isExclusionInAction = true;

                            if (isRuleInAction.Any() && resultCandidate != null && resultCandidate.StartDateTime.Equals(timeEvent.Key.Item1) == false)
                            {
                                resultCandidate = new ResultStructure() { Guid = resultCandidate.Guid, StartDateTime = resultCandidate.StartDateTime, EndDateTime = timeEvent.Key.Item1 };
                                result.Add(resultCandidate);
                                resultCandidate = null;
                            }
                        }
                        else //close
                        {
                            isExclusionInAction = false;
                            if (isRuleInAction.Any())
                            {
                                Guid newGuid = lastOpenGuid ?? new Guid();
                                resultCandidate = new ResultStructure() { Guid = newGuid, StartDateTime = timeEvent.Key.Item1, EndDateTime = new DateTime(0) };
                                lastOpenGuid = null;
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
                                lastOpenGuid = timeEvent.Value.Guid;
                                resultCandidate = new ResultStructure() { Guid = timeEvent.Value.Guid, StartDateTime = timeEvent.Key.Item1, EndDateTime = new DateTime(0) };
                            }
                        }
                        else //close
                        {
                            isRuleInAction.Remove(true);
                            if (isExclusionInAction)
                            {
                                resultCandidate = null;
                            }
                            else if (resultCandidate != null && resultCandidate.StartDateTime.Equals(timeEvent.Key) == false && isRuleInAction.Count == 0)
                            {
                                resultCandidate = new ResultStructure() { Guid = resultCandidate.Guid, StartDateTime = resultCandidate.StartDateTime, EndDateTime = timeEvent.Key.Item1 };
                                result.Add(resultCandidate);
                                resultCandidate = null;
                            }
                        }
                    }
                }
            }
           
            
            

            if (StartDateTime.HasValue)
            {
                if(includeCutIntervals)
                {
                    result = result.Where(x => DateTime.Compare(StartDateTime.Value, x.EndDateTime) <= 0 ).ToList();
                }
                else
                {
                    result = result.Where(x => DateTime.Compare(StartDateTime.Value, x.EndDateTime) <= 0 ).ToList();
                }
            }

            return result;
        }
    }
}
