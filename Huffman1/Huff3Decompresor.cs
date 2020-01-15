using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Huffman1
{
    public static class Huff3Decompresor
    {
        /// <summary>
        /// read data part of input file, writes it to output file decompressed
        /// </summary>
        /// <param name="Reader"></param>
        /// <param name="Writer"></param>
        /// <param name="Tree">contains huffman tree</param>
        public static void DecompressFile(BinaryReader Reader, BinaryWriter Writer, Huff3TreeBuilder Tree)
        {
            int size = 4096;
            byte[] buffer;
            bool empty = true;          
            uint mask = 1; //to get least significant bit
            //we need to know, how many symbols should be in file otherwise we could interpret trailing zeros at the end of file as symbols
            ulong remainingSymbols = Tree.NumberOfSymbols;  
            Node Root = Tree.Root;
            Node iterator = Tree.Root;

           //read file using buffer
            do
            {
                buffer = Reader.ReadBytes(size);
                foreach (byte item in buffer)
                {
                    uint it = (uint)item;
                    for (int i = 0; i < 8; i++)
                    {
                        //get value of the least significant bit
                        uint curr = it & mask;
                       // Console.Write("{0} ", curr);
                       //shift - get rid of bit already taken into account
                        it = it >>1 ;

                        if (remainingSymbols == 0)
                        {
                            //end of file - only trailling zeroes remaining
                            return;
                        }

                        DoStepInTree(ref iterator, curr, Writer, Root,ref remainingSymbols);
                        
                    }
                    empty = false;
                }
            } while (buffer.Length == size);
            if (remainingSymbols > 0)
            {
                Console.WriteLine("File Error");
            }

        }
        /// <summary>
        /// visits left or right son of last node visited, if son is leaf writes its symbol to writer and returns to root
        /// </summary>
        /// <param name="iterator"></param>
        /// <param name="direction">0 left son, 1 right son</param>
        /// <param name="writer"></param>
        /// <param name="Root">root of Huffman tree</param>
        /// <param name="remainingSymbols"></param>
        internal static void DoStepInTree(ref Node iterator, uint direction, BinaryWriter writer, Node Root, ref ulong remainingSymbols)
        {
            //step one level down the tree
            InnerNode inner = (InnerNode)iterator;
            if (direction == 1)
            {
                iterator = inner.Right;
            }
            else if (direction == 0)
            {
                iterator = inner.Left;
            }
            else
            {
                throw new NotImplementedException("HuffDecompresor - extracting of bits does not work properly");
            }
            if(iterator is Leaf)
            {
                Leaf leaf = (Leaf)iterator;
                writer.Write(leaf.Symbol);
                remainingSymbols--;
                //return up to root - we are looking for another leaf
                iterator = Root;
            }
        }
    }
}
