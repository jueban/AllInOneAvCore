using System;
using DAL;
using Models;
using Services;
using Utils;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = JavLibraryService.DownloadCategory().Result;

            Console.WriteLine("Hello World!");
        }
    }
}