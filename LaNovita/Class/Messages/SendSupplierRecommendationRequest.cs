namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendSupplierRecommendationRequest
    {
        public string Identification { get; set; }
        public string SerialKey { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public int SegmentID { get; set; }
    }
}