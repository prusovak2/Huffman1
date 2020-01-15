using System;
using System.Collections.Generic;
using System.IO;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Huffman1Tests")]
namespace Huffman1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("File Error");
            HuffIII(args);
        }

        static void HuffIII(string[] args)
        {
            bool opened= Program.Huff3OpenFiles(args, out BinaryReader reader, out BinaryWriter writer);
            if (!opened)
            {
                return;
            }
            Huff3TreeBuilder b = new Huff3TreeBuilder();

            bool success = b.ReadHeaderAndTreeFromBinaryFile(reader);

            if (!success)
            {
                return;
            }
            Huff3Decompresor.DecompressFile(reader, writer, b);
            reader.Close();
            writer.Close();
        }

        //https://github.com/prusovak2/Huffman1 some unit test available for both Huffman I and II
        static void HuffmanII(string[] args)
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

        public static bool Huff3OpenFiles(string[] args, out BinaryReader Reader, out BinaryWriter Writer)
        {
            if (args.Length != 1)
            {
                Reader = null;
                Writer = null;
                Console.WriteLine("Argument Error");
                return false;
            }
            string inputFile = args[0];
            if(!inputFile.EndsWith(".huff"))
            {
                Reader = null;
                Writer = null;
                Console.WriteLine("Argument Error");
                return false;
            }
            string[] splitted = inputFile.Split('\\');
            int len = splitted.Length;
            if (splitted[len-1].Equals(".huff"))
            {
                Reader = null;
                Writer = null;
                Console.WriteLine("Argument Error");
                return false;
            }
            try
            {
                Stream s = File.OpenRead(inputFile);
                Reader = new BinaryReader(s);
            }
            catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is FileNotFoundException || e is DirectoryNotFoundException || e is IOException || e is NotSupportedException || e is UnauthorizedAccessException)
            {
                Writer = null;
                Reader = null;
                Console.WriteLine("File Error");
                return false;
            }
            string outputFile = inputFile.Substring(0, inputFile.Length - 5);
            try
            {
                Stream o = File.OpenWrite(outputFile);
                Writer = new BinaryWriter(o);
            }
            catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is IOException || e is NotSupportedException || e is UnauthorizedAccessException || e is PathTooLongException || e is DirectoryNotFoundException)
            {
                Writer = null;
                Reader = null;
                Console.WriteLine("File Error");
                return false;
            }
            return true;
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

        public static bool OpenFile(string[] args, out BinaryReader Reader, out BinaryWriter Writer)
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

        //huff 1
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

  
    

