namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveUserAuthenticationRequest
    {
        //string Identification, string Email, string Password, string PushToken, string DeviceOS
        public string Identification { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string PushToken { get; set; }
        public string DeviceOs { get; set; }
    }
}