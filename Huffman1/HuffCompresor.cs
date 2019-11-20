using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Huffman1
{
    /// <summary>
    /// Majority of solution of HUffman II, creates content of compressed file 
    /// </summary>
    public static class HuffCompresor
    {
        //array of 256 arrays, each containing path from particular leaf of Huffman tree to the root
        public static byte[][] Codes = new byte[256][];

        /// <summary>
        /// prints header, coded huffman tree and compressed file content to binary writer
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="reader"></param>
        public static void CompressFile(BinaryWriter writer, BinaryReader reader)
        {
            GenHeader(writer);

            Node Root =GenTree(reader, writer); //reads file

            GenEightZeroBytes(writer);

            reader.BaseStream.Seek(0, 0);  //return to the beginig of file

            //CompressContend needs to read file again

            CompressContent(Root, writer, reader);




        }
        /// <summary>
        /// Generates header of compressed file as specified
        /// </summary>
        /// <param name="writer"></param>
        internal static void GenHeader(BinaryWriter writer)
        {
            //reverse header
            byte[] headerBytes = { 0x66, 0x66, 0x7D, 0x6D, 0x7C, 0x75, 0x68, 0x7B };
            ulong header = headerBytes[1];
            for (int i = 1; i < headerBytes.Length; i++)
            {
                header <<= 8;  //8 zeroes to begining
                header |= headerBytes[i]; //add another header byte
            }
            writer.Write(header);
            writer.Flush();

        }
        /// <summary>
        /// Reads file, creates Huffman tree according its content and writes it to binary writer in specified binary representation
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
        /// <returns>Root of Huffman tree created</returns>
        internal static Node GenTree(BinaryReader reader, BinaryWriter writer)
        {
            HuffmanReader HuffReader = new HuffmanReader(reader);
            bool NotEmpty = HuffReader.ReadFileUsingBuffer();
            //Build Huffman tree
            TreeBuilder Builder = new TreeBuilder(HuffReader.ProvideSymbolsWihtWeights());
            Node Root = Builder.BuildHuffTree();

            TreePrinter.PrintTreeBinary(Root, writer);
            writer.Flush();
            return Root;
            
        }
        /// <summary>
        /// Compresses data from input file
        /// </summary>
        /// <param name="Root">Root of Huffman tree build according to content of input file</param>
        /// <param name="writer"></param>
        /// <param name="reader"></param>
        internal static void CompressContent(Node Root, BinaryWriter writer, BinaryReader reader)
        {
            //get codes of all bytes occuring in input file
            Stack<byte> path = new Stack<byte>();
            CodeBytes(Root, path);

            
            int size = 4096;
            byte[] buffer;
            ulong record = 0;
            ulong oneOnMSb = 0x8000000000000000; //constant - in binary MSb is 1 and another 63 bits are zero
            int bitCounter = 0;

            do //read file using buffer
            {
                buffer = reader.ReadBytes(size);
                foreach (var item in buffer) //for each byte from file
                {
                    //for each bit in code of particular byte (item)
                    for (int i = HuffCompresor.Codes[item].Length-1; i >= 0; i--)
                    {
                        //ulong used as buffer is full, lets print it
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
                            record = record | oneOnMSb; //add one to MSb
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

            //there are some unprinted bits left in buffer after reading of file was finished
            if (bitCounter != 0)
            {
                //shift valid bits so that the first one of them is on least significant position
                int shift = 64 - bitCounter;
                record >>= shift;

                //count how many bytes from ulong record contains valid bits
                //result is supposed to be allign to bytes
                int validBytes = bitCounter / 8;
                if ((bitCounter % 8) > 0)
                {
                    validBytes++;
                }

                //print valid bytes
                byte[] lastBytes = BitConverter.GetBytes(record);
                for (int i = 0; i < validBytes; i++)
                {
                    writer.Write(lastBytes[i]);
                }
            }
            writer.Flush();
        }
        /// <summary>
        /// recursive
        /// using one DFS of Huffman tree generates codes of all bytes occuring in file
        /// codes created are reversed - from leaf to root
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="path">stack representing current possition in tree</param>
        internal static void CodeBytes(Node Root, Stack<byte> path)
        {
            if(Root is Leaf)
            {
                Leaf leaf = (Leaf)Root;
                //leaf found, save its position = its code
                HuffCompresor.Codes[leaf.Symbol] = path.ToArray();
               
                //returning from recursive call means returning to upper lever of Huffman tree = forgeting the last step of path                
                path.Pop();
                return;
            }

            InnerNode node = (InnerNode)Root;
            //going to left subtree (0)
            path.Push(0);
            CodeBytes(node.Left, path);

            //going to right subtree (1)
            path.Push(1);
            CodeBytes(node.Right, path);

            //Root level of Hufftree is not represented in path
            //therefore there is one level more of recursive calls than steps on path
            //while returning from very firts call of function, there is nothing to be poped from path
            if (path.Count>0)
            {
                path.Pop();
            }
        }
        /// <summary>
        /// Generates 8 zero bytes
        /// </summary>
        /// <param name="writer"></param>
        internal static void GenEightZeroBytes(BinaryWriter writer)
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
