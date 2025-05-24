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

        //Deklarujemy zdarzenie LocationChanged, które będzie wywoływane, gdy zmieni się lokalizacja gracza
        public event EventHandler<Location> LocationChanged;

        //Teraz przechodzimy do realizacji konstruktora klasy Player, który przyjmuje dwa argumenty: id gracza i jego początkową lokalizację
        public Player(int id, Location initialLocation)
        {
            //Przypisujemy wartości przekazane do konstruktora do właściwości Id i CurrentLocation
            Id = id;
            CurrentLocation = initialLocation;
        }

        //Teraz przechodzimy do realizacji metody ChangeLocation, która zmienia lokalizację gracza o zadane wartości deltaX i deltaY
        public void ChangeLocation(double deltaX, double deltaY)
        {
            //Obliczamy nowe współrzędne X i Y, upewniając się, że mieszczą się w określonym zakresie (0-500 dla X i 0-1000 dla Y)
            double newX = Math.Max(0, Math.Min(CurrentLocation.X + deltaX, 500));
            double newY = Math.Max(0, Math.Min(CurrentLocation.Y + deltaY, 1000));

            //Tworzymy nowy obiekt Location z nowymi współrzędnymi
            CurrentLocation = new Location(newX, newY);

            //Wywołujemy zdarzenie LocationChanged, przekazując nową lokalizację
            LocationChanged?.Invoke(this, CurrentLocation);
        }
    }
}
