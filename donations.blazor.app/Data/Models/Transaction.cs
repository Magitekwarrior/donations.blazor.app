namespace donations.blazor.app.Data.Models
{
  public class Transaction
  {
    public string m_payment_id { get; set; }
    public decimal amount { get; set; }
    public string item_name { get; set; }
    public string item_description { get; set; }
    public int custom_int1 { get; set; }
    public string custom_str1 { get; set; }
  }

  public class TransactionOptions
  {
    public bool email_confirmation { get; set; }
    public string confirmation_address { get; set; }
  }


}
