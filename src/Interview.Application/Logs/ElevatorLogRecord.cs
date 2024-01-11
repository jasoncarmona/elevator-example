using System;
namespace Interview.Application.Logs {
   public class ElevatorLogRecord {

      //public int Id { get; set; }
      public int CurrentTime { get; set; }
      public int FloorNumber { get; set; }
      public string NextFloors { get; set; } = String.Empty;
      public string People { get; set; } = String.Empty;

      public ElevatorLogRecord() {
      }

      public override string ToString() {
         return $"CurrentTime: {CurrentTime}, CurrentFloor: {FloorNumber}, People: {People}, NextFloors: {NextFloors}";
      }
   }
}
