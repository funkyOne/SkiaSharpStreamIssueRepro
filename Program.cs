using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using SkiaSharp;
namespace skia
{
    class Program
    {
        static void Main(string[] args)
        {
            var brokenUrl = "https://image.ibb.co/cUjP7v/verona_solid.jpg";
            var workingUrl = "https://image.ibb.co/cS8dZa/flintsolid.jpg";

            Test(workingUrl, "working");
            Test(brokenUrl, "broken");
        }

        private static void Test(string url, string name)
        {            
            Console.WriteLine($"Testing '{name}'");
            Console.WriteLine($"Url - {url}");

            // this is to make sure the file is available for webclient and is correctly downloaded
            using (var client = new WebClient())
            {
                var filename = $"./{name}.jpg";
                using (var stream = client.OpenRead(url))
                using (var fileStream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    stream.CopyTo(fileStream);
                }

                Console.WriteLine($"Downloaded to {filename}");
            }

            using (var client = new WebClient())
            {
                Console.Write("Bitmap from bytes[] is .. ");
                using (var ms = new MemoryStream())
                using (var stream = client.OpenRead(url))
                {
                    stream.CopyTo(ms);
                    var bytes = ms.ToArray();
                    var bitmap = SKBitmap.Decode(bytes);

                    if (bitmap == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("null");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("not null");
                    }
                }
            }

            using (var client = new WebClient())
            {
                Console.Write("Bitmap from memory stream proxy is .. ");
                using (var ms = new MemoryStream())
                using (var stream = client.OpenRead(url))
                {
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    var bitmap = SKBitmap.Decode(ms);

                    if (bitmap == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("null");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("not null");
                    }
                }
            }

            using (var client = new WebClient())
            {
                Console.Write("Bitmap from direct stream is .. ");
                using (var stream = client.OpenRead(url))
                {
                    var bitmap = SKBitmap.Decode(stream);
                    if (bitmap == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("null");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("not null");
                    }
                }
            }

            Console.WriteLine();
        }
    }
}
