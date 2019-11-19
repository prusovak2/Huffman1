using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Huffman1
{
    /// <summary>
    /// Abstract class from which nodes of Huffman tree are to be derived
    /// </summary>
    public abstract class Node
    {      
        public abstract ulong Weight { get; set; }
        //public abstract int TimeOfCreation { get; set; }
        public abstract ulong NodeToBinary();
                  
    }
    /// <summary>
    /// Class representing a leaf of Huffman tree
    /// </summary>
    public class Leaf : Node
    {
        public override ulong Weight { get; set; }
        public byte Symbol { get; set; }

        public Leaf(byte Symbol, ulong Weight)
        {
            this.Symbol = Symbol;
            this.Weight = Weight;
        }

        /// <summary>
        /// Codes Leaf to binary as required by Huffman II assigment
        /// </summary>
        /// <returns>ulong containtg bin representation of leaf</returns>
        public override ulong NodeToBinary()
        {
            ulong weight = this.Weight;
            weight <<= 9; //pick lower 55 bits of weight - 9 zeroes to end
            weight >>= 8; //one zero to begining, eight zeroes to end

            ulong symbol = this.Symbol; //56 zeroes and 8 bits of symbol
            symbol <<= 56;

            //first bit 1 represents leaf
            ulong initialOne = 1;

            //or eberzthing together
            ulong binRepresentation = weight | symbol | initialOne;

            return binRepresentation;
        }

        /// <summary>
        /// Handy for testing, converts Leaf to binary and current state after each step of convertion
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        internal ulong NodeToBinaryMock(BinaryWriter writer)
        {
            ulong weight = this.Weight;
            writer.Write(weight);

            weight <<= 9; //pick lower 55 bits of weight - 9 zeroes to end
            weight >>= 8; //one zero to begining

            writer.Write(weight);

            ulong symbol = this.Symbol;
            symbol <<= 56;

            writer.Write(symbol);

            ulong initialOne = 1;

            writer.Write(initialOne);

            ulong binRepresentation = weight | symbol | initialOne;

            writer.Write(binRepresentation);

            return binRepresentation;
        }
    }
    /// <summary>
    /// Class representing inner node of Huffman tree
    /// </summary>
    public class InnerNode : Node
    {
        public override ulong Weight { get; set; }
        internal int TimeOfCreation { get; set; }
       internal Node Left { get; set; }
        internal Node Right { get; set; }

        public InnerNode(ulong weight, int timeOfCreation)
        {
            this.Weight = weight;
            this.TimeOfCreation = timeOfCreation;
        }
        /// <summary>
        /// To conect two Huffman subtrees to one Huffmen tree with a new created root
        /// </summary>
        /// <param name="left">root of left subtree</param>
        /// <param name="right">root of right subtree</param>
        /// <param name="timeOfCreation">serial number of this Inner node</param>
        public InnerNode(Node left, Node right, int timeOfCreation)
        {
            this.Left = left;
            this.Right = right;
            this.TimeOfCreation = timeOfCreation;
            ulong weight = (left.Weight + right.Weight);
            this.Weight = weight;
        }
        /// <summary>
        /// Codes InnerNode to binary as required by Huffman II assigment
        /// </summary>
        /// <returns>ulong containtg bin representation of InnerNode</returns>
        public override ulong NodeToBinary()
        {
            //first bit zero - idicates InnerNode
            //56 bits of weight
            //8 trailing bits zero
            ulong weight = this.Weight;
            weight <<= 9; //pick lower 55 bits of weight - 9 zeroes to end
            weight >>= 8; //one zero to begining, 8 zeroes to end  
            //writer.Write(weight);
            return weight;
        }

    }
    /// <summary>
    /// Comparer of Nodes, Implements all rules, by which nodes are compared
    /// </summary>
    class Comparer : Comparer<Node>
    {
        public override int Compare(Node one, Node two)
        {
            //most important factor are weights
            if (one.Weight < two.Weight)
            {
                return -1;
            }
            else if (one.Weight > two.Weight)
            {
                return 1;
            }
            else
            {
                //leaves are lighter then inner nodes
                if (one is Leaf && two is InnerNode)
                {
                    return -1;
                }
                else if (one is InnerNode && two is Leaf)
                {
                    return 1;
                }
                else if (one is Leaf && two is Leaf)
                {
                    Leaf first = (Leaf)one;
                    Leaf second = (Leaf)two;
                    //of two leves the one with lower Symbol is lighter
                    if (first.Symbol < second.Symbol)
                    {
                        return -1;
                    }
                    else if (first.Symbol > second.Symbol)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else if (one is InnerNode && two is InnerNode)
                {
                    InnerNode first = (InnerNode)one;
                    InnerNode second = (InnerNode)two;
                    //of two inner nodes, the one that had been created sooner is lighter
                    if (first.TimeOfCreation < second.TimeOfCreation)
                    {
                        return -1;
                    }
                    else if (first.TimeOfCreation > second.TimeOfCreation)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
