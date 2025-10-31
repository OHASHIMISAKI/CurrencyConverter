// Program.cs
using System;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            const string API_KEY = "adcf9b84d1f340d48d919457";

            var exchangeRateService = new ExchangeRateService(API_KEY);

            Console.WriteLine("通貨換算アプリ");
            Console.WriteLine("========================");

            while (true)
            {
                Console.WriteLine("\n換算モードを選択してください:");
                Console.WriteLine("1: 円 → ウォン");
                Console.WriteLine("2: 円 → シンガポールドル");
                Console.WriteLine("3: ウォン → 円");
                Console.WriteLine("4: シンガポールドル → 円");
                Console.WriteLine("0: 終了");
                Console.Write("モード選択: ");

                string choice = Console.ReadLine();
                string baseCurrency = "";
                string targetCurrency = "";

                switch (choice)
                {
                    case "1":
                        baseCurrency = "JPY";
                        targetCurrency = "KRW";
                        break;
                    case "2":
                        baseCurrency = "JPY";
                        targetCurrency = "SGD";
                        break;
                    case "3":
                        baseCurrency = "KRW";
                        targetCurrency = "JPY";
                        break;
                    case "4":
                        baseCurrency = "SGD";
                        targetCurrency = "JPY";
                        break;
                    case "0":
                        Console.WriteLine("アプリケーションを終了します。");
                        return; // プログラムを終了
                    default:
                        Console.WriteLine("無効な選択です。もう一度入力してください。");
                        continue; // ループの先頭に戻る
                }

                // 金額の入力
                Console.Write($"\n換算する金額（{baseCurrency}）を入力してください: ");
                string amountStr = Console.ReadLine();

                if (!decimal.TryParse(amountStr, out decimal amount))
                {
                    Console.WriteLine("無効な金額です。数値を入力してください。");
                    continue;
                }

                Console.WriteLine("レート情報を取得中...");

                // APIを呼び出してレート情報を取得
                var ratesResponse = await exchangeRateService.GetLatestRatesAsync(baseCurrency);

                if (ratesResponse != null && ratesResponse.ConversionRates.ContainsKey(targetCurrency))
                {
                    // 換算レートを取得
                    decimal rate = ratesResponse.ConversionRates[targetCurrency];

                    // 金額を計算
                    decimal convertedAmount = amount * rate;

                    // 結果を表示
                    Console.WriteLine("--- 換算結果 ---");
                    Console.WriteLine($"{amount:N0} {baseCurrency} = {convertedAmount:N2} {targetCurrency}");
                }
                else
                {
                    Console.WriteLine("レートの取得または換算に失敗しました。");
                }
            }
        }
    }
}