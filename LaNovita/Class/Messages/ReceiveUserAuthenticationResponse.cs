using System.Collections.Generic;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveUserAuthenticationResponse : ServiceResponseBase
    {
 
        public string Name;
        public int BuyerID;
        public bool Admin;
        public string PushToken;
        public string SerialKey;
        public string PhoneNumber;
        public string Email;

        public ReceiveUserAuthenticationResponse()
        {
            //ServiceResponseBase
            ErrorCode = "";
            ErrorMessage = "";
            Playlist = "";

            //Current type
            Name = "";
            PushToken = "";
            SerialKey = "";
            PhoneNumber = "";
            Email = "";
        }
    }
}