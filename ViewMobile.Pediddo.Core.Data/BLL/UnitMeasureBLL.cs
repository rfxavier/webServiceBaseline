﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class UnitMeasureBLL
    {
        public static UnitMeasure GetBySymbol(string symbol)
        {
            return  UnitMeasureDAL.GetBySymbol(symbol);
        }

    }
}
