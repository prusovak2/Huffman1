using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Huffman1;
using System.Collections.Generic;
using System;

namespace Huffman1Tests
{
    [TestClass]
    public class HuffmanReaderTests
    {
        [TestMethod]
        public void ReadFileTest()
        {
            Stream s = File.OpenRead(@"TestFiles\Ins\ValidInput.bin");
            BinaryReader br = new BinaryReader(s);
            HuffmanReader reader = new HuffmanReader(br);
            bool b = reader.ReadFile();
            Assert.IsTrue(b);
            for (int i = 0; i < 256; i++)
            {
                if (reader.SymbolsWithWeights[i] > 0)
                {
                    Console.WriteLine("{0}:{1}", i, reader.SymbolsWithWeights[i]);
                }

            }
            Dictionary<byte, ulong> Test = new Dictionary<byte, ulong>();
            Test[97] = 6;
            Test[98] = 5;
            Test[32] = 5;
            Test[99] = 5;
            Test[100] = 1;
            Test[101] = 2;
            Test[102] = 2;
            Test[52] = 2;
            foreach (var item in Test)
            {
                if (Test[item.Key] != reader.SymbolsWithWeights[item.Key])
                {

                    Console.WriteLine($"key: {item.Key} test:{Test[item.Key]} reader:{reader.SymbolsWithWeights[item.Key]}");

                    Assert.Fail();
                }
                //reader.SymbolsWithWeights.Remove(item.Key);
            }

        }
        [TestMethod]
        public void EmptyInputTest()
        {
            Stream s = File.OpenRead(@"TestFiles\Ins\EmptyInput.bin");
            BinaryReader br = new BinaryReader(s);
            HuffmanReader reader = new HuffmanReader(br);
            bool b = reader.ReadFile();
            Assert.IsFalse(b);
        }

        [TestMethod]
        public void ReadFileUsingBufferTest()
        {
            Stream s = File.OpenRead(@"TestFiles\Ins\ValidInput.bin");
            BinaryReader br = new BinaryReader(s);
            HuffmanReader reader = new HuffmanReader(br);
            bool b = reader.ReadFileUsingBuffer();
            Assert.IsTrue(b);
            for (int i = 0; i < 256; i++)
            {
                if (reader.SymbolsWithWeights[i] > 0)
                {
                    Console.WriteLine("{0}:{1}", i, reader.SymbolsWithWeights[i]);
                }

            }
            Dictionary<byte, ulong> Test = new Dictionary<byte, ulong>();
            Test[97] = 6;
            Test[98] = 5;
            Test[32] = 5;
            Test[99] = 5;
            Test[100] = 1;
            Test[101] = 2;
            Test[102] = 2;
            Test[52] = 2;
            foreach (var item in Test)
            {
                if (Test[item.Key] != reader.SymbolsWithWeights[item.Key])
                {

                    Console.WriteLine($"key: {item.Key} test:{Test[item.Key]} reader:{reader.SymbolsWithWeights[item.Key]}");

                    Assert.Fail();
                }
                //reader.SymbolsWithWeights.Remove(item.Key);
            }

        }
    }

           

            
    }
            
 
    

