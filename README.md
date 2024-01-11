# Instructions:

1. Install dependencies
  This is not needed, dotnet build command will restore packages automatically

2. Build the project
  dotnet build --configuration Release

3. Run the project

  dotnet src/Interview.ConsoleInterface/bin/Release/net7.0/Interview.ConsoleInterface.dll -i input.csv -o output.csv

# Description
- I'm using a Clean Architecture layout and organization for the solution
- I'm using a library to manage CSV generation and reading of content
- I'm using an heuristic approach to the way the elevator operates, instead of planning ahead of time every step,
  the elevator is moving one floor at a time and stops in case there's a call to attend or that floor is a selected destiny
  It continues until it has to change direction or it has to stop

# Assumptions

- Even though there's no need for DI according to the description, I decided to use a basic implementation of it
- The scope of the test didn't specify the need of creating tests, nor the library to use it, I created the structure in the solution to add them
- When working with an elevator, there are different strategies, some to minimize energy consumption, others to minimize response time or even prioritize some floors at specific time frames, no specific strategy was requested, I went with the minimization of the response time
- Implementation of specific patters could be considered like an overengineering of the solution, but since the idea is to show a variety of practices, some patterns were implemented as part of the solution
- Elevator controller is using only one elevator, not an array of them, multiple elevators are out of the scope of this case

# Improvements
- Add tests
- Allow the use of multiple elevators
- Control the time at the controller level and not by the elevator when checking if there are more requesets to process
