namespace donations.blazor.app.Data.Models
{
  public class Merchant
  {
    public int merchant_id { get; set; }
    public string merchant_key { get; set; }
    public string return_url { get; set; }
    public string cancel_url { get; set; }
    public string notify_url { get; set; }
  }
}
