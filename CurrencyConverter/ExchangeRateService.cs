// ExchangeRateService.cs
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey;

        public ExchangeRateService(string apiKey)
        {
            _apiKey = apiKey;
        }

        // 指定された基本通貨の最新レートを取得する非同期メソッド
        public async Task<ApiResponse> GetLatestRatesAsync(string baseCurrency)
        {
            // APIのエンドポイントURLを構築
            string apiUrl = $"https://v6.exchangerate-api.com/v6/{_apiKey}/latest/{baseCurrency}";

            try
            {
                // APIにGETリクエストを送信し、レスポンスを受け取る
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                // レスポンスが成功でなければ例外を返す
                response.EnsureSuccessStatusCode();

                // レスポンスボディを文字列として読み取る
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // JSON文字列をApiResponseオブジェクトに変換する
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

                if (apiResponse == null || apiResponse.Result != "success")
                {
                    throw new Exception("APIから有効なデータが取得できませんでした。");
                }

                return apiResponse;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"\nAPIリクエストエラー: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n予期せぬエラー: {e.Message}");
                return null;
            }
        }
    }
}