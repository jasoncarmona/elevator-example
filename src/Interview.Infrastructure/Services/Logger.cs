using System;
using Interview.Application.Common.Interfaces;

namespace Interview.Infrastructure.Services {
   public class Logger : ILogger {
      public async Task Log(string value) {
         await Console.Out.WriteLineAsync($"{value}");
      }
   }
}

