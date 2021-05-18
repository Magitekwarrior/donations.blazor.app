namespace donations.blazor.app.Data.Models
{
  public class Merchant
  {
    public int merchant_id { get; set; }
    public int merchant_key { get; set; }
    public int return_url { get; set; }
    public int cancel_url { get; set; }
    public int notify_url { get; set; }
  }
}
