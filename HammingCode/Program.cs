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
                //Set Block
                block = new List<int>(new int[size]);
                Console.WriteLine("Success");
                List<int> data = messageData.ToList();
                this.data = data;
                block = SetBlock(size - 1);

                //Set Parity
                foreach (var item in parityBits)
                {
                    block[item] = SetParity(size - 1, item);
                }
                
                //Output Block
                Console.WriteLine(GetBlock());
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
                    //Write Data to Block
                    block.Add(this.data[posData]);

                    posData++;
                    posBlock++;
                }
            }
                       
            block.RemoveAt(block.Count - 1);
            parityBits.RemoveAt(parityBits.Count - 1);
            return block;
        }

        private int SetParity(int size, int posParityBit)
        {

            int posBlock = posParityBit, parityValue = 0;
            int readCount = 0;
            List<int> paritySum = new List<int>();
            
            //While there are still bits in data write those bits to a list and output if the value is  even
            do
            {
                if (readCount < posParityBit)
                {
                    for (int i = 0; i < posParityBit; i++)
                    {
                        if (posBlock < size)
                            if (block[posBlock] == 3)
                            {
                                paritySum.Add(0);
                                readCount++;
                                posBlock++;
                            }
                            else
                            {
                                paritySum.Add(block[posBlock]);
                                readCount++;
                                posBlock++;
                            }
                    }

                }
                else
                {
                    posBlock = posBlock + posParityBit;
                    readCount = 0;
                }
            } while (posBlock < size);

            parityValue = XorOfList(paritySum, paritySum.Count);

            return parityValue;
        }

        //Retruns block as string
        private string GetBlock()
        {
            string joinedString = null;

            foreach(var item in this.block)
            {
                joinedString += item + " "; 
            }
            return joinedString;
        }

        /*
         *    MATH
         */

        //Checks if number is Power of 2
        private bool IsPowerOfTwo(ulong x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        //Gets The XOR of a List
        private int XorOfList(List<int> list, int n)
        {
            int xor_list = 0;

            for(int i = 0; i < n; i++)
            {
                xor_list = xor_list ^ list[i];
            }

            return xor_list;
        }

    }



    //Decode HammingCode
    class DecodeHammingCode
    {

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
