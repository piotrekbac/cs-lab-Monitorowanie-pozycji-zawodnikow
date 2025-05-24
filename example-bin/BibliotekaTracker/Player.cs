using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaTracker
{

    // Piotr Bacior 15 722 - WSEI Kraków

    public class Player
    {
        //Deklarujemy właściwość Id, czyli identyfikator gracza    
        public int Id { get; }

        //Deklarujemy właściwość CurrentLocation, czyli aktualną lokalizację naszego gracza
        public Location CurrentLocation { get; private set; }

        //Deklarujemy stałe MaxX i MaxY, które określają maksymalne wartości współrzędnych X i Y
        private const double MaxX = 1000;
        private const double MaxY = 500;

        //Deklarujemy zdarzenie LocationChanged, które będzie wywoływane, gdy zmieni się lokalizacja gracza
        public event EventHandler<Location>? LocationChanged;

        //Teraz przechodzimy do realizacji konstruktora klasy Player, który przyjmuje dwa argumenty: id gracza i jego początkową lokalizację
        public Player(int id, Location initialLocation)
        {
            //Przypisujemy wartości przekazane do konstruktora do właściwości Id i CurrentLocation
            Id = id;
            CurrentLocation = initialLocation;
        }

        //Teraz przechodzimy do realizacji metody ChangeLocation, która zmienia lokalizację gracza o zadane wartości dx i dy
        public void ChangeLocation(double dx, double dy)
        {
            //Obliczamy nową lokalizację, dodając przesunięcia dx i dy do aktualnych współrzędnych X i Y
            double newX = Math.Clamp(CurrentLocation.X + dx, 0, MaxX);
            double newY = Math.Clamp(CurrentLocation.Y + dy, 0, MaxY);

            //Tworzymy nową instancję klasy Location z obliczonymi współrzędnymi
            var newLocation = new Location(newX, newY);

            //Sprawdzamy, czy nowa lokalizacja jest różna od aktualnej lokalizacji
            if (newLocation.DistanceTo(CurrentLocation) > 0.01)
            {
                //Jeśli tak, to aktualizujemy CurrentLocation i wywołujemy zdarzenie LocationChanged
                CurrentLocation = newLocation;

                // Wywołujemy zdarzenie LocationChanged, przekazując nową lokalizację
                LocationChanged?.Invoke(this, newLocation);
            }
        }
    }
}
