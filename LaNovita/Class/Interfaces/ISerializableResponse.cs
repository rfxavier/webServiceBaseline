using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Interfaces
{
    public interface ISerializableResponse
    {
        byte[] Serialize();
    }
}