using System;
using CsvHelper;
using System.Globalization;
using Interview.Application.Common.Interfaces;
using Interview.Application.Elevator;

namespace Interview.Infrastructure.Files {

   public class CsvFileReader : ICsvFileReader {

      public IEnumerable<ElevatorRequest>? ReadElevatorRequestsFile(string pathToFile) {
         using var reader = new StreamReader(pathToFile);
         using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

         return (IEnumerable<ElevatorRequest>?)csv
            .GetRecords<ElevatorRequest>()
            .ToList()
            .OrderBy(request => request.CallingTime);
      }

      //public DataTable ReadElevatorRequestsFile(string pathToFile) {
      //   //pathToFile = "path\\to\\file.csv";
      //   using var reader = new StreamReader(pathToFile);
      //   using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
      //   using (var dr = new CsvDataReader(csv))
      //   {
      //      var dt = new DataTable();
      //      dt.Columns.Add("Id", typeof(string));
      //      dt.Columns.Add("CallingFloor", typeof(int));
      //      dt.Columns.Add("DestinyFloor", typeof(int));
      //      dt.Columns.Add("CallingTime", typeof(DateTime));

      //      dt.Load(dr);

      //      return dt;
      //   }
      //}
   }
}

