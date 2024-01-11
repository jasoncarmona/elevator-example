using System;
using CommandLine;

namespace Interview.ConsoleInterface {
   public class CommandLineOptions {
      public CommandLineOptions() {
         this.InputFilePath = "input.csv";
         this.OutputFilePath = "output.csv";
      }

      [Option('v', "verbose", Required = false, HelpText = "enable verbose output")]
      public bool Verbose { get; set; }

      [Option('i', "input", Required = true, HelpText = "input .csv file path", Default = "input.csv")]
      public string InputFilePath { get; set; }

      [Option('o', "output", Required = false, HelpText = "output .csv file path", Default = "output.csv")]
      public string OutputFilePath { get; set; }
   }
}

