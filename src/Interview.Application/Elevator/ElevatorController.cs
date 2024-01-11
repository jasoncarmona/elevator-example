using System;
using System.Threading;
using Interview.Application.Common.Interfaces;

namespace Interview.Application.Elevator {
   public class ElevatorController : IElevatorController {

      public static readonly int DEFAULT_ELEVATORS_COUNT = 1;

      public Queue<ElevatorRequest> RawElevatorRequests { get; set; } = new();

      public List<IElevator> Elevators { get; set; } = new();

      public IOperationStrategy OperationStrategy { get; set; }

      public int CurrentTime { get; set; }

      public ICsvFileGenerator CsvFileGenerator { get; set; }

      public ILogger Logger { get; set; }

      public ElevatorController() {
         this.OperationStrategy = new SpeedStrategy();
         this.Elevators.Add(new Elevator());
         this.CurrentTime = 0;
         
      }

      public void ProcessRequests() {
         using (new Timer(TryToProcessRequest, null, 0, 1000)) {
            while (this.RawElevatorRequests.Count > 0 || this.AreTherePendingRequests()) {
               Thread.Sleep(TimeSpan.FromSeconds(1));
               this.Elevators.First().ProcessTick();
            }
         }
         this.CsvFileGenerator.BuildElevatorLogFile(this.Elevators.First().Logs);
         Console.WriteLine("No more pending requests, finishing thread");
      }

      private void TryToProcessRequest(object? state) {
         this.CurrentTime++;

         // this is because there's only one elevator, if not, this would be set by the operation strategy
         var elevatorTime = this.Elevators.First().CurrentTime;

         Console.WriteLine($"Current time {elevatorTime} : {DateTime.Now}");

         ElevatorRequest elevatorRequest;
         while (this.RawElevatorRequests.TryPeek(out elevatorRequest!) && elevatorRequest.CallingTime <= elevatorTime) {
            var nextElevatorRequest = this.RawElevatorRequests.Dequeue();
            var elevator = this.OperationStrategy.getElevator(this.Elevators, elevatorRequest);
            elevator.ProcessCall(nextElevatorRequest);

            Console.WriteLine($"Sending request to controller {nextElevatorRequest.ToString()}");
         }
      }

      public bool AreTherePendingRequests() {
         return this.Elevators.First().AreTherePendingRequests();
      }


   }
}

