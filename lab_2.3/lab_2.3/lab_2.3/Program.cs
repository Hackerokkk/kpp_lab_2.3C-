using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Container
{
    private List<Route> routes;

    public Container(List<Route> routes)
    {
        this.routes = routes;
    }

    public List<Route> SortBySeats()
    {
        List<Route> sortedRoutes = new List<Route>(routes);
        sortedRoutes.Sort((r1, r2) => r2.GetAvailableSeats() - r1.GetAvailableSeats());
        return sortedRoutes;
    }

    public List<Route> SortByDayOfWeek()
    {
        List<Route> sortedRoutes = new List<Route>(routes);
        sortedRoutes.Sort((r1, r2) => string.Compare(r1.GetDayOfWeek(), r2.GetDayOfWeek()));
        return sortedRoutes;
    }

    public List<Route> SortByFlightNumber()
    {
        List<Route> sortedRoutes = new List<Route>(routes);
        sortedRoutes.Sort((r1, r2) => r1.GetFlightNumber() - r2.GetFlightNumber());
        return sortedRoutes;
    }
}

class Route
{
    private string stationName;
    private int availableSeats;
    private string dayOfWeek;
    private int flightNumber;

    public Route(string stationName, int availableSeats, string dayOfWeek, int flightNumber)
    {
        this.stationName = stationName;
        this.availableSeats = availableSeats;
        this.dayOfWeek = dayOfWeek;
        this.flightNumber = flightNumber;
    }

    public int GetAvailableSeats()
    {
        return availableSeats;
    }

    public string GetDayOfWeek()
    {
        return dayOfWeek;
    }

    public int GetFlightNumber()
    {
        return flightNumber;
    }

    public string GetStationName()
    {
        return stationName;
    }
}

class ParallelProcessingExample
{
    static void Main(string[] args)
    {
        List<Route> routes = new List<Route>();
        // Додайте ваші дані про маршрути сюди
        routes.Add(new Route("StationA", 100, "Monday", 101));
        routes.Add(new Route("StationB", 80, "Tuesday", 102));
        routes.Add(new Route("StationC", 120, "Wednesday", 103));
        routes.Add(new Route("StationD", 90, "Thursday", 104));
        routes.Add(new Route("StationE", 110, "Friday", 105));

        Container container = new Container(routes);

        var tasks = new List<Task<List<Route>>>();
        tasks.Add(Task.Run(() => container.SortBySeats()));
        tasks.Add(Task.Run(() => container.SortByDayOfWeek()));
        tasks.Add(Task.Run(() => container.SortByFlightNumber()));

        Task.WhenAll(tasks).Wait();

        List<Route> resultBySeats = tasks[0].Result;
        List<Route> resultByDayOfWeek = tasks[1].Result;
        List<Route> resultByFlightNumber = tasks[2].Result;

        // Порівнюємо час паралельної обробки і послідовної обробки
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        List<Route> sequentialResultBySeats = container.SortBySeats();
        List<Route> sequentialResultByDayOfWeek = container.SortByDayOfWeek();
        List<Route> sequentialResultByFlightNumber = container.SortByFlightNumber();
        stopwatch.Stop();
        long sequentialTime = stopwatch.ElapsedTicks;

        Console.WriteLine("Час послідовної обробки: " + sequentialTime + " тіків");

        // Обробка і виведення результатів паралельної обробки
        Console.WriteLine("Результати сортування за кількістю місць (паралельно):");
        foreach (Route route in resultBySeats)
        {
            Console.WriteLine(route.GetStationName() + " - " + route.GetAvailableSeats());
        }

        Console.WriteLine("Результати сортування за днем тижня (паралельно):");
        foreach (Route route in resultByDayOfWeek)
        {
            Console.WriteLine(route.GetStationName() + " - " + route.GetDayOfWeek());
        }

        Console.WriteLine("Результати сортування за номером рейсу (паралельно):");
        foreach (Route route in resultByFlightNumber)
        {
            Console.WriteLine(route.GetStationName() + " - " + route.GetFlightNumber());
        }
    }
}
