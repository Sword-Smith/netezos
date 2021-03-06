﻿using System;

namespace Netezos.Rpc
{
    static class DateTimeExtension
    {
        static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static int ToEpoch(this DateTime datetime)
        {
            return (int)(datetime - Epoch).TotalSeconds;
        }
    }
}
