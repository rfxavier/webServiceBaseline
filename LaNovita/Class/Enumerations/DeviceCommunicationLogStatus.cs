using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Enumerations
{
    public enum DeviceCommunicationLogStatus
    {
        Success,
        NullXml,
        InvalidXml,
        InvalidDevice,
        KnownError,
        UnknownError,
    }
}