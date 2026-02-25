using System;
using System.Collections.Generic;
using System.Linq;
using System.DateTime;
using System.Text;

namespace Lab1
{
  class Program
  {
    static class Flight
    {
      public int Id { get; set; }
      public int AirlineId { get; set; }
      public int AirplaneId { get; set; }
      public DateTime DepartureTime { get; set; }
      public DateTime TakeOffTime { get; set; }
      public int DestinationAirportId { get; set; }
      public int DepartureAirportId { get; set; }
    }

    static class Airplane
    {
      public int Id { get; set; }
      public int PassengerSeats { get; set; }
    }

    static class Airline
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }

    static class Airport
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }
    enum TicketClass
    {
      Economy,
      Business,
      FirstClass
    }
    static class Ticket
    {
      public int Id { get; set; }
      public int FlightId { get; set; }
      public int PassengerId { get; set; }
      public TicketClass ClassType { get; set; }
    }

    static class Passenger
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }
    
    static void Main (string[] args)
    {
      
    }
  }
  
}