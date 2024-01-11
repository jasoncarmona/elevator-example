using System;
using Interview.Application.Elevator;

namespace Interview.Application.Common.Interfaces {
   public interface ICsvFileReader {
      public IEnumerable<ElevatorRequest>? ReadElevatorRequestsFile(string pathToFile);
   }
}

