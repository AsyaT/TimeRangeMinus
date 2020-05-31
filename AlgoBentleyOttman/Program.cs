using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AlgoBentleyOttman
{
    class Program
    {
        static void Main(string[] args)
        {
            RepairRule rule1 = new RepairRule(new DateTime(2020,6,1, 9,0,0), new DateTime(2020,6,1,17,0,0), 1, "день");
            RepairRule rule2 = new RepairRule(new DateTime(2020,6,1, 6,0,0), new DateTime(2020,6,1,7,0,0), 1, "день");
            RepairExclusion exclusion1 = new RepairExclusion(new DateTime(2020,6,6, 0,0,0), new DateTime(2020,6,8,0,0,0), 7, "день");
            RepairExclusion exclusion2 = new RepairExclusion(new DateTime(2020,6,2, 13,0,0), new DateTime(2020,6,2,14,0,0), 7, "день");

            List<Tuple<DateTime,DateTime>> result = new List<Tuple<DateTime, DateTime>>();

            Dictionary<DateTime, MainDicStructure> MainDictionary = new Dictionary<DateTime, MainDicStructure>();

            MainDictionary.AddInterval(rule1);
            MainDictionary.AddInterval(rule2);
            MainDictionary.AddInterval(exclusion1);
            MainDictionary.AddInterval(exclusion2);

            bool isExclusionInAction = false;
            bool isRuleInAction = false;
            Tuple <DateTime,DateTime > resultCandidate = null;

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
            

            Console.WriteLine("Hello World!");
        }
    }
}
