﻿using System;
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
        internal long[] SymbolsWithWeights;
        /// <summary>
        /// reader, reading input file
        /// </summary>
        private BinaryReader Reader;

        public HuffmanReader(BinaryReader reader)
        {
            this.SymbolsWithWeights = new long[256];
            for (int i = 0; i < this.SymbolsWithWeights.Length; i++)
            {
                this.SymbolsWithWeights[i] = 0;
            }
            this.Reader = reader;
        }
        
        public bool ReadFileUsingBuffer()
        {
            int size = 4096;
            byte[] buffer;
            bool empty = true;
            do
            {
                buffer = Reader.ReadBytes(size);
                foreach (var item in buffer)
                {
                    this.SymbolsWithWeights[item]++;
                    empty = false;
                }
            } while (buffer.Length == size);

            return !empty;
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

                this.SymbolsWithWeights[SymbolRead]++;
                
             }
            return !empty;

        }
        /// <summary>
        /// provides the access to SymbolsAndWeights dictionary
        /// </summary>
        /// <returns></returns>
        public long[] ProvideSymbolsWihtWeights()
        {
            return this.SymbolsWithWeights;
        }
    }
}

