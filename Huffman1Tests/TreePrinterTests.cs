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
            
            Node Root = builder.BuildHuffTree();
            TreePrinter.PrintNiceTree(Root, 0);
        }
        [TestMethod]
        public void PrintNiceTreeLeafTest()
        {
            ulong[] Test = new ulong[256];
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

            Node Root = builder.BuildHuffTree();
            StreamWriter sw = new StreamWriter(@"TestFiles\myCompresTree.txt");
            TreePrinter.PrintCompresedTree(Root,sw);
            //sw.Flush();
        }
        [TestMethod]
        public void PrintTreeBinaryTest()
        {
            Stream s = File.OpenWrite(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\PrintTreeBin.out");
            BinaryWriter writer = new BinaryWriter(s);

            Stream ins = File.OpenRead(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Ins\simple4.in");
            BinaryReader reader = new BinaryReader(ins);
            HuffmanReader HuffReader = new HuffmanReader(reader);
            bool NotEmpty = HuffReader.ReadFileUsingBuffer();
            //Build Huffman tree
            TreeBuilder Builder = new TreeBuilder(HuffReader.ProvideSymbolsWihtWeights());
            Node Root = Builder.BuildHuffTree();

            TreePrinter.PrintTreeBinary(Root, writer);
            writer.Flush();
            writer.Close();
            reader.Close();
            bool same = Utils.FileDiff(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Ins\Tree.in", @"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\PrintTreeBin.out");
            Assert.IsTrue(same);
        }
    }
}
