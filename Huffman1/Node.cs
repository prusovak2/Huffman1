using System;
using System.Collections.Generic;
using System.Text;

namespace Huffman1
{
    /// <summary>
    /// Abstract class from which nodes of Huffman tree are to be derived
    /// </summary>
    public abstract class Node
    {      
        public abstract int Weight { get; set; }
        //public abstract int TimeOfCreation { get; set; }
                  
    }
    /// <summary>
    /// Class representing a leaf of Huffman tree
    /// </summary>
    public class Leaf : Node
    {
        public override int Weight { get; set; }
        public byte Symbol { get; set; }

        public Leaf(byte Symbol, int Weight)
        {
            this.Symbol = Symbol;
            this.Weight = Weight;
        }
    }
    /// <summary>
    /// Class representing inner node of Huffman tree
    /// </summary>
    public class InnerNode : Node
    {
        public override int Weight { get; set; }
        internal int TimeOfCreation { get; set; }
       internal Node Left { get; set; }
        internal Node Right { get; set; }

        public InnerNode(int weight, int timeOfCreation)
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
            int weight = (left.Weight + right.Weight);
            this.Weight = weight;
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
