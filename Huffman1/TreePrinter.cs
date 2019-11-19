using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Huffman1
{
    /// <summary>
    /// class cotainig functions to print Huffman tree
    /// </summary>
    public static class TreePrinter
    {
        /// <summary>
        /// recursive :(
        /// prints Huffman tree in visually more pleasant way
        /// root is the leftes member printed, its right subtree is higher, left subtree lower
        /// not from the assigment, just fancy tool to observe trees
        /// </summary>
        /// <param name="root"></param>
        /// <param name="TabCounter"></param>
        public static void PrintNiceTree(Node root, int TabCounter)
        {
            if(root is Leaf)
            {
                for (int i = 0; i < TabCounter; i++)
                {
                    Console.Write("\t");
                }
                Leaf l = (Leaf)root;
                Console.WriteLine($"*{l.Symbol}:{l.Weight}");
                return;
            }
            InnerNode node = (InnerNode)root;
            TabCounter++;
            PrintNiceTree(node.Right, TabCounter);
            TabCounter--;
            for (int i = 0; i < TabCounter; i++)
            {
                Console.Write("\t");
            }
            InnerNode n = (InnerNode)root;
            Console.WriteLine($"{n.Weight}");
            TabCounter++;
            PrintNiceTree(node.Left, TabCounter);
        }
        /// <summary>
        /// recursive :(
        /// prints Huffman tree in prefix notation on one line
        /// inner nodes in format WEIGHT LEFT subtree RIGHT subtree
        /// leaves are in format *SYMBOL:WEIGHT
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="FirstRecord">when function is called true shoud be passed</param>
        /// <param name="Writer"></param>
        private static void PrintCompresedTree(Node Root, bool FirstRecord, StreamWriter Writer)
        {
            if (Root is Leaf)
            {
                //print leaf, baser case of recursion
                Leaf l = (Leaf)Root;
                //do not add space before the first record printed
                if (FirstRecord)
                {
                    FirstRecord = false;
                }
                else
                {
                    Writer.Write(" ");
                }
                Writer.Write($"*{l.Symbol}:{l.Weight}");
               // Writer.Flush();
                return;
            }
            InnerNode node = (InnerNode)Root;
            //do not add space before the first record printed
            if (FirstRecord)
            {
                FirstRecord = false;
            }
            else
            {
                Writer.Write(" ");
            }
            //prefix notation
            //first print node
            Writer.Write($"{node.Weight}");
           // Writer.Flush();
            //then recursively print its left subtree
            PrintCompresedTree(node.Left, false, Writer);
            //then recursively print its right subtree
            PrintCompresedTree(node.Right, false, Writer);
        }
        /// <summary>
        /// recursive
        /// writes binary representation of Huffman tree as if was specified by Huffman II assigment to given binary writer
        /// </summary>
        /// <param name="root">root of tree to be printed</param>
        /// <param name="writer"></param>
        public static void PrintTreeBinary(Node root, BinaryWriter writer)
        {
            //print actuall root, implementation of NodeToBinary differs for leaf and inner node
            ulong record =root.NodeToBinary();
            writer.Write(record);
            if(root is InnerNode)
            { 
                //recursively print subtrees
                InnerNode node = (InnerNode)root;
                PrintTreeBinary(node.Left, writer);
                PrintTreeBinary(node.Right, writer);
            }
        }

        /// <summary>
        /// prints Huffman tree in prefix notation on one line
        /// inner nodes in format WEIGHT LEFT subtree RIGHT subtree
        /// leaves are in format *SYMBOL:WEIGHT
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="writer"></param>
        /// 
        //encapsulation of private recursive method, provides constant parametr (important for recursive calls) and flushes 
        public static void PrintCompresedTree(Node Root, StreamWriter writer)
        {
            PrintCompresedTree(Root, true, writer);
            writer.Flush();
        }
    }
}
