namespace donations.blazor.app.Data.Models
{
  public class Payment
  {
    public int merchant_id { get; set; }
    public string merchant_key { get; set; }
    public decimal amount { get; set; }
    public string item_name { get; set; }
  }

  public class PaymentMethod
  {
    public string payment_method { get; set; }
  }
}
