using System;
namespace Interview.Application.Common.Interfaces {
   public interface ILogger {
      Task Log(string value);
   }
}
