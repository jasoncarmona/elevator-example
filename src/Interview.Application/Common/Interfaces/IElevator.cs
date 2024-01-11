using System;
using Interview.Application.Elevator;
using Interview.Application.Logs;

namespace Interview.Application.Common.Interfaces {
   public interface IElevator {
      public Dictionary<int, List<ElevatorRequest>> CallFloorRequests { get; set; }
      public Dictionary<int, List<ElevatorRequest>> DestinyFloorRequests { get; set; }
      public int CurrentTime { get; set; }
      public List<ElevatorLogRecord> Logs { get; set; }

      public void ProcessCall(ElevatorRequest elevatorRequest);

      //public void ProcessDestiny(int floorNumber);

      public void ProcessTick();

      public bool AreTherePendingRequests();
   }
}
