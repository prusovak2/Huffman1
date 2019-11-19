using System;
using System.Collections.Generic;
using System.IO;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Huffman1Tests")]
namespace Huffman1
{
    class Program
    {
        //https://github.com/prusovak2/Huffman1 some unit test available
        static void Main(string[] args)
        {
            //try to open file by binary reader
            bool opened = OpenFile(args, out BinaryReader Reader, out BinaryWriter Writer);
            if (!opened)
            {
                return;
            }
            HuffCompresor.CompressFile(Writer, Reader);
            Writer.Close();
        }


        static void Huffman1(string[] args)
        {
            //try to open file by binary reader
            bool opened = OpenFile(args, out BinaryReader Reader, out BinaryWriter Writer);
            if (!opened)
            {
                return;
            }
            //create HuffmanReader, that is going to count occurences of bytes in input file
            HuffmanReader HuffReader = new HuffmanReader(Reader);
            bool NotEmpty = HuffReader.ReadFileUsingBuffer();
            //Build Huffman tree
            TreeBuilder Builder = new TreeBuilder(HuffReader.ProvideSymbolsWihtWeights());
            Node Root = Builder.BuildHuffTree();

            Stream Outputstream = Console.OpenStandardOutput();
            StreamWriter writer = new StreamWriter(Outputstream);
            //Writer.AutoFlush = true;
            Console.SetOut(writer);

            //print Huffman tree
            TreePrinter.PrintCompresedTree(Root, writer);
        }

        private static bool OpenFile(string[] args, out BinaryReader Reader, out BinaryWriter Writer)
        {
            if (args.Length != 1)
            {
                Reader = null;
                Writer = null;
                Console.WriteLine("Argument Error");
                return false;
            }
            try
            {
                Stream s = File.OpenRead(args[0]);
                Reader = new BinaryReader(s);
            }
            catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is FileNotFoundException || e is DirectoryNotFoundException || e is IOException ||e is NotSupportedException||e is UnauthorizedAccessException)
            {
                Writer = null;
                Reader = null;
                Console.WriteLine("File Error");
                return false;
            }
            try
            {
                string outputFile = args[0] +".huff";
                Stream o = File.OpenWrite(outputFile);
                Writer = new BinaryWriter(o);
            }
            catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is IOException || e is NotSupportedException || e is UnauthorizedAccessException|| e is PathTooLongException||e is DirectoryNotFoundException)
            {
                Writer = null;
                Reader = null;
                Console.WriteLine("File Error");
                return false;
            }
            return true;

        }

        public static Node ReadFileAndBuildTree(BinaryReader Reader)
        {
            //create HuffmanReader, that is going to count occurences of bytes in input file
            HuffmanReader HuffReader = new HuffmanReader(Reader);
            bool NotEmpty = HuffReader.ReadFileUsingBuffer();
            //Build Huffman tree
            TreeBuilder Builder = new TreeBuilder(HuffReader.ProvideSymbolsWihtWeights());
            Node Root = Builder.BuildHuffTree();
            return Root;
        }
    }
}

  
    

