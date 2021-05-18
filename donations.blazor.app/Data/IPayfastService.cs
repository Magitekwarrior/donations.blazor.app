using System.Threading.Tasks;

namespace donations.blazor.app.Data
{
  public interface IPayfastService
  {
    Task SubmitPayment();
  }
}
