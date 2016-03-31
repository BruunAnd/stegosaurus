﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus
{
    interface IStegoAlgorithm
    {

        StegoCarrier Carrier { get; set; }

        long ComputeBandwidth();

        void Embed(StegoMessage message);

        StegoMessage Extract();
    }
}
