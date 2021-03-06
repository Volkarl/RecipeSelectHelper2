﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public static class Prettyprinter
    {
        public static string ToPrettyString(BoughtProduct bp)
        {
            return $"{bp.CorrespondingProduct}\n" + 
                   $"{bp.ExpirationData}\n" + 
                   $"{bp.OwnValue}";
        }
    }
}
