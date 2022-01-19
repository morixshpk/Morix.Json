using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Morix.Json.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Competitors>();
        }
    }
}