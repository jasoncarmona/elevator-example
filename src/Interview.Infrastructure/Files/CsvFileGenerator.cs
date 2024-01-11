﻿using System;
using System.Globalization;
using CsvHelper;
using Interview.Application.Common.Interfaces;
using Interview.Infrastructure.Files.Maps;
using Interview.Application.Logs;


namespace Interview.Infrastructure.Files {
   public class CsvFileGenerator : ICsvFileGenerator {

      public string PathToFile { get; set; } = String.Empty;

      public void BuildElevatorLogFile(IEnumerable<ElevatorLogRecord> records) {
         //using var writter = new StreamWriter(this.PathToFile);
         //using var memoryStream = new MemoryStream();
         using (var streamWriter = new StreamWriter(this.PathToFile)) { // memoryStream
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Context.RegisterClassMap<ElevatorLogMap>();
            csvWriter.WriteRecords(records);
         }
         //writter.Write(memoryStream);
         //return memoryStream.ToArray();
      }
   }
}