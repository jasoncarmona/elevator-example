using System;
using Interview.Application.Logs;

namespace Interview.Application.Common.Interfaces {
   public interface ICsvFileGenerator {
      public string PathToFile { get; set; }
      public void BuildElevatorLogFile(IEnumerable<ElevatorLogRecord> records);
   }
}
