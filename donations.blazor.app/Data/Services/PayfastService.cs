using donations.blazor.app.Data.Config;
using donations.blazor.app.Data.Models;
using donations.blazor.app.Data.Services.Contracts;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace donations.blazor.app.Data.Services
{
  public class PayfastService : IPayfastService
  {
    private readonly AppSettings _appSettings;
    private readonly ServicesEndpoints _servicesEndpoints;
    private readonly IHttpClientFactory _httpClientFactory;

    public PayfastService(IOptions<AppSettings> appSettings,
      IOptions<ServicesEndpoints> servicesEndpoints,
      IHttpClientFactory httpClientFactory)
    {
      _appSettings = appSettings.Value;
      _servicesEndpoints = servicesEndpoints.Value;
      _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<Merchant> GetPaymentDetails()
    {
      Merchant merchant = new Merchant()
      {
        merchant_id = _appSettings.MerchantId,
        merchant_key = _appSettings.MerchantKey,
        item_name = _appSettings.ItemName,         
        cancel_url = _appSettings.ReturnUrl,
        notify_url = _appSettings.NotifyUrl,
        return_url = _appSettings.ReturnUrl
      };

      return merchant;
    }

    public async Task<HttpResponseMessage> SubmitPayment(decimal donationAmount)
    {
      var httpClient = _httpClientFactory.CreateClient();
      httpClient.BaseAddress = new Uri(_servicesEndpoints.Services["Payfast"].Url);
      httpClient.Timeout = new TimeSpan(0, 0, 30);
      httpClient.DefaultRequestHeaders.Clear();

      Payment payfastPayment = new Payment()
      {
        merchant_id = _appSettings.MerchantId,
        merchant_key = _appSettings.MerchantKey,
        amount = donationAmount,
        item_name = _appSettings.ItemName
      };

      var url = _servicesEndpoints.Services["Payfast"].Url;

      try
      {
        var json = JsonConvert.SerializeObject(payfastPayment);
        var form = new List<KeyValuePair<string, string>>
        {
          KeyValuePair.Create("merchant_id", payfastPayment.merchant_id.ToString()),
          KeyValuePair.Create("merchant_key", payfastPayment.merchant_key),
          KeyValuePair.Create("amount", payfastPayment.amount.ToString()),
          KeyValuePair.Create("item_name", payfastPayment.item_name)
        };

        //HttpContent content = new FormUrlEncodedContent(form);
        //var result = await httpClient.PostAsync(url, content);

        var jsonContent = JsonConvert.SerializeObject(payfastPayment);
        StringContent postData = new StringContent(jsonContent, Encoding.UTF8, "application/x-www-form-urlencoded");
        using (HttpResponseMessage result = httpClient.PostAsync(url, postData).Result)
        {
          string resultJson = result.Content.ReadAsStringAsync().Result;

          return result;
        }

        //var result = await url.AllowAnyHttpStatus().PostAsync(content);
        //var result = await url.AllowAnyHttpStatus().PostUrlEncodedAsync(new
        //{
        //  payfastPayment.merchant_id,
        //  payfastPayment.merchant_key,
        //  payfastPayment.amount,
        //  payfastPayment.item_name
        //});

        //if (result.StatusCode == HttpStatusCode.OK)
        //  return result;

      }
      catch (Exception ex)
      {
        var error = ex.Message;
        Log.Error($"Error returned from: {ex.InnerException}");
      }
      //catch (FlurlHttpTimeoutException ex)
      //{
      //  // handle timeout
      //}
      //catch (FlurlHttpException ex)
      //{
      //  Log.Error($"Error returned from {ex.Call.Request.Url}: {ex.InnerException}");
      //}

      return new HttpResponseMessage();
    }

  }
}
