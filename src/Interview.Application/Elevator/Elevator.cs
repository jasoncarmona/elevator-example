using System;
using Interview.Domain.Entities;
using Interview.Application.Common.Interfaces;
using Interview.Application.Logs;
using Interview.Domain.Enums;

namespace Interview.Application.Elevator {
   public class Elevator : IElevator {
      public static readonly int DEFAULT_CAPACITY = 8;
      public static readonly int FLOOR_TRIP_SPEED = 10;

      public static readonly int MIN_FLOOR = 0;
      public static readonly int MAX_FLOOR = 10;

      public Dictionary<int, List<ElevatorRequest>> CallFloorRequests { get; set; }
      public Dictionary<int, List<ElevatorRequest>> DestinyFloorRequests { get; set; }

      public ILogger Logger { get; set; }
      public List<ElevatorLogRecord> Logs { get; set; }

      public ElevatorDirection Direction { get; set; }
      public ElevatorStatus Status { get; set; }
      public Floor CurrentFloor { get; set; }
      public int CurrentTime { get; set; }
      public int Capacity { get; set; }

      public Elevator(ILogger logger) {
         this.CurrentFloor = new Floor() { Number = MIN_FLOOR };
         this.Capacity = Elevator.DEFAULT_CAPACITY;
         this.Direction = ElevatorDirection.Up;
         this.Status = ElevatorStatus.Idle;
         this.Logger = logger;

         this.CallFloorRequests = new Dictionary<int, List<ElevatorRequest>>(MAX_FLOOR + 1);
         this.DestinyFloorRequests = new Dictionary<int, List<ElevatorRequest>>(MAX_FLOOR + 1);

         for (int i = 0; i < MAX_FLOOR + 1; i++) {
            this.CallFloorRequests.Add(i, new List<ElevatorRequest>());
            this.DestinyFloorRequests.Add(i, new List<ElevatorRequest>());
         }

         this.Logs = new List<ElevatorLogRecord>();
      }

      public void ProcessCall(ElevatorRequest elevatorRequest) {
         this.CallFloorRequests[elevatorRequest.CallingFloor].Add(elevatorRequest);
      }

      public void ProcessTick() {
         if (this.Status == ElevatorStatus.Moving) {
            this.IncreaseWaitingTime(FLOOR_TRIP_SPEED);
            this.CurrentTime +=  FLOOR_TRIP_SPEED;
            this.CurrentFloor.Number += this.Direction == ElevatorDirection.Up ? 1 : -1;

            while (this.CallFloorRequests[this.CurrentFloor.Number].Count > 0 && this.Capacity >= 0) {
               this.Capacity--;
               var elevatorRequest = this.CallFloorRequests[this.CurrentFloor.Number][0];
               elevatorRequest.SetCallProcessed(true);
               this.DestinyFloorRequests[elevatorRequest.DestinyFloor].Add(elevatorRequest);
               this.CallFloorRequests[this.CurrentFloor.Number].Remove(elevatorRequest);
            }

            this.DestinyFloorRequests[this.CurrentFloor.Number].ForEach(elevatorRequest => {
               this.Capacity++;
               elevatorRequest.SetDestinyProcessed(true);
               this.Logger.Log($"Elevator: Request processed -> {elevatorRequest.ToString()}");
            });
            this.DestinyFloorRequests[this.CurrentFloor.Number].Clear();

            if (this.GetNewDirection(out ElevatorDirection newDirection)) this.Direction = newDirection;
            else this.Status = ElevatorStatus.Idle;

            this.AddLogEntry();
         }
         else {
            this.CurrentTime++;
            this.IncreaseWaitingTime(1);
            if (this.GetNewDirection(out ElevatorDirection newDirection)) {
               this.Status = ElevatorStatus.Moving;
               this.Direction = newDirection;
            }
            else {
               this.CurrentTime++;
               this.Status = ElevatorStatus.Idle;
            }
         }
         
      }

      public bool AreTherePendingRequests() {
         return this.CallFloorRequests.Any(pairValue => pairValue.Value.Count > 0)
            || this.DestinyFloorRequests.Any(pairValue => pairValue.Value.Count > 0);
      }

      private void IncreaseWaitingTime(int time) {
         foreach (var (key, requests) in this.CallFloorRequests) requests.ForEach(request => request.IncreaseTime(time));

         foreach (var (key, requests) in this.DestinyFloorRequests) requests.ForEach(request => request.IncreaseTime(time));
      }

      private bool GetNewDirection(out ElevatorDirection newDirection) {
         bool upRequests = this.CallFloorRequests.Any(pair => pair.Key > this.CurrentFloor.Number && pair.Value.Count > 0)
            || this.DestinyFloorRequests.Any(pair => pair.Key > this.CurrentFloor.Number && pair.Value.Count > 0);
         bool downRequests = this.CallFloorRequests.Any(pair => pair.Key < this.CurrentFloor.Number && pair.Value.Count > 0)
            || this.DestinyFloorRequests.Any(pair => pair.Key < this.CurrentFloor.Number && pair.Value.Count > 0);

         if ((this.Direction == ElevatorDirection.Up && upRequests) || (this.Direction == ElevatorDirection.Down && !downRequests && upRequests)) {
            newDirection = ElevatorDirection.Up;
            return true;
         }
         if ((this.Direction == ElevatorDirection.Down && downRequests) || (this.Direction == ElevatorDirection.Up && !upRequests && downRequests)) {
            newDirection = ElevatorDirection.Down;
            return true;
         }
         newDirection = ElevatorDirection.Up;
         return false;
      }

      private void AddLogEntry() {
         HashSet<int> nextFloorsUp = new(), nextFloorsDown = new();
         HashSet<string> people = new();

         foreach (var (key, requests) in this.CallFloorRequests) {
            if (requests.Count > 0) {
               if (this.Direction == ElevatorDirection.Up && this.CurrentFloor.Number > key) nextFloorsDown.Add(key);
               else nextFloorsUp.Add(key);
            }
         }
         foreach (var (key, requests) in this.DestinyFloorRequests) {
            if (requests.Count > 0) {
               if (this.Direction == ElevatorDirection.Up && this.CurrentFloor.Number > key) nextFloorsDown.Add(key);
               else nextFloorsUp.Add(key);
               requests.ForEach(request => people.Add(request.Id));
            }
         }

         var logEntry = new ElevatorLogRecord() {
            CurrentTime = this.CurrentTime,
            FloorNumber = this.CurrentFloor.Number,
            NextFloors = String.Join("-", nextFloorsUp.ToList().Order().Concat(nextFloorsDown.ToList().OrderDescending()).ToList<int>()),
            People = String.Join("-", people.ToArray()),
         };
         this.Logs.Add(logEntry);
         this.Logger.Log($"Elevator: {logEntry.ToString()}");
      }
   }
}
