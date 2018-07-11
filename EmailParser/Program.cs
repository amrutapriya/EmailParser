using System;
using System.IO;


namespace EmailParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please select a file to parse");
            Console.WriteLine("1. TestEmail1.txt");
            Console.WriteLine("2. TestEmail2.txt");
            Console.WriteLine("Enter 1 or 2");
            string input = Console.ReadLine();
            ValidateInput(input);
            EmailParser parser = new EmailParser();
            parser.Parse(Path.Combine(Directory.GetCurrentDirectory() + "\\Resources\\testemail" + input + ".txt"));
        }

        public static void ValidateInput(string input)
        {
            if (!(input.Equals("1") || (input.Equals("2"))))
            {
                Console.WriteLine("Input entered is not correct.");
                Environment.Exit(-1);
            }
        }
    }
}
