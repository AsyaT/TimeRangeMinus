using System;
using BentleyOttman;

namespace AlgoBentleyOttman
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start program: "+ DateTime.Now.Minute+":" + DateTime.Now.Second+ ":"+DateTime.Now.Millisecond);

            RepairRule rule1 = new RepairRule(new DateTime(2020,6,1, 9,0,0), new DateTime(2020,6,1,17,0,0), 1, "день");
            RepairRule rule2 = new RepairRule(new DateTime(2020,6,1, 6,0,0), new DateTime(2020,6,1,7,0,0), 1, "день");
            RepairExclusion exclusion1 = new RepairExclusion(new DateTime(2020,6,6, 0,0,0), new DateTime(2020,6,8,0,0,0), 7, "день");
            RepairExclusion exclusion2 = new RepairExclusion(new DateTime(2020,6,2, 13,0,0), new DateTime(2020,6,2,14,0,0), 7, "день");

            Console.WriteLine("Before loop: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(0).AddYears(1).Ticks);
             algo.AddRule(rule1);
             algo.AddRule(rule2);
             algo.AddRule(exclusion1);
             algo.AddRule(exclusion2);
             var result = algo.GetResult();
             
            Console.WriteLine("After loop: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
            Console.WriteLine("Finish!");
        }
    }
}
