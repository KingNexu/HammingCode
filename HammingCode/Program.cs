using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammingCode
{
    class EncodeHammingCode
    {
        List<int> data = new List<int>();
        List<int> block;
        List<int> parityBits = new List<int>();

        //Constructor
        public EncodeHammingCode(int[] messageData, int size)
        {

            //Check Size of Data
            if (messageData.Length != ((size - Math.Log(size, 2)) - 1))
                Console.Error.WriteLine("Error: Data To long or short");
            else
            {
                block = new List<int>(new int[size]);
                Console.WriteLine("Success");
                List<int> data = messageData.ToList();
                this.data = data;
                block = SetBlock(size - 1);
            }


        }

        //Sorts out Parity bits and writes data to the other bits
        private List<int> SetBlock(int size)
        {
            //tempoary list
            int posData = 0, posBlock = 1;
            List<int> block = new List<int>();
            block.Add(0);

            foreach (var item in this.block)
            {
                if (IsPowerOfTwo((ulong)posBlock) == true)
                {
                    //Write Position of Parity bit to List
                    parityBits.Add(posBlock);
                    block.Add(3);

                    posBlock++;
                }
                else
                {
                    block.Add(this.data[posData]);

                    posData++;
                    posBlock++;
                }
            }

            int count = 0;
            foreach(int item in block)
            {
                Console.Write(item + " -- ");
                Console.Write(count + "\n");
                count++;
            }

            block.RemoveAt(block.Count - 1);
            parityBits.RemoveAt(parityBits.Count - 1);
            return block;
        }

        private int SetParity(int posParity)
        {
            //Declare
            int parityValue = 0;




            return parityValue;
        }

        /*
         *    MATH  
         * 
         * 
         */
        private bool IsPowerOfTwo(ulong x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

    }
    class Program
    {

        static void Main(string[] args)
        {
            int[] sampleData = {0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0};
            

            EncodeHammingCode Hamming1 = new EncodeHammingCode(sampleData, 16);
        }

    }
}
