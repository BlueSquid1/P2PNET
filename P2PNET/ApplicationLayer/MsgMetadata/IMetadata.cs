﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer.MsgMetadata
{
    interface IMetadata
    {
        string MsgType { get; }
        string Version { get; }
    }
}