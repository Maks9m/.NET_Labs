using System;
using System.Linq;
namespace Lab1
{
  class Program
  {
    public enum TicketClass
    {
      Economy,
      Business
    }

    public class Passenger
    {
      public int Id { get; set; }
      public required string Name { get; set; }
    }

    public class Airline
    {
      public int Id { get; set; }
      public required string Name { get; set; }
    }

    public class Airplane
    {
      public int Id { get; set; }
      public int CarryingCapacity { get; set; }
    }

    public class Flight
    {
      public int Id { get; set; }
      public int AirlineId { get; set; }
      public int AirplaneId { get; set; }
      public int RouteId { get; set; }
      public DateTime DepartureTime { get; set; }
      public DateTime TakeOffTime { get; set; }
    }

    public class Ticket
    {
      public int PassengerId { get; set; }
      public int FlightId { get; set; }
      public TicketClass ClassType { get; set; }
    }

    // Статичні колекції з тестовими даними
    static readonly List<Passenger> passengers =
    [
      new Passenger { Id = 1, Name = "Oleksandr" }, // Літав багатьма компаніями
      new Passenger { Id = 2, Name = "Olena" },
      new Passenger { Id = 3, Name = "Ivan" }
    ];

    static readonly List<Airline> airlines =
    [
      new Airline { Id = 1, Name = "SkyUp Airlines" }, // Компанія з різким збільшенням затримок
      new Airline { Id = 2, Name = "Ukraine International Airlines" },
      new Airline { Id = 3, Name = "Ryanair" },
      new Airline { Id = 4, Name = "Wizz Air" },
      new Airline { Id = 5, Name = "Lufthansa" },
      new Airline { Id = 6, Name = "Turkish Airlines" }
    ];

    static readonly List<Airplane> airplanes =
    [
      new Airplane { Id = 1, CarryingCapacity = 10 },  // Маленький літак для перевірки бізнес-класу
      new Airplane { Id = 2, CarryingCapacity = 180 },
      new Airplane { Id = 3, CarryingCapacity = 200 }
    ];

    static readonly List<Flight> flights =
    [
      // Нещодавні рейси (останні 6 місяців), затримка 45 хвилин (для Запиту 1)
      new Flight { Id = 1, AirlineId = 1, AirplaneId = 1, RouteId = 1, DepartureTime = DateTime.Now.AddMonths(-2), TakeOffTime = DateTime.Now.AddMonths(-2).AddMinutes(45) },

      // Рейси для пасажира 1 (щоб було > 5 авіакомпаній за рік)
      new Flight { Id = 2, AirlineId = 2, AirplaneId = 2, RouteId = 2, DepartureTime = DateTime.Now.AddMonths(-3), TakeOffTime = DateTime.Now.AddMonths(-3).AddMinutes(10) },
      new Flight { Id = 3, AirlineId = 3, AirplaneId = 2, RouteId = 3, DepartureTime = DateTime.Now.AddMonths(-4), TakeOffTime = DateTime.Now.AddMonths(-4) },
      new Flight { Id = 4, AirlineId = 4, AirplaneId = 2, RouteId = 4, DepartureTime = DateTime.Now.AddMonths(-5), TakeOffTime = DateTime.Now.AddMonths(-5) },
      new Flight { Id = 5, AirlineId = 5, AirplaneId = 3, RouteId = 5, DepartureTime = DateTime.Now.AddMonths(-6), TakeOffTime = DateTime.Now.AddMonths(-6) },
      new Flight { Id = 6, AirlineId = 6, AirplaneId = 3, RouteId = 6, DepartureTime = DateTime.Now.AddMonths(-7), TakeOffTime = DateTime.Now.AddMonths(-7) },

      // Старі рейси (понад рік тому) для Запиту 4 (порівняння затримок)
      // Минулого року SkyUp (Airline 1) літав без затримок
      new Flight { Id = 7, AirlineId = 1, AirplaneId = 2, RouteId = 7, DepartureTime = DateTime.Now.AddYears(-2), TakeOffTime = DateTime.Now.AddYears(-2) }
    ];

    static readonly List<Ticket> tickets =
    [
      // Квитки для пасажира 1 (Oleksandr) на різні авіакомпанії
      new Ticket { PassengerId = 1, FlightId = 1, ClassType = TicketClass.Business },
      new Ticket { PassengerId = 1, FlightId = 2, ClassType = TicketClass.Economy },
      new Ticket { PassengerId = 1, FlightId = 3, ClassType = TicketClass.Economy },
      new Ticket { PassengerId = 1, FlightId = 4, ClassType = TicketClass.Economy },
      new Ticket { PassengerId = 1, FlightId = 5, ClassType = TicketClass.Economy },
      new Ticket { PassengerId = 1, FlightId = 6, ClassType = TicketClass.Economy },
  
      // Наповнюємо перший рейс (літак місткістю 10 місць) бізнес-класом для Запиту 3
      new Ticket { PassengerId = 2, FlightId = 1, ClassType = TicketClass.Business },
      new Ticket { PassengerId = 3, FlightId = 1, ClassType = TicketClass.Business },
      new Ticket { PassengerId = 1, FlightId = 1, ClassType = TicketClass.Business },
      new Ticket { PassengerId = 2, FlightId = 1, ClassType = TicketClass.Business },
      new Ticket { PassengerId = 3, FlightId = 1, ClassType = TicketClass.Business },
      new Ticket { PassengerId = 1, FlightId = 1, ClassType = TicketClass.Business },
      new Ticket { PassengerId = 2, FlightId = 1, ClassType = TicketClass.Business }
      // Разом 7 квитків бізнес-класу на 10 місць (70% заповненості)
    ];

    static void Main(string[] args)
    {
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