using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Huffman1;
using System.Collections.Generic;
using System;
namespace Huffman1Tests
{
    [TestClass]
    public class TreePrinterTests
    {
        [TestMethod]
        public void PrintNiceTreeTest()
        {
            Dictionary<byte, int> Test = new Dictionary<byte, int>();
            Test[97] = 6;
            Test[98] = 2;
            Test[32] = 5;
            Test[99] = 5;
            Test[100] = 1;
            Test[101] = 2;
            Test[102] = 2;
            Test[52] = 2;
            TreeBuilder builder = new TreeBuilder(Test);
            
            Node Root = builder.BuildHuffTree();
            TreePrinter.PrintNiceTree(Root, 0);
        }
        [TestMethod]
        public void PrintNiceTreeLeafTest()
        {
            Dictionary<byte, int> Test = new Dictionary<byte, int>();
           /* Test[97] = 6;
            Test[98] = 2;
            Test[32] = 5;
            Test[99] = 5; */
            Test[100] = 1;
           /* Test[101] = 2;
            Test[102] = 2;
            Test[52] = 2;*/
            TreeBuilder builder = new TreeBuilder(Test);

            Node Root = builder.BuildHuffTree();
            TreePrinter.PrintNiceTree(Root, 0);
        }
        [TestMethod]
        public void PrintCompresedTreeTest()
        {
            Dictionary<byte, int> Test = new Dictionary<byte, int>();
            Test[97] = 6;
            Test[98] = 2;
            Test[32] = 5;
            Test[99] = 5;
            Test[100] = 1;
            Test[101] = 2;
            Test[102] = 2;
            Test[52] = 2;
            TreeBuilder builder = new TreeBuilder(Test);

            Node Root = builder.BuildHuffTree();
            StreamWriter sw = new StreamWriter(@"TestFiles\myCompresTree.txt");
            TreePrinter.PrintCompresedTree(Root,sw);
            //sw.Flush();
        }
    }
}
