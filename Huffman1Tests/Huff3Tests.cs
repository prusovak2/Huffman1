using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Huffman1;

namespace Huffman1Tests
{
    [TestClass]
    public class Huff3Tests
    {
        [TestMethod]
        public void JustPlayingAround()
        {
            byte b = 113;
            uint mask = 1;
            uint u = (uint)b & mask;
            Console.WriteLine(u);
            if (u != (uint)1)
            {
                Assert.Fail();
            }


        }
        [TestMethod]
        public void DecompressTest()
        {
            string inputFile = @"TestFiles\Huff3Tests\simple4.in.huff";
            string[] arg = new string[1];
            arg[0] = inputFile;
            Program.Huff3OpenFiles(arg, out BinaryReader reader, out BinaryWriter writer);
            Huff3TreeBuilder b = new Huff3TreeBuilder();

            bool success = b.ReadHeaderAndTreeFromBinaryFile(reader);


            Assert.IsNotNull(b.Root);
            TreePrinter.PrintNiceTree(b.Root, 0);
            Huff3Decompresor.DecompressFile(reader, writer, b);
            reader.Close();
            writer.Close();

        }

        [TestMethod]
        public void ReadTreeAndHeaderTest()
        {
            string inputFile = @"TestFiles\Huff3Tests\simple4.in.huff";
            string[] arg = new string[1];
            arg[0] = inputFile;
            Program.Huff3OpenFiles(arg, out BinaryReader reader, out BinaryWriter writer);
            Huff3TreeBuilder b = new Huff3TreeBuilder();

            bool success=b.ReadHeaderAndTreeFromBinaryFile(reader);
            Assert.IsNotNull(b.Root);
            TreePrinter.PrintNiceTree(b.Root, 0);
            reader.Close();
            writer.Close();
        }
        [TestMethod]
        public void ReadTreeTest()
        {
            string inputFile = @"TestFiles\Huff3Tests\simple4.in.huff";
            string[] arg = new string[1];
            arg[0] = inputFile;
            Program.Huff3OpenFiles(arg, out BinaryReader reader, out BinaryWriter writer);
            reader.ReadUInt64(); //skip header
            Huff3TreeBuilder b = new Huff3TreeBuilder();

            bool f=false;
            Node n = b.ReadTreeFromBinaryFile(reader, ref f);
            Assert.IsNotNull(n);
            TreePrinter.PrintNiceTree(n, 0);
            Console.WriteLine(b.NumberOfSymbols);
            reader.Close();
            writer.Close();
        }


        [TestMethod]
        public void Huff3OpenFileTest()
        {
            string inputFile = @"TestFiles\Huff3Tests\simple4.in.huff";
            string[] args = new string[1];
            args[0] = inputFile;
            Program.Huff3OpenFiles(args, out BinaryReader reader, out BinaryWriter writer);
            Assert.IsNotNull(reader);
            Assert.IsNotNull(writer);

            reader.Close();
            writer.Close();

            inputFile = @"TestFiles\Huff3Tests\simple.in.h";
            args = new string[1];
            args[0] = inputFile;
            Program.Huff3OpenFiles(args, out reader, out writer);
            Assert.IsNull(reader);
            Assert.IsNull(writer);

            inputFile = @"TestFiles\Huff3Tests\simple.huff";
            args = new string[1];
            args[0] = inputFile;
            Program.Huff3OpenFiles(args, out reader, out writer);
            Assert.IsNull(reader);
            Assert.IsNull(writer);

            
        }

        [TestMethod]
        public void CheckHeaderTest()
        {
            Stream s = File.OpenRead(@"TestFiles\Huff3Tests\simple.in.huff");
            BinaryReader br = new BinaryReader(s);
            ulong u = br.ReadUInt64();
            Huff3TreeBuilder b = new Huff3TreeBuilder();
            bool correct = b.checkHeader(u);
            Assert.IsTrue(correct);
            ulong something = 238723328;
            correct = b.checkHeader(something);
            Assert.IsFalse(correct);

        }

        [TestMethod]
        public void JustPrintTree()
        {
            string inputFile = @"TestFiles\Huff3Tests\simple4.in";
            string[] args = new string[1];
            args[0] = inputFile;
            bool opened = Program.OpenFile(args, out BinaryReader Reader, out BinaryWriter Writer);
            if (!opened)
            {
                return;
            }
            //create HuffmanReader, that is going to count occurences of bytes in input file
            HuffmanReader HuffReader = new HuffmanReader(Reader);
            bool NotEmpty = HuffReader.ReadFileUsingBuffer();
            //Build Huffman tree
            TreeBuilder Builder = new TreeBuilder(HuffReader.ProvideSymbolsWihtWeights());
            Node Root = Builder.BuildHuffTree();
            TreePrinter.PrintNiceTree(Root, 0);
            Reader.Close();
            Writer.Close();
        }
        [TestMethod]
        public void CreateNodeTest()
        { 

            string inputFile = @"TestFiles\Huff3Tests\simple.in.huff";
            string[] arg = new string[1];
            arg[0] = inputFile;
            Program.Huff3OpenFiles(arg, out BinaryReader reader, out BinaryWriter writer);
            Huff3TreeBuilder b = new Huff3TreeBuilder();
            reader.ReadInt64(); //header
            ulong input = reader.ReadUInt64();
            Node node = b.IdentifyAndCreateNode(input);
            if(node is Leaf)
            {
                Assert.Fail("typecheck error");
            }
            Console.WriteLine("something");
            Assert.AreEqual((ulong)6, node.Weight);

            input = reader.ReadUInt64();
            node = b.IdentifyAndCreateNode(input);
            if (node is InnerNode)
            {
                Assert.Fail("typecheck error");
            }
            Console.WriteLine("something");
            Assert.AreEqual((ulong)3, node.Weight);
            Assert.AreEqual((byte)97, ((Leaf)node).Symbol);

            input = reader.ReadUInt64();
            node = b.IdentifyAndCreateNode(input);
            if (node is Leaf)
            {
                Assert.Fail("typecheck error");
            }
            Console.WriteLine("something");
            Assert.AreEqual((ulong)3, node.Weight);

            input = reader.ReadUInt64();
            node = b.IdentifyAndCreateNode(input);
            if (node is InnerNode)
            {
                Assert.Fail("typecheck error");
            }
            Console.WriteLine("something");
            Assert.AreEqual((ulong)1, node.Weight);
            Assert.AreEqual((byte)99, ((Leaf)node).Symbol);

            input = reader.ReadUInt64();
            node = b.IdentifyAndCreateNode(input);
            if (node is InnerNode)
            {
                Assert.Fail("typecheck error");
            }
            Console.WriteLine("something");
            Assert.AreEqual((ulong)2, node.Weight);
            Assert.AreEqual((byte)98, ((Leaf)node).Symbol);


            input = reader.ReadUInt64();
            Console.WriteLine(input);

            reader.Close();
            writer.Close();
        }

    }
}
