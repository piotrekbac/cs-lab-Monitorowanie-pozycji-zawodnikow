using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Piotr Bacior 15 722 - WSEI Kraków

namespace BibliotekaTracker
{
    public class Tracker
    {
        //Deklarujemy słownik, który będzie przechowywał identyfikatory graczy i ich lokalizacje
        private readonly Dictionary<int, List<Location>> _positions = new();

        //Deklarujemy metodę RegisterPlayer, która rejestruje gracza w systemie
        public void RegisterPlayer(Player player)
        {
            //Tworzymy nową listę lokalizacji dla gracza i dodajemy jego aktualną lokalizację do słownika 
            _positions[player.Id] = new List<Location> { player.CurrentLocation };

            //Subskrybujemy zdarzenie LocationChanged, aby śledzić zmiany lokalizacji gracza
            player.LocationChanged += OnLocationChanged;
        }

        //Deklarujemy teraz metode OnLocationChanged, która będzie wywoływana, gdy zmieni się lokalizacja gracza
        private void OnLocationChanged(object sender, Location loc)
        {
            //Sprawdzamy, czy sender jest instancją klasy Player i czy jego identyfikator znajduje się w słowniku _positions
            if (sender is Player p && _positions.ContainsKey(p.Id))
                _positions[p.Id].Add(loc);
        }

        //Deklarujemy metodę GetPlayerWithMaxDistance, która zwraca identyfikator gracza, który przebył największą odległość
        public int GetPlayerWithMaxDistance()
        {
            //Dla każdego gracza zwracamy jego identyfikator, a następnie sortujemy ich według przebytej odległości w porządku malejącym
            return _positions
                //Przechodzimy przez słownik, gdzie sortujemy według wartości, czyli przebytej odległości przez gracza
                .OrderByDescending(p => CalculateTotalDistance(p.Value))

                //Wybieramy pierwszego gracza z posortowanej listy - tego który przebył największą odległość
                .First().Key;
        }

        //Deklarujemy metodę CalculateTotalDistance, która oblicza całkowitą odległość przebyta przez gracza
        private double CalculateTotalDistance(List<Location> locations)
        {
            //Deklarujemy zmienną sum, która będzie przechowywała całkowitą odległość
            double sum = 0;

            //Sprawdzamy, czy lista lokalizacji ma więcej niż jeden element
            for (int i = 1; i < locations.Count; i++)
            {
                //Obliczamy różnicę między współrzędnymi X i Y dla kolejnych lokalizacji i dodajemy do sumy
                var dx = locations[i].X - locations[i - 1].X;
                var dy = locations[i].Y - locations[i - 1].Y;

                //Dodajemy do sumy pierwiastek z sumy kwadratów różnic współrzędnych
                sum += Math.Sqrt(dx * dx + dy * dy);
            }

            //Zwracamy całkowitą odległość
            return sum;
        }
    }
}
