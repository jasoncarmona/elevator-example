using System;
using Interview.Application.Elevator;

namespace Interview.Application.Common.Interfaces {

   public interface IOperationStrategy {

      public IElevator getElevator(List<IElevator> elevators, ElevatorRequest request);
   }
}

