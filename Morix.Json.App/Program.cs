using System;

namespace Morix.Json.App
{
    internal class Program
    {
        static void Main()
        {
            //string data = "\"hello\nthere\"";
            //try
            //{
            //    var morix = JsonConvert.Deserialize<string>(data);
            //    if (morix != null)
            //    {

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}

            //try
            //{
            //    var parsed = Newtonsoft.Json.JsonConvert.
            //        DeserializeObject<string>(data);
            //}
            //catch (Exception ex)
            //{

            //}

            //try
            //{
            //    var parsed = System.Text.Json.JsonDocument.Parse(data);

            //}
            //catch (Exception ex)
            //{

            //}

            //BenchmarkRunner.Run<Competitors>();

            MyTests(1);
            MyTests(100);
            MyTests(1000);
            MyTests(10000);
            MyTests(1000);
            MyTests(100);
            Console.ReadKey();
        }

        private static void MyTests(int max)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Runnig iterations = " + max.ToString("#,##0"));
            var s = new Competitors();
            var sw = System.Diagnostics.Stopwatch.StartNew();
            int i = max;
            while (i > 0)
            {
                s.Morix_Serialize();
                i--;
            }
            sw.Stop();
            Console.WriteLine("Morix_Serialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");

            sw.Restart();
            i = max;
            while (i > 0)
            {
                s.Newton_Serialize();
                i--;
            }
            sw.Stop();
            Console.WriteLine("Newton_Serialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");

            sw.Restart();
            i = max;
            while (i > 0)
            {
                s.Microsoft_Serialize();
                i--;
            }
            sw.Stop();
            Console.WriteLine("Microsoft_Serialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");
            sw.Restart();
            i = max;
            while (i > 0)
            {
                s.Morix_Deserialize();
                i--;
            }
            sw.Stop();
            Console.WriteLine("Morix_Deserialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");
            sw.Restart();
            i = max;
            while (i > 0)
            {
                s.Newton_Deserialize();
                i--;
            }
            sw.Stop();
            Console.WriteLine("Newton_Deserialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");
            sw.Restart();
            i = max;
            while (i > 0)
            {
                s.Microsoft_Deserialize();
                i--;
            }
            sw.Stop();
            Console.WriteLine("Microsoft_Deserialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");
        }
    }
}