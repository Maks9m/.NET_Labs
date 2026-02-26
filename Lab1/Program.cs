using System;
using System.Linq;
namespace Lab1
{
  class Program
  {
    class Flight
    {
      public int Id { get; set; }
      public int AirlineId { get; set; }
      public int AirplaneId { get; set; }
      public DateTime DepartureTime { get; set; }
      public DateTime TakeOffTime { get; set; }
      public int RouteId { get; set; }

      public Flight(int id, int airlineId, int airplaneId, DateTime departureTime, DateTime takeOffTime, int routeId)
      {
        Id = id;
        AirlineId = airlineId;
        AirplaneId = airplaneId;
        DepartureTime = departureTime;
        TakeOffTime = takeOffTime;
        RouteId = routeId;
      }
    }

    class Airplane
    {
      public int Id { get; set; }
      public int CarryingCapacity { get; set; }
      public Airplane(int id, int carryingCapacity)
      {
        Id = id;
        CarryingCapacity = carryingCapacity;
      }
    }

    class Airline
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public Airline(int id, string name)
      {
        Id = id;
        Name = name;
      }
    }

    class Route
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public Route(int id, string name)
      {
        Id = id;
        Name = name;
      }
    }
    enum TicketClass
    {
      Economy,
      Business,
      FirstClass
    }
    class Ticket
    {
      public int Id { get; set; }
      public int FlightId { get; set; }
      public int PassengerId { get; set; }
      public TicketClass ClassType { get; set; }
      public Ticket(int id, int flightId, int passengerId, TicketClass classType)
      {
        Id = id;
        FlightId = flightId;
        PassengerId = passengerId;
        ClassType = classType;
      }
    }

    class Passenger
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public Passenger(int id, string name)
      {
        Id = id;
        Name = name;
      }
    }
    static List<Flight> flights = new List<Flight>();
    static List<Airplane> airplanes = new List<Airplane>();
    static List<Airline> airlines = new List<Airline>();
    static List<Route> airports = new List<Route>();
    static List<Ticket> tickets = new List<Ticket>();
    static List<Passenger> passengers = new List<Passenger>();
    
    static void Main (string[] args)
    {
      List<Flight> AddFlights()
      {
        flights.Add(new Flight(1, 1, 1, DateTime.Parse("2024-01-01 08:00"), DateTime.Parse("2024-01-01 10:00"), 2));
        flights.Add(new Flight(2, 2, 2, DateTime.Parse("2024-01-02 09:00"), DateTime.Parse("2024-01-02 11:00"), 3));
        flights.Add(new Flight(3, 1, 3, DateTime.Parse("2024-01-03 10:00"), DateTime.Parse("2024-01-03 12:00"), 1));
        return flights;
      }
      AddFlights();

      List<Airplane> AddAirplanes()
      {
        airplanes.Add(new Airplane(1, 150));
        airplanes.Add(new Airplane(2, 200));
        airplanes.Add(new Airplane(3, 250));
        return airplanes;
      }
      AddAirplanes();

      List<Airline> AddAirlines()
      {
        airlines.Add(new Airline(1, "Airline A"));
        airlines.Add(new Airline(2, "Airline B"));
        return airlines;
      }
      AddAirlines();

      List<Route> AddAirports()
      {
        airports.Add(new Route(1, "Airport X"));
        airports.Add(new Route(2, "Airport Y"));
        airports.Add(new Route(3, "Airport Z"));
        return airports;
      }
      AddAirports();

      List<Ticket> AddTickets()
      {
        tickets.Add(new Ticket(1, 1, 1, TicketClass.Economy));
        tickets.Add(new Ticket(2, 1, 2, TicketClass.Business));
        tickets.Add(new Ticket(3, 2, 3, TicketClass.FirstClass));
        return tickets;
      }
      AddTickets();

      List<Passenger> AddPassengers()
      {
        passengers.Add(new Passenger(1, "John Doe"));
        passengers.Add(new Passenger(2, "Jane Smith"));
        passengers.Add(new Passenger(3, "Alice Johnson"));
        return passengers;
      }
      AddPassengers();

      // Знайти маршрути, де середня затримка рейсів перевищує 30 хвилин за останні 6 місяців.
      var query1 = from flight in flights
                   where flight.DepartureTime >= DateTime.Now.AddMonths(-6)
                   group flight by flight.RouteId into routeGroup
                   where routeGroup.Average((f) => (f.TakeOffTime - f.DepartureTime).TotalMinutes) > 30
                   select routeGroup;
      Console.WriteLine("Маршрути з середньою затримкою понад 30 хвилин за останні 6 місяців:\n");
      foreach (var route in query1)
        Console.WriteLine($"Маршрут: {route.Key}, \n Середня затримка: {route.Average((f) => (f.TakeOffTime - f.DepartureTime).TotalMinutes)} хвилин");

      // Визначити пасажирів, які літали з більш ніж 5 авіакомпаніями протягом року.
      var query2 = from ticket in tickets
                   join flight in flights on ticket.FlightId equals flight.Id
                   where flight.DepartureTime >= DateTime.Now.AddYears(-1)
                   group flight by ticket.PassengerId into passengerFlights
                   where passengerFlights.Select((f) => f.AirlineId).Distinct().Count() > 5
                   select passengerFlights.Key;
      Console.WriteLine("Пасажири, які літали з більш ніж 5 авіакомпаніями протягом року:\n");
      foreach (var passenger in query2)
        Console.WriteLine($"Пасажир ID: {passenger}\n");

      // Визначити маршрути, на яких середня кількість пасажирів у бізнес- класі перевищує 60% місткості літака.
      var query3 = from flight in flights
                   join airplane in airplanes on flight.AirplaneId equals airplane.Id
                   let businessPassengers = tickets.Count((t) => t.FlightId == flight.Id && t.ClassType == TicketClass.Business)
                   let businessPassengerPercentage = (double)businessPassengers / airplane.CarryingCapacity
                   group new { flight, businessPassengerPercentage } by flight.RouteId into routeGroup
                   where routeGroup.Average((f) => f.businessPassengerPercentage) > 0.6
                   select routeGroup.Key;
      Console.WriteLine("Маршрути, на яких середня кількість пасажирів у бізнес-класі перевищує 60% місткості літака:\n");
      foreach (var route in query3)
        Console.WriteLine($"Маршрут ID: {route}\n");

      // Знайти авіакомпанії, у яких відсоток рейсів із запізненням понад 30 хвилин зріс більш ніж на 20% за останній рік.
      var query4 = from airline in airlines
                    join flight in flights on airline.Id equals flight.AirlineId into airlineFlights
                    let flightsThisYear = airlineFlights.Where((f) => f.DepartureTime >= DateTime.Now.AddYears(-1)).ToList()
                    let delayedFlightsThisYear = flightsThisYear.Count((f) => (f.TakeOffTime - f.DepartureTime).TotalMinutes > 30)
                    let flightsPrevYears = airlineFlights.Where((f) => f.DepartureTime <= DateTime.Now.AddYears(-1)).ToList()
                    let delayedFlightsPrevYears = flightsPrevYears.Count((f) => (f.TakeOffTime - f.DepartureTime).TotalMinutes > 30)
                    where flightsPrevYears.Count > 0 && flightsThisYear.Count > 0
                    let delayPercentagePrevYears = (double)delayedFlightsPrevYears / flightsPrevYears.Count * 100
                    let delayPercentageThisYear = (double)delayedFlightsThisYear / flightsThisYear.Count * 100
                    where delayPercentageThisYear - delayPercentagePrevYears > 20
                    select airline;
      Console.WriteLine("Авіакомпанії, у яких відсоток рейсів із запізненням понад 30 хвилин зріс більш ніж на 20% за останній рік:\n");
      foreach (var airline in query4)
        Console.WriteLine($"Авіакомпанія: {airline.Name}\n");

      // Топ-5 рейсів з найдовшою затримкою
      var query5 = (from flight in flights
                   let delay = (flight.TakeOffTime - flight.DepartureTime).TotalMinutes
                   orderby delay descending
                   select new { FlightId = flight.Id, Delay = delay }).Take(5);
      Console.WriteLine("Топ-5 рейсів з найдовшою затримкою:\n");
      foreach (var flight in query5)
        Console.WriteLine($"Рейс ID: {flight.FlightId}, Затримка: {flight.Delay} хвилин\n");

      // Перевірка вчасності вилітів
      var query6 = flights.All((f) => (f.TakeOffTime - f.DepartureTime).TotalMinutes <= 30);
      Console.WriteLine($"Чи всі рейси вилетіли вчасно (затримка не більше 30 хвилин)? {query6}");

      // Алфавітний список пасажирів на літеру "О"
      var query7 = from passenger in passengers
                    where passenger.Name.StartsWith('O')
                    orderby passenger.Name
                    select passenger;
      Console.WriteLine("Пасажири, чиї імена починаються на 'О':\n");
      foreach (var passenger in query7)
        Console.WriteLine($"Пасажир: {passenger.Name}\n");

      // Створення швидкого довідника літаків
      var query8 = airplanes.ToDictionary((a) => a.Id, (a) => a.CarryingCapacity);
      Console.WriteLine("Довідник літаків (ID -> Місткість):\n");
      foreach (var airplane in query8)
        Console.WriteLine($"Літак ID: {airplane.Key}, Місткість: {airplane.Value}\n");

      // Статистика проданих квитків за класами
      var query9 = from ticket in tickets
                    group ticket by ticket.ClassType into classGroup
                    orderby classGroup.Count() descending
                    select new { ClassType = classGroup.Key, Count = classGroup.Count() };
      Console.WriteLine("Статистика проданих квитків за класами:\n");
      foreach (var item in query9)
        Console.WriteLine($"Клас: {item.ClassType}, Кількість: {item.Count}\n");

      // Об'єднання рейсів двох авіакомпаній (дії над множинами)
      var query10 = (from flight in flights
                    where flight.AirlineId == 1
                    select flight).Union(from flight in flights
                                         where flight.AirlineId == 2
                                         select flight)
                                         .OrderByDescending((f) => f.DepartureTime);
      Console.WriteLine("Рейси авіакомпаній 1 та 2:\n");
      foreach (var flight in query10)
        Console.WriteLine($"Рейс ID: {flight.Id}, Авіакомпанія ID: {flight.AirlineId}\n");
    }
  }
}