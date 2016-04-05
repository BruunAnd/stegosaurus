using System;
using Stegosaurus.Carrier;
using System.Collections;

namespace Stegosaurus.Algorithm
{
    interface IStegoAlgorithm
    {

        /* CARRIERMEDIET INDEHOLDER DE FORSKELLIGE FILER SAMT BESKEDEN */
        ICarrierMedia CarrierMedia { get; set; }

        /* BEREGN MÆNGDEN AF PLADS I CARRIERMEDIET */
        long ComputeBandwidth(ICarrierMedia CarrierMedia);

        /* SKJUL FILER SAMT EN STRING I CARRIERMEDIET */
        void Embed(StegoMessage message);

        /* HENT FILER SAMT EN STRING FRA CARRIERMEDIET */
        StegoMessage Extract(ICarrierMedia CarrierMedia);
    }

    class placeholder : IStegoAlgorithm
    {
        /* CARRIERMEDIET INDEHOLDER DE FORSKELLIGE FILER SAMT BESKEDEN */
        ICarrierMedia CarrierMedia { get; set; }

        /* BEREGN MÆNGDEN AF PLADS I CARRIERMEDIET */
        long ComputeBandwidth(ICarrierMedia CarrierMedia)
        {
            return CarrierMedia.ByteArray.Length;
        }

        /* SKJUL FILER SAMT EN STRING I CARRIERMEDIET */
        /// <summary>
        /// This method converts the received bytearray to a bitarray, 
        /// and compares each bit of this array to the least significant bit of the corresponding byte in the CarrierMedia bytearray. 
        /// If these are not equal the CarrierMedia bytearray is changed accordingly.
        /// The length of the message bitarray is converted into a bitarray that is similarly is 
        /// embedded in the end of the CarrierMedia bytearray.
        /// </summary>
        /// <param name="message"></param>
        void Embed(StegoMessage message)
        {
            BitArray messageInBits = new BitArray(message);

            for (int index = 0; index < messageInBits.Length; index++)
            {
                bool carrierBit;
                
                carrierBit = (CarrierMedia.ByteArray[index] % 2) == 1;

                if (carrierBit != messageInBits[index])
                {
                    if (carrierBit)
                    {
                        CarrierMedia.ByteArray[index]--;
                    }
                    else
                    {
                        CarrierMedia.ByteArray[index]++;
                    }

                }
                
            }

            BitArray header = new BitArray(new int[] { messageInBits.Length });
            for (int index = CarrierMedia.ByteArray.Length - (1 + (8 * sizeof(int))); index < CarrierMedia.ByteArray.Length; index++)
            {
                bool carrierBit;

                carrierBit = (CarrierMedia.ByteArray[index] % 2) == 1;

                if (carrierBit != header[index])
                {
                    if (carrierBit)
                    {
                        CarrierMedia.ByteArray[index]--;
                    }
                    else
                    {
                        CarrierMedia.ByteArray[index]++;
                    }

                }

            }

        }

        BitArray ByteArrayToBitArray()
        {
            return new BitArray 
        }

        /* HENT FILER SAMT EN STRING FRA CARRIERMEDIET */
        StegoMessage Extract(ICarrierMedia CarrierMedia)
        {

        }
}
}
