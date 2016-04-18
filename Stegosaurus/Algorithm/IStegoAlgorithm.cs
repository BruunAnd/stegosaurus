using System;
using Stegosaurus.Carrier;
using System.Collections;

namespace Stegosaurus.Algorithm
{
    interface IStegoAlgorithm
    {
        string Name { get; }

        /* CARRIERMEDIET INDEHOLDER DE FORSKELLIGE FILER SAMT BESKEDEN */
        ICarrierMedia CarrierMedia { get; set; }

        /* BEREGN MÆNGDEN AF PLADS I CARRIERMEDIET */
        long ComputeBandwidth(ICarrierMedia CarrierMedia);

        /* SKJUL FILER SAMT EN STRING I CARRIERMEDIET */
        void Embed(StegoMessage message);

        /* HENT FILER SAMT EN STRING FRA CARRIERMEDIET */
        StegoMessage Extract(ICarrierMedia CarrierMedia);
    }
}
