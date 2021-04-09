using System;
using System.IO;


namespace App
{
   
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string fpath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString()+"\\";
           
            string input_fname = @"train_46189_1Left_0.csv";
            string output_fname = @"output.csv";

            Double a = 0.9;
            char split_symbol = ';';

            SecondSolution.ProcessFileToFindNonStrictExtrema(fpath + input_fname, fpath + output_fname, a, split_symbol);
        }
    }
}
