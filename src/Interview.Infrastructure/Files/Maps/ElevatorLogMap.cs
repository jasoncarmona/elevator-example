using System;
using CsvHelper.Configuration;
using System.Globalization;
using Interview.Application.Logs;

namespace Interview.Infrastructure.Files.Maps {
   public class ElevatorLogMap : ClassMap<ElevatorLogRecord> {
      public ElevatorLogMap() {
         AutoMap(CultureInfo.InvariantCulture);
         //Map(m => m.NextFloors).Convert(c => c.Reduce());
      }
   }
}