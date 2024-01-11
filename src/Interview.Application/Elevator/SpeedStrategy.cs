using System;
using Interview.Application.Common.Interfaces;

namespace Interview.Application.Elevator {
   public class SpeedStrategy : IOperationStrategy {
      public SpeedStrategy() {
      }

      public IElevator getElevator(List<IElevator> elevators, ElevatorRequest request) {
         // because of the scope of this interview assesment, there's only one elvator
         // but if more are added, this selection strategy will check for the ones closer
         // to the floor calling for the service
         return elevators.First();
      }
   }
}

