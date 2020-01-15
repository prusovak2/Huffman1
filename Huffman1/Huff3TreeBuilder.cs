using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Huffman1
{
    public class Huff3TreeBuilder
    {
        //root of huff tree created - read from binary file;
        public Node Root { get; set; }
        public ulong NumberOfSymbols { get; set; } = 0;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>false when error reading file occured, true otherwise</returns>
        public bool ReadHeaderAndTreeFromBinaryFile(BinaryReader reader)
        {
            ulong header;
            try
            {
                 header = reader.ReadUInt64();
            }
            catch(Exception e) when (e is EndOfStreamException)
            {
                //file is empty
                Console.WriteLine("File Error");
                return false;
            }
            
            if (!this.checkHeader(header))
            {
                //flawed or missing header - file error message is pronted from checkHeader method
                return false;
            }
            bool flawed = false;
            this.Root = ReadTreeFromBinaryFile(reader, ref flawed);
            if (flawed) 
            {
                Console.WriteLine("File Error");
                return false;
            }
            ulong zero = reader.ReadUInt64();
            if (zero != (ulong)0)
            {
                //zero ulong expected between tree and compresed data
                Console.WriteLine("File Error");
                return false;
            }

            return true; //just to have some ret value before im gonna write meanigfull code
        }

        internal Node ReadTreeFromBinaryFile(BinaryReader reader, ref bool flawed) 
        {
            ulong input;
            try
            {
                input = reader.ReadUInt64();
            }
            catch (Exception e) when (e is EndOfStreamException)
            {
                //end of file - unexpeccted
                flawed = true;
                return null;
            }
            if (input == (ulong)0) 
            {
                //end of tree -  unexpected - recursion should read tree and only tree
                flawed = true;
                return null;
            }
            Node node = this.IdentifyAndCreateNode(input);
            if(node is Leaf) 
            {
                return node;
            }
            InnerNode inner = (InnerNode)node;
            inner.Left = ReadTreeFromBinaryFile(reader, ref flawed);
            inner.Right = ReadTreeFromBinaryFile(reader, ref flawed);
            return inner;
        }

        internal Node IdentifyAndCreateNode(ulong input)
        {
            ulong weight = input << 8;
            weight >>= 9;

            ulong typeMask = 1;
            ulong type = input & typeMask;
            if (type == 0)
            {
                //inner node
                InnerNode innner = new InnerNode(weight);
                return innner;
            }
            if(type == 1)
            {
                //leaf
                ulong symbol = input >> 56;
                byte s = (byte)symbol;
                Leaf leaf = new Leaf(s, weight);
                this.NumberOfSymbols += weight;
                return leaf;
            }
            else
            {
                throw new NotImplementedException("SOME MISTAKE IN TYPE CHECK OF NODE");
            }

        }

        internal bool checkHeader(ulong toCheck)
        {
            byte[] bytesToCheck = BitConverter.GetBytes(toCheck);
            byte[] header = { 0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66 };
            for (int i = 0; i < header.Length; i++)
            {
                if(header[i]!= bytesToCheck[i])
                {
                    Console.WriteLine("File Error");
                    return false;
                }
            }
            return true;
        }
    }
}
