using Microsoft.VisualStudio.TestTools.UnitTesting;
using WheelTTSLinesCreatorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary.Tests
{
    [TestClass()]
    public class TextFilterTests
    {
        [TestMethod()]
        public void SetFiltersTest()
        {
            //arrange
            string input = "{[i]}>{}|{[/i]}>{}|{\"}>{}|{*}>{}|{!}>{,}|{. }>{, }";

            List<Filter> expected = new List<Filter>();
            expected.Add(new Filter { key = "[i]", convertTo = "" });
            expected.Add(new Filter { key = "[/i]", convertTo = "" });
            expected.Add(new Filter { key = "\\", convertTo = "" });
            expected.Add(new Filter { key = "*", convertTo = "" });
            expected.Add(new Filter() { key = "!", convertTo = "," });
            expected.Add(new Filter { key = ". ", convertTo = ", " });

            //act
            TextFilter.SetFilters(input);

            //assert
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].key, TextFilter.Filters[i].key);
                Assert.AreEqual(expected[i].convertTo, TextFilter.Filters[i].convertTo);
            }
        }

        [TestMethod()]
        public void FilterStringTest()
        {
            //arrange
            string input1 = "Which of these \"veggies\" are, weirdly enough, fruit?";
            string expected1 = "Which of these veggies are, weirdly enough, fruit?";

            string input2 = "Which of these are actual bucket list items from [i]The Bucket List[/i]!";
            string expected2 = "Which of these are actual bucket list items from The Bucket List,";

            string input3 = "Which schools are U.S. Ivy League *universities*?";
            string expected3 = "Which schools are U.S, Ivy League universities?";


            //act
            TextFilter.SetFilters("{[i]}>{}|{[/i]}>{}|{\"}>{}|{*}>{}|{!}>{,}|{. }>{, }");
            string actual1 = TextFilter.FilterString(input1);
            string actual2 = TextFilter.FilterString(input2);
            string actual3 = TextFilter.FilterString(input3);

            //assert
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
            Assert.AreEqual(expected3, actual3);
        }
    }
}