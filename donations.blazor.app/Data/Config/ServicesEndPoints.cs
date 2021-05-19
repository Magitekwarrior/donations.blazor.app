using System.Collections.Generic;

namespace donations.blazor.app.Data.Config
{
  public class ServicesEndpoints
  {
    public Dictionary<string, ServiceAttributes> Services { get; set; } // Dictionaries must have string keys
  }

  public partial class ServiceAttributes
  {
    public string Url { get; set; }
  }
}
