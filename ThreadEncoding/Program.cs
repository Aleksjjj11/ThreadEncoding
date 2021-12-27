using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadEncoding
{
    internal class Program
    {
        private static Encoding Encoding;

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding = Encoding.GetEncoding("windows-1251");
            var generator = new Generator(3485);

            var key = "111112";

            while (true)
            {
                var array = new[]
                {
                    generator.GetNumber().ToString(), generator.GetNumber().ToString(), generator.GetNumber().ToString()
                };

                foreach (var s in array)
                {
                    Console.Out.WriteLine($"Before: {s}");
                    var cipherText = XORStrimmed(Encoding.GetBytes(key), Encoding.GetBytes(s)).ToArray();

                    var decripted = XORStrimmed(Encoding.GetBytes(key), cipherText);

                    var decriptedStr = Encoding.GetString(decripted.ToArray());

                    Console.Out.WriteLine($"Encode: {Encoding.GetString(cipherText.ToArray())}");
                    Console.Out.WriteLine($"Decode: {decriptedStr}");
                }

                Console.Out.WriteLine("Enter any key to continue");
                Console.ReadKey();
            }

            
        }

        static IEnumerable<byte> XORStrimmed(byte[] gamma, IEnumerable<byte> data)
        {

            var gammaIndex = 0;
            foreach (var bb in data)
            {
                // XOR
                yield return (byte)(bb ^ gamma[gammaIndex]);

                if (gammaIndex < gamma.Length - 1)
                    gammaIndex++;
                else
                    gammaIndex = 0;
            }
        }
    }

    class Generator
    {
        protected int Value = 0;

        public Generator(int initialValue)
        {
            Value = initialValue;
        }


        public int GetNumber()
        {
            string stepValue = Convert.ToInt32(Math.Pow(Value, 2)).ToString();
            int startIndex = stepValue.Length / 4;
            int len = Value.ToString().Length;

            Value = int.Parse(stepValue.Substring(startIndex, len));

            return Value;
        }
    }
}
