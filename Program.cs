using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaApp
{
    class Program
    {
        static List<Movie> Movies = new List<Movie>();
        static List<Seat> Seats = new List<Seat>();
        static Accounting Accounting = new Accounting();

        static void Main(string[] args)
        {
            SeedMovies();
            SeedSeats();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CINEMA SALES SYSTEM ===");
                Console.WriteLine("1. Sell Ticket");
                Console.WriteLine("2. Sell Snack");
                Console.WriteLine("3. View Accounting");
                Console.WriteLine("4. Exit");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": SellTicket(); break;
                    case "2": SellSnack(); break;
                    case "3": ShowAccounting(); break;
                    case "4": return;
                    default: Console.WriteLine("Invalid choice"); break;
                }

                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
        }

        // -----------------------------
        // INITIAL DATA
        // -----------------------------
        static void SeedMovies()
        {
            Movies.Add(new Movie("Furiosa", new[] { "Tuesday 18:00", "Saturday 21:00" }));
            Movies.Add(new Movie("Dog Day Afternoon", new[] { "Thursday 21:00" }));
            Movies.Add(new Movie("The Fall Guy", new[] { "Tuesday 21:00", "Friday 21:00" }));
            Movies.Add(new Movie("Iron Man 3", new[] { "Wednesday 18:00", "Saturday 13:00" }));
            Movies.Add(new Movie("Civil War", new[] { "Friday 18:00", "Saturday 18:00" }));
            Movies.Add(new Movie("The Room Next Door", new[] { "Wednesday 21:00", "Thursday 18:00" }));
        }

        static void SeedSeats()
        {
            for (int i = 1; i <= 54; i++)
                Seats.Add(new Seat(i));
        }

        // -----------------------------
        // TICKET SALE
        // -----------------------------
        static void SellTicket()
        {
            Console.Clear();
            Console.WriteLine("=== SELL TICKET ===");

            // Choose movie
            for (int i = 0; i < Movies.Count; i++)
                Console.WriteLine($"{i + 1}. {Movies[i].Title}");

            Console.Write("Choose movie: ");
            int movieIndex = int.Parse(Console.ReadLine()) - 1;
            Movie movie = Movies[movieIndex];

            // Choose showtime
            Console.WriteLine("\nShowtimes:");
            for (int i = 0; i < movie.Showtimes.Length; i++)
                Console.WriteLine($"{i + 1}. {movie.Showtimes[i]}");

            Console.Write("Choose showtime: ");
            int timeIndex = int.Parse(Console.ReadLine()) - 1;
            string showtime = movie.Showtimes[timeIndex];

            // Choose seat
            Console.WriteLine("\nAvailable seats:");
            foreach (var seat in Seats.Where(s => !s.IsBooked))
                Console.Write(seat.Number + " ");

            Console.Write("\nChoose seat number: ");
            int seatNumber = int.Parse(Console.ReadLine());
            Seat chosenSeat = Seats.First(s => s.Number == seatNumber);
            chosenSeat.IsBooked = true;

            // Choose age category
            Console.WriteLine("\nAge category:");
            Console.WriteLine("1. Child below 6 (Free)");
            Console.WriteLine("2. Age 6–11 (65 SEK)");
            Console.WriteLine("3. Age 12–67 (90 SEK)");
            Console.WriteLine("4. Adult (Regular price)");
            Console.Write("Choose: ");
            int ageChoice = int.Parse(Console.ReadLine());

            decimal price = CalculateTicketPrice(showtime, ageChoice);

            // MAY campaign
            if (showtime.Contains("18:00"))
            {
                Console.Write("Enter campaign code (or press ENTER): ");
                string code = Console.ReadLine();

                if (code == "MAY26")
                {
                    price /= 2;
                    Console.WriteLine("Campaign applied! 50% off.");
                }
            }

            // VAT
            decimal vat = price * 0.25m;
            Accounting.TotalTicketSales += price;
            Accounting.VAT25 += vat;

            Console.WriteLine($"\nTicket sold! Price: {price} SEK (VAT 25%: {vat} SEK)");
        }

        static decimal CalculateTicketPrice(string showtime, int ageChoice)
        {
            if (ageChoice == 1) return 0;
            if (ageChoice == 2) return 65;
            if (ageChoice == 3) return 90;

            // Adult pricing
            if (showtime.Contains("13:00")) return 105;
            return 130;
        }

        // -----------------------------
        // SNACK SALE
        // -----------------------------
        static void SellSnack()
        {
            Console.Clear();
            Console.WriteLine("=== SELL SNACK ===");
            Console.WriteLine("1. Ahlgrens bilar – 22 SEK");
            Console.WriteLine("2. Popcorn + Coca-Cola – 43 SEK");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();
            decimal price = choice == "1" ? 22 : 43;

            decimal vat = price * 0.12m;
            Accounting.TotalSnackSales += price;
            Accounting.VAT12 += vat;

            Console.WriteLine($"\nSnack sold! Price: {price} SEK (VAT 12%: {vat} SEK)");
        }

        // -----------------------------
        // ACCOUNTING
        // -----------------------------
        static void ShowAccounting()
        {
            Console.Clear();
            Console.WriteLine("=== ACCOUNTING ===");
            Console.WriteLine($"Total ticket sales: {Accounting.TotalTicketSales} SEK");
            Console.WriteLine($"Total snack sales: {Accounting.TotalSnackSales} SEK");
            Console.WriteLine($"Total VAT 25%: {Accounting.VAT25} SEK");
            Console.WriteLine($"Total VAT 12%: {Accounting.VAT12} SEK");
        }
    }

    // -----------------------------
    // MODELS
    // -----------------------------
    class Movie
    {
        public string Title { get; set; }
        public string[] Showtimes { get; set; }

        public Movie(string title, string[] showtimes)
        {
            Title = title;
            Showtimes = showtimes;
        }
    }

    class Seat
    {
        public int Number { get; set; }
        public bool IsBooked { get; set; }

        public Seat(int number)
        {
            Number = number;
            IsBooked = false;
        }
    }

    class Accounting
    {
        public decimal TotalTicketSales { get; set; }
        public decimal TotalSnackSales { get; set; }
        public decimal VAT25 { get; set; }
        public decimal VAT12 { get; set; }
    }
}