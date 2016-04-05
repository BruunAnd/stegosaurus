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
        /// <summary>
        /// This algorithm returns the data capacity of the carrier media with the given stegoAlgorithm.
        /// </summary>
        /// <param name="CarrierMedia"></param>
        /// <returns></returns>
        long ComputeBandwidth(ICarrierMedia CarrierMedia)
        {
            return CarrierMedia.ByteArray.Length / 8;
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

        }

        BitArray ByteArrayToBitArray()
        {
            return new BitArray 
        }

        /// <summary>
        /// This method reads the last bits of each byte of the CarrierMedia to get the message.
        /// 
        /// </summary>
        /// <param name="CarrierMedia"></param>
        /// <returns></returns>
        StegoMessage Extract(ICarrierMedia CarrierMedia)
        {
            BitArray[] messageSize = new BitArray[8 * sizeof(int)];
            int messageSizeBytes, messageSizeBits, headerSize = 8 * sizeof(int);
            bool carrierBit;

            for (int index = 0; index < headerSize; index++)
            {
                messageSize[index] = (CarrierMedia.ByteArray[index] % 2) == 1;
            }
            
            messageSize.CopyTo(messageSizeBytes, 0);
            Byte[] stegoMessage = new byte[messageSizeBytes];
            messageSizeBits = messageSizeBytes * 8;
            BitArray[] message = new BitArray[messageSizeBits];

            for (int index = headerSize; index < (messageSizeBits + headerSize); index++)
            {
                message[index] = (CarrierMedia.ByteArray[index] % 2) == 1;
            }

            message.CopyTo(stegoMessage, 0);
            return stegoMessage;
        }
}
}
