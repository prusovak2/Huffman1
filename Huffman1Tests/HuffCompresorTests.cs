using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Huffman1;
using System.Collections.Generic;
using System;
namespace Huffman1Tests
{
    [TestClass]
    public class HuffCompresorTests
    {
        [TestMethod]
        public void Test()
        {
            //ulong u = 0xbb;
            Stream s = File.OpenWrite(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\TestFile.out");
            BinaryWriter writer = new BinaryWriter(s);
            Leaf leaf = new Leaf(112, 2);
            ulong result = 0x8000000000000000;
            //u <<= 8;
            // writer.Write(result);
            writer.Write(result);
            writer.Flush();
            writer.Close();



        }
        [TestMethod]
        public void GenHeaderTest()
        {
            //ulong b = 0x2B;
            Stream s =File.OpenWrite(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\Header.out");
            BinaryWriter writer = new BinaryWriter(s);

            HuffCompresor.GenHeader(writer);
            writer.Flush();
            writer.Close();
            s.Close();
            bool same = Utils.FileDiff(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Ins\header.in", @"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\Header.out");
            Assert.IsTrue(same);
            
        }
        [TestMethod]
        public void InnerNodeToBinaryTest()
        {
            InnerNode node = new InnerNode(44, 42);
            Stream s = File.OpenWrite(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\TestFile.out");
            BinaryWriter writer = new BinaryWriter(s);
            ulong correct = 0x58;
            ulong result =node.NodeToBinary();
            writer.Write(result);
            Assert.AreEqual(correct, result);
            writer.Flush();
            writer.Close();
        }
        [TestMethod]
        public void LeafToBinaryTest()
        {
            Leaf leaf = new Leaf(112, 2);
            Stream s = File.OpenWrite(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\LeafToBin.out");
            BinaryWriter writer = new BinaryWriter(s);
            ulong result = leaf.NodeToBinary();
            writer.Write(result);

            ulong correct = 0x7000000000000005;
            Assert.AreEqual(correct, result);

             leaf.Weight = 1;
             leaf.Symbol = 46;
             result = leaf.NodeToBinary();
             writer.Write(result);
            correct = 0x2E00000000000003;
            Assert.AreEqual(correct, result);

            leaf.Weight = 1;
             leaf.Symbol = 83;
             result = leaf.NodeToBinary();
            correct = 0x5300000000000003;
            writer.Write(result);
            Assert.AreEqual(correct, result);

            writer.Flush();
             writer.Close();
             
        }
        [TestMethod]
        public void GenTreeTest()
        {
            Stream s = File.OpenWrite(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\PrintTreeBin.out");
            BinaryWriter writer = new BinaryWriter(s);

            Stream ins = File.OpenRead(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Ins\simple4.in");
            BinaryReader reader = new BinaryReader(ins);

            HuffCompresor.GenTree(reader, writer);

            writer.Flush();
            writer.Close();
            reader.Close();
            bool same = Utils.FileDiff(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Ins\Tree.in", @"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\PrintTreeBin.out");
            Assert.IsTrue(same);


        }
        [TestMethod]
        public void CodeBytesTest()
        {
            Stream ins = File.OpenRead(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Ins\simple4.in");
            BinaryReader reader = new BinaryReader(ins);

            HuffmanReader HuffReader = new HuffmanReader(reader);
            bool NotEmpty = HuffReader.ReadFileUsingBuffer();
            //Build Huffman tree
            TreeBuilder Builder = new TreeBuilder(HuffReader.ProvideSymbolsWihtWeights());
            Node Root = Builder.BuildHuffTree();

            TreePrinter.PrintNiceTree(Root, 0);

            Stack<byte> path = new Stack<byte>();
            HuffCompresor.CodeBytes(Root, path);

            for (int i = 0; i < HuffCompresor.Codes.Length; i++)
            {
                if (HuffCompresor.Codes[i] != null)
                {
                    Console.Write($"{i}: ");
                    for (int j = 0; j < HuffCompresor.Codes[i].Length; j++)
                    {
                        Console.Write($"{HuffCompresor.Codes[i][j]} ");
                    }
                    Console.WriteLine();
                }
            } 
        }

        [TestMethod]
        public void CompressContentTest()
        {
            Stream s = File.OpenWrite(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Outs\Compressed.out");
            BinaryWriter writer = new BinaryWriter(s);

            Stream ins = File.OpenRead(@"D:\MFF\ZS_2019\c#_repos\Huffman1\Huffman1Tests\bin\Debug\netcoreapp3.0\TestFiles\Huff2\Ins\simple4.in");
            BinaryReader reader = new BinaryReader(ins);


            HuffmanReader HuffReader = new HuffmanReader(reader);
            bool NotEmpty = HuffReader.ReadFileUsingBuffer();
            //Build Huffman tree
            TreeBuilder Builder = new TreeBuilder(HuffReader.ProvideSymbolsWihtWeights());
            Node Root = Builder.BuildHuffTree();

            ins.Seek(0, 0);

            HuffCompresor.CompressContent(Root, writer, reader);

        }
        
       
    }
}
