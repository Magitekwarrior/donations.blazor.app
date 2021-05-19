using System;

namespace donations.blazor.app.Data.Config
{
  public class AppSettings
  {
    public AppSettings()
    {
      MerchantId = 0;
      MerchantKey = string.Empty;
      ItemName = string.Empty;
      ReturnUrl = string.Empty;
      CancelUrl = string.Empty;
      NotifyUrl = string.Empty;
    }

    public int MerchantId { get; set; }
    public string MerchantKey { get; set; }
    public string ItemName { get; set; }
    public string ReturnUrl { get; set; }
    public string CancelUrl { get; set; }
    public string NotifyUrl { get; set; }
  }
}
