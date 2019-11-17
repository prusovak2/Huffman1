using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Huffman1
{

    public static class HuffCompresor
    {
        public static byte[][] Codes = new byte[256][];

        internal static void GenHeader(BinaryWriter writer)
        {
            byte[] headerBytes = { 0x66, 0x66, 0x7D, 0x6D, 0x7C, 0x75, 0x68, 0x7B };
            ulong header = headerBytes[1];
            for (int i = 1; i < headerBytes.Length; i++)
            {
                header <<= 8;
                header |= headerBytes[i];
            }
            writer.Write(header);
            writer.Flush();

        }
        internal static void GenTree(BinaryReader reader, BinaryWriter writer)
        {
            HuffmanReader HuffReader = new HuffmanReader(reader);
            bool NotEmpty = HuffReader.ReadFileUsingBuffer();
            //Build Huffman tree
            TreeBuilder Builder = new TreeBuilder(HuffReader.ProvideSymbolsWihtWeights());
            Node Root = Builder.BuildHuffTree();

            TreePrinter.PrintTreeBinary(Root, writer);
            writer.Flush();
            
        }
        public static void CompressContent(Node Root, BinaryWriter writer, BinaryReader reader)
        {
            Stack<byte> path = new Stack<byte>();
            CodeBytes(Root, path);

            int size = 4096;
            byte[] buffer;
            ulong record = 0;
            ulong oneOnMSb = 0x8000000000000000;
            int bitCounter = 0;
            do
            {
                buffer = reader.ReadBytes(size);
                foreach (var item in buffer)
                {
                    for (int i = HuffCompresor.Codes[item].Length-1; i >= 0; i--)
                    {
                        if (bitCounter == 64)
                        {
                            //print ulong
                            writer.Write(record);
                            record = 0;
                            bitCounter = 0;
                        }
                        if (HuffCompresor.Codes[item][i] == 1)
                        {
                            //add one to most significant bit
                            record >>= 1; //shif by one to lsb
                            record = record | oneOnMSb;
                            bitCounter++;
                        }
                        else
                        {
                            //add zero to most significant bit
                            record >>= 1; //shif by one to lsb
                            bitCounter++;
                        }
                    }                                      
                }
            } while (buffer.Length == size);
            if (bitCounter != 0)
            {
                //shift valid bits so that the firts one of them is on least significant position
                int shift = 64 - bitCounter;
                record >>= shift;

                int validBytes = bitCounter / 8;
                if ((bitCounter % 8) > 0)
                {
                    validBytes++;
                }

                byte[] lastBytes = BitConverter.GetBytes(record);
                for (int i = 0; i < validBytes; i++)
                {
                    writer.Write(lastBytes[i]);
                }
            }
            writer.Flush();
        }

        internal static void CodeBytes(Node Root, Stack<byte> path)
        {
            if(Root is Leaf)
            {
                Leaf leaf = (Leaf)Root;
                HuffCompresor.Codes[leaf.Symbol] = path.ToArray();
                path.Pop();
                return;
            }

            InnerNode node = (InnerNode)Root;
            path.Push(0);
            CodeBytes(node.Left, path);
            path.Push(1);
            CodeBytes(node.Right, path);
            if (path.Count>0)
            {
                path.Pop();
            }
        }

        public static void GenEightZeroBytes(BinaryWriter writer)
        {
            byte zero = 0;
            for (int i = 0; i < 8; i++)
            {
                writer.Write(zero);

            }
            writer.Flush();
        }

    }
}
