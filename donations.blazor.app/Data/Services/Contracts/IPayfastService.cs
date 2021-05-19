using donations.blazor.app.Data.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace donations.blazor.app.Data.Services.Contracts
{
  public interface IPayfastService
  {
    Task<HttpResponseMessage> SubmitPayment(decimal donationAmount);
    Task<Merchant> GetPaymentDetails();
  }
}
