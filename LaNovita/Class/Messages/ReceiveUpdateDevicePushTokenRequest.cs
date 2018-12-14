namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveUpdateUserDeviceRequest
    {
        public string Identification { get; set; }

        public string SerialKey { get; set; }
        public string PushToken { get; set; }


    }
}