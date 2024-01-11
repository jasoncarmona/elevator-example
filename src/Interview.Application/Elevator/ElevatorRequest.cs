using System;
namespace Interview.Application.Elevator {
   public class ElevatorRequest {
      public ElevatorRequest() { }

      public string Id { get; set; } = String.Empty;
      public int CallingFloor { get; set; } = 0;
      public int DestinyFloor { get; set; } = 0;
      public int CallingTime { get; set; } = 0;

      private int WaitingTime { get; set; } = 0;
      private int TravelTime { get; set; } = 0;

      private bool CallProcessed { get; set; } = false;
      private bool DestinyProcessed { get; set; } = false;

      public override string ToString() {
         return $"Id: {this.Id}, callingFloor: {this.CallingFloor}, destinyFloor: {this.DestinyFloor}, callingTime: {this.CallingTime}, waitingTime: {this.WaitingTime}, travelTime: {this.TravelTime}";
      }

      public void IncreaseTime(int elapsedTime) {
         if (this.CallProcessed) this.TravelTime += elapsedTime;
         else this.WaitingTime += elapsedTime;
      }
      public void SetCallProcessed(bool value) {
         this.CallProcessed = value;
      }
      public void SetDestinyProcessed(bool value) {
         this.DestinyProcessed = value;
      }
   }
}

