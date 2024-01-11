using System;
using Interview.Application.Elevator;

namespace Interview.Application.Common.Interfaces {
   public interface IElevatorController {
      public Queue<ElevatorRequest> RawElevatorRequests { get; set; }
      
      public List<IElevator> Elevators { get; set; }

      public IOperationStrategy OperationStrategy { get; set; }

      public ICsvFileGenerator CsvFileGenerator { get; set; }

      public ILogger Logger { get; set; }

      public void ProcessRequests();
   }
}

