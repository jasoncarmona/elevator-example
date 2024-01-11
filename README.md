# Description

- Elevator implementation using .Net Core 7, CsvHelper, Autofac
- Even though a simple console app could show the same functionality, there's a focus on design practices and project structure
- Input csv file has the format Id,CallingFloor,DestinyFloor,CallingTime
- Output csv file has the format CurrentTime,FloorNumber,NextFloors,People
- Elevator capacity: 8, number of elevators: 1, no directional call buttons [only call request]
- No specific scheduling or operative strategies implemented, using always the only elevator and moving it when there's a request

# Dev Notes

- I'm using a Clean Architecture layout and organization for the solution
- I'm using a library to manage CSV generation and reading of content
- For DI autufac was selected
- Libraries like serilog and mediatR were left out to keep the implementation simpler
- Instead of planning ahead of time every step/destiny as it's usual with elevator applications, I'm using an heuristic instead
  where the elevator is moving one floor at a time and stops in case there's a call to attend or that floor is a selected destiny
  It continues until it has to change direction or it has to stop
- Time management was simplified to use a ticker/count approach, so the system is not managing a datetime for this field

# Project structure
![project structure](https://i0.wp.com/jasontaylor.dev/wp-content/uploads/2020/01/Figure-01-2.png?resize=531%2C531&ssl=1)
- There are two main up level folders organizing the code `src` and `tests`
- src
  - Interview.Domain: domain level entities, enums, etc.
  - Interview.Application: contains common interfaces, business logic, etc.
  - Interview.Infrastructure: provides functionality to access files, eventually repositories, etc
  - Interview.ConsoleInterface: main interaction project, it sets up dependencies 
- tests
  - Interview.Domain.Tests: unit tests, currently empty, pending to add solution for integration tests also.
  - Interview.Infrastructure.Tests: unit tests, currently empty, pending to add solution for integration tests also.

# Workflow / Reasoning

- There's a list of rawRequests coming from a file
- There's an infinite loop that will execute a long there are rawRequests or processed requests by the elevator controller
- The infinite loop will sort rawRequests and will send them to the elevator controller if it's their time to be processed
- The elevator controller has to main lists callRequests and destinyRequests
- A callRequest is when a user asks for the elevator and waits in a floor
- A destinyRequest is when the user is inside the elevator and asks to go to a floor
- A callRequest is converted into a destinyRequest when the elevator reaches the floor and there's capacity
- A destinyRequest is fulfilled when the elevator reaches a floor, the capacity is augmented after this
- The elevator will move floor to floor and will continue in the same direction as long as there are call or destiny requests in that direction
- The elevator will change directions when there are no more requests to process in the current direction
- The elevator will enter the idle status if there are no more requests
- Each request has a waitingTime and travelTime counters to measure theh time the user had to wait to get into the elevator and then to reach the desired floor

# Assumptions

- Even though there's no need for DI according to the description, I decided to use a basic implementation of it
- The scope of the test didn't specify the need of creating tests, nor the library to use it, I created the structure in the solution to add them
- When working with an elevator, there are different strategies, some to minimize energy consumption, others to minimize response time or even prioritize some floors at specific time frames, no specific strategy was requested, I went with the minimization of the response time
- Implementation of specific patters could be considered like an overengineering of the solution, but since the idea is to show a variety of practices, some patterns were implemented as part of the solution
- Elevator controller is using only one elevator, not an array of them, multiple elevators are out of the scope of this case

# Improvements

- Add tests
- Manage time as a datetime instead of a ticker or seconds value
- Allow the use of multiple elevators
- Control the time at the controller level and not by the elevator when checking if there are more requesets to process
- Because of the heuristic controller implementation, there's not a known list of next floors, because of this, when logging the list of comming floors, it's constructed manually, this can be changed

# How to run

1. Install dependencies
  This is not needed, dotnet build command will restore packages automatically

2. Build the project
  dotnet build --configuration Release

3. Run the project
  dotnet src/Interview.ConsoleInterface/bin/Release/net7.0/Interview.ConsoleInterface.dll -i input.csv -o output.csv
