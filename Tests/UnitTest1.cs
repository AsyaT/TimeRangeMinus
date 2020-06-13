using System;
using BentleyOttman;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void OnePeriodicalRule()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3,23,59,59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            algo.AddRule(rule1);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new Tuple<DateTime, DateTime> ( 
                new DateTime(2020,8,1, 9, 0, 0), 
                new DateTime(2020, 8, 1, 17, 0, 0) ),result[0]);
            Assert.AreEqual(new Tuple<DateTime, DateTime> ( 
                new DateTime(2020,8,2, 9, 0, 0), 
                new DateTime(2020, 8, 2, 17, 0, 0) ),result[1]);
            Assert.AreEqual(new Tuple<DateTime, DateTime> ( 
                new DateTime(2020,8,3, 9, 0, 0), 
                new DateTime(2020, 8, 3, 17, 0, 0) ),result[2]);
        }

        [Test]
        public void OneNonPeriodicalRule()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 8, 2, 9, 0, 0),
                new DateTime(2020, 8, 2, 17, 0, 0),
                0,
                TimeMeasure.None);
            RepairRule rule2 = new RepairRule(
                new DateTime(2020, 7, 2, 9, 0, 0),
                new DateTime(2020, 7, 2, 17, 0, 0),
                1,
                TimeMeasure.None);
            RepairRule rule3 = new RepairRule(
                new DateTime(2020, 7, 2, 9, 0, 0),
                new DateTime(2020, 7, 2, 17, 0, 0),
                0,
                TimeMeasure.Days);
            algo.AddRule(rule1);
            algo.AddRule(rule2);
            algo.AddRule(rule3);

            var result = algo.GetResult();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 2, 9, 0, 0),
                new DateTime(2020, 8, 2, 17, 0, 0)), result[0]);
        }

        [Test]
        public void RuleAndException_1()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            RepairExclusion exclusion1 = new RepairExclusion(
                new DateTime(2020, 8, 2, 0, 0, 0),
                new DateTime(2020, 8, 3, 0, 0, 0),
                0,
                TimeMeasure.None
                );

            algo.AddRule(rule1);
            algo.AddRule(exclusion1);

            var result = algo.GetResult();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void RuleAndException_2()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            RepairExclusion exclusion1 = new RepairExclusion(
                new DateTime(2020, 8, 2, 13, 0, 0),
                new DateTime(2020, 8, 2, 14, 0, 0),
                0,
                TimeMeasure.None
            );

            algo.AddRule(rule1);
            algo.AddRule(exclusion1);

            var result = algo.GetResult();
            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public void RuleAndException_3()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            RepairExclusion exclusion1 = new RepairExclusion(
                new DateTime(2020, 6, 2, 8, 0, 0),
                new DateTime(2020, 6, 2, 14, 0, 0),
                0,
                TimeMeasure.None
            );

            algo.AddRule(rule1);
            algo.AddRule(exclusion1);

            var result = algo.GetResult();
            Assert.AreEqual(3, result.Count);
        }

        public void RuleAndException_4()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            RepairExclusion exclusion1 = new RepairExclusion(
                new DateTime(2020, 6, 2, 16, 0, 0),
                new DateTime(2020, 6, 2, 20, 0, 0),
                0,
                TimeMeasure.None
            );

            algo.AddRule(rule1);
            algo.AddRule(exclusion1);

            var result = algo.GetResult();
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void TwoNonIntersecRules()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);
            RepairRule rule2 = new RepairRule(
                new DateTime(2020, 6, 1, 18, 0, 0),
                new DateTime(2020, 6, 1, 20, 0, 0),
                1,
                TimeMeasure.Days);

            algo.AddRule(rule1);
            algo.AddRule(rule2);

            var result = algo.GetResult();

            Assert.AreEqual(6, result.Count);
        }

        [Test]
        public void TwoIntersecRules()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);
            RepairRule rule2 = new RepairRule(
                new DateTime(2020, 6, 1, 12, 0, 0),
                new DateTime(2020, 6, 1, 13, 0, 0),
                1,
                TimeMeasure.Days);

            algo.AddRule(rule1);
            algo.AddRule(rule2);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 1, 9, 0, 0),
                new DateTime(2020, 8, 1, 17, 0, 0)), result[0]);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 2, 9, 0, 0),
                new DateTime(2020, 8, 2, 17, 0, 0)), result[1]);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 3, 9, 0, 0),
                new DateTime(2020, 8, 3, 17, 0, 0)), result[2]);
        }

        [Test]
        public void TwoIntersecRules_2()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);
            RepairRule rule2 = new RepairRule(
                new DateTime(2020, 6, 1, 16, 0, 0),
                new DateTime(2020, 6, 1, 18, 0, 0),
                1,
                TimeMeasure.Days);

            algo.AddRule(rule1);
            algo.AddRule(rule2);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 1, 9, 0, 0),
                new DateTime(2020, 8, 1, 18, 0, 0)), result[0]);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 2, 9, 0, 0),
                new DateTime(2020, 8, 2, 18, 0, 0)), result[1]);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 3, 9, 0, 0),
                new DateTime(2020, 8, 3, 18, 0, 0)), result[2]);
        }

        [Test]
        public void TwoEntersecException()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            RepairExclusion exclusion1 = new RepairExclusion(
                new DateTime(2020, 8, 2, 8, 0, 0),
                new DateTime(2020, 8, 2, 10, 0, 0),
                0,
                TimeMeasure.None);
            RepairExclusion exclusion2 = new RepairExclusion(
                new DateTime(2020, 8, 2, 9, 0, 0),
                new DateTime(2020, 8, 2, 11, 0, 0),
                0,
                TimeMeasure.None);

            algo.AddRule(rule1);
            algo.AddRule(exclusion1);
            algo.AddRule(exclusion2);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 1, 9, 0, 0),
                new DateTime(2020, 8, 1, 17, 0, 0)), result[0]);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 2, 11, 0, 0),
                new DateTime(2020, 8, 2, 17, 0, 0)), result[1]);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 3, 9, 0, 0),
                new DateTime(2020, 8, 3, 17, 0, 0)), result[2]);
        }

        [Test]
        public void TwoEntersecException_2()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            RepairRule rule1 = new RepairRule(
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            RepairExclusion exclusion1 = new RepairExclusion(
                new DateTime(2020, 8, 2, 8, 0, 0),
                new DateTime(2020, 8, 2, 12, 0, 0),
                0,
                TimeMeasure.None);
            RepairExclusion exclusion2 = new RepairExclusion(
                new DateTime(2020, 8, 2, 9, 0, 0),
                new DateTime(2020, 8, 2, 11, 0, 0),
                0,
                TimeMeasure.None);

            algo.AddRule(rule1);
            algo.AddRule(exclusion1);
            algo.AddRule(exclusion2);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 1, 9, 0, 0),
                new DateTime(2020, 8, 1, 17, 0, 0)), result[0]);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 2, 12, 0, 0),
                new DateTime(2020, 8, 2, 17, 0, 0)), result[1]);
            Assert.AreEqual(new Tuple<DateTime, DateTime>(
                new DateTime(2020, 8, 3, 9, 0, 0),
                new DateTime(2020, 8, 3, 17, 0, 0)), result[2]);
        }
    }
}