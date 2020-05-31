using System;
using System.Collections.Generic;
using System.Linq;
using AlgoBentleyOttman;

namespace BentleyOttman
{
    public class BentleyOttmanAlgorithm
    {
        Dictionary<DateTime, MainDicStructure> MainDictionary = new Dictionary<DateTime, MainDicStructure>();
        List<Tuple<DateTime, DateTime>> result = new List<Tuple<DateTime, DateTime>>();

        public void AddRule(IBaseRule rule)
        {
            MainDictionary.AddInterval(rule);
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

                        if (isRuleInAction)
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
                        resultCandidate = new Tuple<DateTime, DateTime>(timeEvent.Key, new DateTime(0));
                    }
                    else //close
                    {
                        isRuleInAction = false;
                        if (isExclusionInAction)
                        {
                            resultCandidate = null;
                        }
                        else
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
