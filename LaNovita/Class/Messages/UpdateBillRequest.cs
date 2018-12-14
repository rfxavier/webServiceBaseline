namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class UpdateBillRequest
    {
        public int BillID;
        public string ProductName { get; set; }
        public decimal Value { get; set; }
        public string Obs { get; set; }
        public bool? Paid { get; set; }
    }
}