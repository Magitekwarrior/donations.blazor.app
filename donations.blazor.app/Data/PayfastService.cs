using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace donations.blazor.app.Data
{
  public class PayfastService: IPayfastService
  {
    private HttpClient _client;


    public PayfastService(HttpClient client)
    {
      _client = client;
    }

    public Task SubmitPayment()
    {
      throw new ApplicationException();
    }

  }
}
