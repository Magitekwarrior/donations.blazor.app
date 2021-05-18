using donations.blazor.app.Data.Models;
using Flurl.Http;
using System.Threading.Tasks;

namespace donations.blazor.app.Data
{
  public class PayfastService: IPayfastService
  {
    public PayfastService()
    {

    }

    public async Task SubmitPayment(decimal donationAmount)
    {
      Payment payfastPayment = new Payment()
      {
        merchant_id = 10000100, // appsettings.json
        merchant_key = "46f0cd694581a", // appsettings.json
        amount = donationAmount,
        item_name = "Don Sama" // appsettings.json
      };

      var url = "https://sandbox.payfast.co.za​/eng/process";
      var result = await url.GetJsonAsync();
    }

  }
}
