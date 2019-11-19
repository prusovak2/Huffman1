using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Huffman1
{
    /// <summary>
    /// When occurences of bytes in input file are counted, builds a Huffman tree 
    /// </summary>
    public class TreeBuilder
    {
        /// <summary>
        /// forest of unfinished Huffman trees, at first it containes all leaves
        /// everytime, two members are conected into new tree, its root is added to forest
        /// two members to be connected are first removed from forest
        /// </summary>
        internal SortedSet<Node> Forest;
        
        /// <summary>
        /// creates leaves of Huffman from dictionary of bytes and occurences 
        /// </summary>
        /// <param name="SymbolsAndWeights">Dictionary provided by HuffmanReader class</param>
        public TreeBuilder(ulong[] SymbolsAndWeights)
        {
            SortedSet<Node> forest = new SortedSet<Node>(new Comparer());
            for (int i = 0; i < 256; i++)
            {
                if (SymbolsAndWeights[i] > 0)
                {
                    Leaf leaf = new Leaf((byte)i, SymbolsAndWeights[i]);
                    forest.Add(leaf);
                }
                
            }
            this.Forest = forest;
        } 
        /// <summary>
        /// builds Huffman tree from forest of leaves 
        /// </summary>
        /// <returns>Root of Huffman tree</returns>
        public Node BuildHuffTree()
        {
            //times of creation are one of parameters by which inner nodes are compared
            int timeOfCreation = 1;
            
            // while there is more tree in the forest
            while (this.Forest.Count > 1)
            {
                //comparer in Forest takes into account all criteria by which Nodes are compared
                Node firts = this.Forest.Min;
                this.Forest.Remove(firts);
                Node second = this.Forest.Min;
                this.Forest.Remove(second);
                //thaks to comparer it is certaint that Node first is lighter than Node second
                InnerNode newRoot = new InnerNode(firts, second, timeOfCreation);
                timeOfCreation++;
                //add newly created tree to ther forest
                this.Forest.Add(newRoot);
            }
            Node Root = this.Forest.Min;
            this.Forest.Remove(Root);
            //Root of Huffman tree should be the only remaining thing in Forest 
            if (this.Forest.Count != 0)
            {
                Console.WriteLine("Error!");
            }
            return Root;
        }


    }
}
