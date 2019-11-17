using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Huffman1;
using System.Collections.Generic;
using System;

namespace Huffman1Tests
{
    [TestClass]
    public class TreeBuilderTests
    {
        [TestMethod]
        public void ComparerTest()
        {
            ulong[] Test = new ulong[256];
            Test[97] = 6;
            Test[98] = 2;
            Test[32] = 5;
            Test[99] = 5;
            Test[100] = 1;
            Test[101] = 2;
            Test[102] = 2;
            Test[52] = 2;
            TreeBuilder builder = new TreeBuilder(Test);
            for (int i = 0; i < 5; i++)
            {
                InnerNode n = new InnerNode((ulong)i, i);
                builder.Forest.Add(n);
            }
            while (builder.Forest.Count > 0)
            {
                var a = builder.Forest.Min;
                Console.WriteLine(a.GetType());
                Console.WriteLine(a.Weight);
                builder.Forest.Remove(a);
            }
        }
    }
}
