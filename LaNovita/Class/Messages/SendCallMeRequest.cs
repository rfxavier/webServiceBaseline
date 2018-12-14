namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendCallMeRequest
    {
        public string Identification { get; set; }

        public string SerialKey { get; set; }
        public string SupplierID { get; set; }
        public string SellerID { get; set; }
    }
}