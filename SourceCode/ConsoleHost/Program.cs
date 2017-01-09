using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TractorServer;

namespace ConsoleHost
{
  class Program
  {
    static void Main(string[] args)
    {
      using (ServiceHost host = new ServiceHost(typeof (TractorHost)))
      {
        host.Opened += delegate
          {
            Console.WriteLine("Tractor Server has started, type quit to stop");
          };
        host.Open();
        while (true)
        {
            string command = Console.ReadLine();
            if (command == "quit")
                break;
        }
      }
    }
  }
}
