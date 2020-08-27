using System;
using System.Linq;
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

            var guid = Guid.NewGuid();
            Rule rule1 = new Rule(
                guid, 
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            algo.AddRule(rule1);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(
                new ResultStructure ()
                {
                    Guid = guid, 
                    StartDateTime = new DateTime(2020, 8, 1, 9, 0, 0), 
                    EndDateTime = new DateTime(2020, 8, 1, 17, 0, 0)
                },
                result[0]);
            Assert.AreEqual(new ResultStructure()
            {
                Guid = guid,
                StartDateTime = new DateTime(2020, 8, 2, 9, 0, 0),
                EndDateTime = new DateTime(2020, 8, 2, 17, 0, 0)
            }, result[1]);
            Assert.AreEqual(new ResultStructure()
            {
                Guid = guid,
                StartDateTime = new DateTime(2020, 8, 3, 9, 0, 0),
                EndDateTime = new DateTime(2020, 8, 3, 17, 0, 0)
            }, result[2]);
        }

        [Test]
        public void OneNonPeriodicalRule()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            var rule0 = Guid.NewGuid();

            Rule rule1 = new Rule(rule0,
                new DateTime(2020, 8, 2, 9, 0, 0),
                new DateTime(2020, 8, 2, 17, 0, 0),
                0,
                TimeMeasure.None);
            Rule rule2 = new Rule(new Guid(),
                new DateTime(2020, 7, 2, 9, 0, 0),
                new DateTime(2020, 7, 2, 17, 0, 0),
                1,
                TimeMeasure.None);
            Rule rule3 = new Rule(new Guid(),
                new DateTime(2020, 7, 2, 9, 0, 0),
                new DateTime(2020, 7, 2, 17, 0, 0),
                0,
                TimeMeasure.Days);
            algo.AddRule(rule1);
            algo.AddRule(rule2);
            algo.AddRule(rule3);

            var result = algo.GetResult();

            Assert.AreEqual(1, result.Count);
            ResultStructure resultStructure = new ResultStructure()
            {
                Guid = rule0 ,
                StartDateTime = new DateTime(2020, 8, 2, 9, 0, 0),  
                EndDateTime = new DateTime(2020, 8, 2, 17, 0, 0)
            };

            Assert.AreEqual(resultStructure, (ResultStructure)(result[0]));
        }

        [Test]
        public void RuleAndException_1()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            Rule rule1 = new Rule(Guid.NewGuid(),
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            Exclusion exclusion1 = new Exclusion(
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

            Rule rule1 = new Rule(Guid.NewGuid(),
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            Exclusion exclusion1 = new Exclusion(
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

            Rule rule1 = new Rule(Guid.NewGuid(),
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            Exclusion exclusion1 = new Exclusion(
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

            Rule rule1 = new Rule(Guid.NewGuid(),
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            Exclusion exclusion1 = new Exclusion(
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

            Rule rule1 = new Rule(Guid.NewGuid(),
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);
            Rule rule2 = new Rule(Guid.NewGuid(),
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

            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();

            Rule rule1 = new Rule(guid1,
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);
            Rule rule2 = new Rule(guid2,
                new DateTime(2020, 6, 1, 12, 0, 0),
                new DateTime(2020, 6, 1, 13, 0, 0),
                1,
                TimeMeasure.Days);

            algo.AddRule(rule1);
            algo.AddRule(rule2);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(new ResultStructure(){Guid = guid1, StartDateTime = new DateTime(2020, 8, 1, 9, 0, 0),
                EndDateTime = new DateTime(2020, 8, 1, 17, 0, 0) }, result[0]);
            Assert.AreEqual(new ResultStructure()
            {
                Guid = guid1,
                StartDateTime = new DateTime(2020, 8, 2, 9, 0, 0),
                EndDateTime = new DateTime(2020, 8, 2, 17, 0, 0)
            }, result[1]);
            Assert.AreEqual(new ResultStructure()
            {
                Guid = guid1,
                StartDateTime = new DateTime(2020, 8, 3, 9, 0, 0),
                EndDateTime = new DateTime(2020, 8, 3, 17, 0, 0)
            }, result[2]);
        }

        [Test]
        public void TwoIntersecRules_2()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            Rule rule1 = new Rule(guid1,
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);
            Rule rule2 = new Rule(guid2,
                new DateTime(2020, 6, 1, 16, 0, 0),
                new DateTime(2020, 6, 1, 18, 0, 0),
                1,
                TimeMeasure.Days);

            algo.AddRule(rule1);
            algo.AddRule(rule2);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(new ResultStructure()
            {
                Guid = guid1,
                StartDateTime = new DateTime(2020, 8, 1, 9, 0, 0),
                EndDateTime = new DateTime(2020, 8, 1, 18, 0, 0)
            }, result[0]);
            Assert.AreEqual(new ResultStructure()
            {
                Guid = guid1,
                StartDateTime = new DateTime(2020, 8, 2, 9, 0, 0),
                EndDateTime = new DateTime(2020, 8, 2, 18, 0, 0)
            }, result[1]);
            Assert.AreEqual(new ResultStructure()
            {
                Guid = guid1,
                StartDateTime = new DateTime(2020, 8, 3, 9, 0, 0),
                EndDateTime = new DateTime(2020, 8, 3, 18, 0, 0)
            }, result[2]);
        }

        [Test]
        public void TwoEntersecException()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));

            var guid = Guid.NewGuid();
            Rule rule1 = new Rule(guid,
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            Exclusion exclusion1 = new Exclusion(
                new DateTime(2020, 8, 2, 8, 0, 0),
                new DateTime(2020, 8, 2, 10, 0, 0),
                0,
                TimeMeasure.None);
            Exclusion exclusion2 = new Exclusion(
                new DateTime(2020, 8, 2, 9, 0, 0),
                new DateTime(2020, 8, 2, 11, 0, 0),
                0,
                TimeMeasure.None);

            algo.AddRule(rule1);
            algo.AddRule(exclusion1);
            algo.AddRule(exclusion2);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new ResultStructure()
            {
                Guid = guid, 
                StartDateTime = new DateTime(2020, 8, 1, 9, 0, 0), 
                EndDateTime = new DateTime(2020, 8, 1, 17, 0, 0)
            }, result[0]);
            Assert.AreEqual(new ResultStructure()
                {
                    Guid = guid, 
                    StartDateTime = new DateTime(2020, 8, 2, 11, 0, 0), 
                    EndDateTime = new DateTime(2020, 8, 2, 17, 0, 0)
                }, result[1]);
            Assert.AreEqual(new ResultStructure()
                {
                    Guid = guid,
                    StartDateTime = new DateTime(2020, 8, 3, 9, 0, 0),
                    EndDateTime = new DateTime(2020, 8, 3, 17, 0, 0)
                }, result[2]);
        }

        [Test]
        public void TwoEntersecException_2()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020, 8, 1), new DateTime(2020, 8, 3, 23, 59, 59));
            Guid guid = Guid.NewGuid();
            Rule rule1 = new Rule(guid,
                new DateTime(2020, 6, 1, 9, 0, 0),
                new DateTime(2020, 6, 1, 17, 0, 0),
                1,
                TimeMeasure.Days);

            Exclusion exclusion1 = new Exclusion(
                new DateTime(2020, 8, 2, 8, 0, 0),
                new DateTime(2020, 8, 2, 12, 0, 0),
                0,
                TimeMeasure.None);
            Exclusion exclusion2 = new Exclusion(
                new DateTime(2020, 8, 2, 9, 0, 0),
                new DateTime(2020, 8, 2, 11, 0, 0),
                0,
                TimeMeasure.None);

            algo.AddRule(rule1);
            algo.AddRule(exclusion1);
            algo.AddRule(exclusion2);

            var result = algo.GetResult();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new ResultStructure()
                {
                    Guid = guid,
                    StartDateTime = new DateTime(2020, 8, 1, 9, 0, 0),
                EndDateTime = new DateTime(2020, 8, 1, 17, 0, 0)
            }, result[0]);
            Assert.AreEqual(new ResultStructure()
                {
                    Guid = guid,
                    StartDateTime = new DateTime(2020, 8, 2, 12, 0, 0),
                    EndDateTime = new DateTime(2020, 8, 2, 17, 0, 0)
            }, result[1]);
            Assert.AreEqual(new ResultStructure()
                {
                    Guid = guid,
                    StartDateTime = new DateTime(2020, 8, 3, 9, 0, 0),
                    EndDateTime = new DateTime(2020, 8, 3, 17, 0, 0)
            }, result[2]);
        }

        [Test]
        public void LiveTest()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(null, new DateTime(2020,6,15));

            algo.AddRule(new Rule(Guid.NewGuid(), new DateTime(2020,6,1, 8,0,0), new DateTime(2020,6,1,20,0,0), 1, TimeMeasure.Days));
            algo.AddRule(new Exclusion( new DateTime(2020, 6, 3, 8, 0, 0), new DateTime(2020, 6, 3, 20, 0, 0), 7, TimeMeasure.Days));

            var result = algo.GetResult();

            Assert.AreEqual(12, result.Count);
        }

        [Test]
        public void CutPeriodTest()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(new DateTime(2020,6,5, 11,30,00), new DateTime(2020, 6, 15, 11,30,00));

            algo.AddRule(new Rule(Guid.NewGuid(), new DateTime(2020,6,1,8,0,0), new DateTime(2020,6,1,13,0,0), 1, TimeMeasure.Days));

            var result = algo.GetResult(true);

            Assert.AreEqual(11, result.Count);
        }

        [Test]
        public void RealTest()
        {
            BentleyOttmanAlgorithm algo = new BentleyOttmanAlgorithm(null, DateTime.Now.AddDays(60));
            Guid rule1 = Guid.NewGuid();
            Guid rule2 = Guid.NewGuid();
            algo.AddRule(new Rule(rule1,new DateTime(2020,8,2,8,0,0), new DateTime(2020,8,3,0,0,0), null,TimeMeasure.None  ));
            algo.AddRule(new Rule(rule2,new DateTime(2020,8,10,8,0,0), new DateTime(2020,8,11,0,0,0), null,TimeMeasure.None  ));

            var result = algo.GetResult(true);

            Assert.AreEqual(rule1, result.First().Guid);
            Assert.AreEqual(rule2, result.Last().Guid);
            Assert.AreEqual(2, result.Count);
        }
    }
}