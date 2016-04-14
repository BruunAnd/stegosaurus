using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stegosaurus.Carrier;
using System.Collections;

namespace Stegosaurus.Algorithm
{
    /// <summary>
    /// Placeholder algorithm. Direct linear LSB implementation.
    /// </summary>
    public class PlaceholderAlgorithm : IStegoAlgorithm
    {

        public ICarrierMedia CarrierMedia { get; set; }
        /// <summary>
        /// This algorithm returns the data capacity of the carrier media with the given stegoAlgorithm.
        /// </summary>
        /// <param name="_CarrierMedia"></param>
        /// <returns></returns>
        public long ComputeBandwidth(ICarrierMedia _CarrierMedia)
        {
            return _CarrierMedia.ByteArray.Length / 8;
        }

        /* SKJUL FILER SAMT EN STRING I CARRIERMEDIET */
        /// <summary>
        /// This method converts the received bytearray to a bitarray, 
        /// and compares each bit of this array to the least significant bit of the corresponding byte in the CarrierMedia bytearray. 
        /// If these are not equal the CarrierMedia bytearray is changed accordingly.
        /// The length of the message bitarray is converted into a bitarray that is similarly is 
        /// embedded in the end of the CarrierMedia bytearray.
        /// </summary>
        /// <param name="_message"></param>
        public void Embed(StegoMessage _message)
        {
            BitArray messageInBits = new BitArray(_message.Bytes);
            bool carrierBit;
            for (int index = 0; index < messageInBits.Length; index++)
            {
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

        /// <summary>
        /// This method reads the last bits of each byte of the CarrierMedia to get the message.
        /// </summary>
        /// <param name="_CarrierMedia"></param>
        /// <returns></returns>
        public StegoMessage Extract(ICarrierMedia _CarrierMedia)
        {
            BitArray[] messageSizeBitArray = new BitArray[8 * sizeof(int)];
            int headerSize = 8 * sizeof(int);
            for (int index = 0; index < headerSize; index++)
            {
                messageSizeBitArray.SetValue((_CarrierMedia.ByteArray[index] % 2) == 1, index);
            }

            int[] messageSizeBytes = new int[1];
            messageSizeBitArray.CopyTo(messageSizeBytes, 0);
            Byte[] stegoMessage = new byte[messageSizeBytes[0]];
            int messageSizeBits = messageSizeBytes[0] * 8;
            BitArray[] message = new BitArray[messageSizeBits];

            for (int index = headerSize; index < (messageSizeBits + headerSize); index++)
            {
                message.SetValue((_CarrierMedia.ByteArray[index] % 2) == 1, index);
            }

            message.CopyTo(stegoMessage, 0);
            return new StegoMessage(stegoMessage);
        }



    }
}
