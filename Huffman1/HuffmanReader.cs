using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Huffman1
{
    /// <summary>
    /// Class supossed to read binary file opened by Reader and count occurences of its bytes
    /// </summary>
    public class HuffmanReader
    {
        /// <summary>
        /// Dictionary storing numbers of occurences of bytes in input file
        /// </summary>
        internal Dictionary<byte, int> SymbolsWithWeights;
        /// <summary>
        /// reader, reading input file
        /// </summary>
        private BinaryReader Reader;

        public HuffmanReader(BinaryReader reader)
        {
            this.SymbolsWithWeights = new Dictionary<byte, int>();
            this.Reader = reader;
        }
        /// <summary>
        /// reads binary input file and counts occurences of its bytes
        /// </summary>
        /// <returns>true, if file is not empty, false otherwise</returns>
        public bool ReadFile()
        {
            bool empty = true;
            byte SymbolRead;
             while (true)
             {
                //try to read byte
                try
                {
                    SymbolRead = this.Reader.ReadByte();
                    empty = false;
                }
                catch (Exception e) when (e is EndOfStreamException)
                {
                    break;
                }

                //read was succesful, add read value to dictionary
                if (this.SymbolsWithWeights.ContainsKey(SymbolRead))
                {
                    this.SymbolsWithWeights[SymbolRead]++;
                }
                else
                {
                    this.SymbolsWithWeights[SymbolRead] = 1;
                }
                
             }
            return !empty;

        }
        /// <summary>
        /// provides the access to SymbolsAndWeights dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<byte, int> ProvideSymbolsWihtWeights()
        {
            return this.SymbolsWithWeights;
        }
    }
}

