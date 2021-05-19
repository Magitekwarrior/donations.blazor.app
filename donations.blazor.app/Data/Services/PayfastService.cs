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

    public async Task<bool> SubmitPayment(decimal donationAmount)
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

        HttpContent content = new FormUrlEncodedContent(form);
        var result = httpClient.PostAsync(url, content);

        //var result = await url.AllowAnyHttpStatus().PostAsync(content);
        //var result = await url.AllowAnyHttpStatus().PostUrlEncodedAsync(new
        //{
        //  payfastPayment.merchant_id,
        //  payfastPayment.merchant_key,
        //  payfastPayment.amount,
        //  payfastPayment.item_name
        //});

        //if (result.StatusCode == (int)HttpStatusCode.OK)

        if (result.Result.StatusCode == HttpStatusCode.OK)
          return true;

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

      return false;
    }

  }
}
