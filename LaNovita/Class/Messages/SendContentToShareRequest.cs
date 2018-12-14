using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendContentToShareRequest : ServiceRequestBase
    {
        public string ContentCode { get; set; }
        public List<Contact> ContactList { get; set; }
        public class Contact
        {
            public int TypeId { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }

    }
}