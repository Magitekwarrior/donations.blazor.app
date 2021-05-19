using System.Threading.Tasks;

namespace donations.blazor.app.Data.Services.Contracts
{
  public interface IPayfastService
  {
    Task<bool> SubmitPayment(decimal donationAmount);
  }
}
