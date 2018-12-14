namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendNewBillRequest
    {
        public int? CustomerID;
        public string ProductName { get; set; }
        public decimal Value { get; set; }
        public string Obs { get; set; }
        public bool? Paid { get; set; }
    }
}