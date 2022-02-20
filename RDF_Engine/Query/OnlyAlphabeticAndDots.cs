﻿
using BH.oM.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.RDF
{
    public static partial class Query
    {
        public static string OnlyAlphabeticAndDots(this string str)
        {
            return new String(str.Where(c => char.IsLetter(c) || c == '.').ToArray());
        }
    }
}
