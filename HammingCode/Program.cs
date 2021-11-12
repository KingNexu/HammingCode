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
                block[0] = SetControlParityBit();
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

        //Sets Control Parity Bit
        private int SetControlParityBit()
        {
            if ((block.Sum() % 2) == 0)
                return 0;
            else
                return 1;
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
        public bool IsPowerOfTwo(ulong x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        //Gets The XOR of a List
        public int XorOfList(List<int> list, int n)
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
        List<int> data = new List<int>();
        int error;

        public DecodeHammingCode(int[] messagedata)
        {
            if(IsPowerOfTwo((ulong)messagedata.Length) == false)
                Console.Error.WriteLine("Error: Data To long or short");
            else
            {
                //Convert to List
                this.data = messagedata.ToList();

                //Write the Error
                error = checkBits();
                if (error == 0)
                    Console.WriteLine("No Error Detected");
                else
                {
                    string correctedBlock = CorrectedBlock(error);
                    if (checkForMultiError() == false)
                    {
                        Console.WriteLine("Multiple Errors detected");
                    }
                    else
                    {
                        Console.WriteLine("Error on bit: " + error);
                        Console.WriteLine("Corrected Block: " + CorrectedBlock(error));
                    }
                }



            }
        }

        //Check for Error
        private int checkBits()
        {
            int posData = 0;
            int returnBit = 0;
            List<int> aktiveBits = new List<int>();

            foreach(int bit in this.data)
            {
                if(bit == 1)
                {
                    string binary = Convert.ToString(bit, 2);
                    aktiveBits.Add(posData);
                }
                posData++;
            }
            returnBit = XorOfList(aktiveBits, aktiveBits.Count);
            return returnBit;
        }

        //Check for Multi errror
        private bool checkForMultiError()
        {
            if ((this.data.Sum() % 2) == 0)
                return true;
            else
                return false;
        }

        //CorrectString
        private string CorrectedBlock(int error)
        {
            //flips bit
            this.data[error] = 1 - data[error];

            //returns list as string
            string joinedString = null;

            foreach (var item in this.data)
            {
                joinedString += item + " ";
            }
            return joinedString;

        }

        /*
         *    MATH
         */

        //Gets The XOR of a List
        private int XorOfList(List<int> list, int n)
        {
            int xor_list = 0;

            for (int i = 0; i < n; i++)
            {
                xor_list = xor_list ^ list[i];
            }

            return xor_list;
        }

        private bool IsPowerOfTwo(ulong x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }
    }


    class Program
    {

        static void Main(string[] args)
        {
            int[] sampleDataEncode = {0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0};
            int[] sampleDataDecode = {1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1,0 };
            

            EncodeHammingCode Hamming1 = new EncodeHammingCode(sampleDataEncode, 16);
            DecodeHammingCode Hamming2 = new DecodeHammingCode(sampleDataDecode);

        }

    }
}
