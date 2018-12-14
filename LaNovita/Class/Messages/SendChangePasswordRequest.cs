namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendChangePasswordRequest
    {
        public string Identification { get; set; }
        public string SerialKey { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}