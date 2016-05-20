using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stegosaurus.Carrier;

namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Sample
    {
        public byte[] Values;

        public byte ModValue;
        public byte TargetModValue;
        
        public Sample(byte[] _values)
        {
            Values = _values;
        }

        public void UpdateModValue(byte _bitwiseModFactor)
        {
            ModValue = (byte) (Values.Sum(val => val) & _bitwiseModFactor);
        }

        /// <summary>
        /// Returns a list of all samples in the CarrierMedia.
        /// </summary>
        public static List<Sample> GetSampleListFrom(ICarrierMedia _carrierMedia, byte _bitwiseModFactor)
        {
            List<Sample> sampleList = new List<Sample>(_carrierMedia.ByteArray.Length / _carrierMedia.BytesPerSample);

            int currentSample = 0;
            while (currentSample < _carrierMedia.ByteArray.Length)
            {
                byte[] sampleValues = new byte[_carrierMedia.BytesPerSample];

                for (int i = 0; i < _carrierMedia.BytesPerSample; i++)
                {
                    sampleValues[i] = _carrierMedia.ByteArray[currentSample++];
                }

                // Add new sample to list.
                Sample sample = new Sample(sampleValues);
                sample.UpdateModValue(_bitwiseModFactor);
                sampleList.Add(sample);
            }

            return sampleList;
        }

        /// <summary>
        /// Swap the values of this sample with another sample.
        /// </summary>
        public void Swap(Sample _otherSample)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                byte temp = Values[i];
                Values[i] = _otherSample.Values[i];
                _otherSample.Values[i] = temp;
            }
        }
    }
}
