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
        //Deklarujemy słownik, który będzie przechowywał historię lokalizacji graczy
        private readonly Dictionary<int, List<Location>> _history = new();

        //Deklarujemy metodę RegisterPlayer, która rejestruje gracza w trackerze i inicjalizuje jego historię lokalizacji
        public void RegisterPlayer(Player player)
        {
            //Sprawdzamy, czy gracz już jest zarejestrowany, jeśli tak, to nic nie robimy
            //UWAGA: tu nadpisujemy historię, lepiej dodać warunek aby nie nadpisywać przypadkiem
            if (!_history.ContainsKey(player.Id))
            {
                _history[player.Id] = new List<Location> { player.CurrentLocation };
            }

            //Rejestrujemy zdarzenie LocationChanged, które będzie wywoływane, gdy zmieni się lokalizacja gracza
            player.LocationChanged += OnLocationChange;
        }

        //Deklarujemy metodę OnLocationChange, która będzie wywoływana, gdy zmieni się lokalizacja gracza
        private void OnLocationChange(object? sender, Location newLocation)
        {
            //Sprawdzamy, czy sender jest instancją klasy Player, jeśli tak, to dodajemy nową lokalizację do historii gracza
            if (sender is Player player)
            {
                //Sprawdzamy, czy gracz jest zarejestrowany, jeśli nie, to nic nie robimy
                if (_history.ContainsKey(player.Id))
                {
                    _history[player.Id].Add(newLocation);
                }
            }
        }

        //Deklarujemy metodę GetPlayerWithLongestDistance, która zwraca identyfikator gracza, który pokonał najdłuższą odległość
        public int GetPlayerWithLongestDistance()
        {
            //Sprawdzamy, czy historia jest pusta, jeśli tak, to zwracamy -1 (brak gracza)
            double maxDistance = 0;
            int maxId = -1;

            //Iterujemy przez historię lokalizacji każdego gracza
            foreach (var typek in _history)
            {
                //Sprawdzamy, czy gracz ma co najmniej dwie lokalizacje, jeśli nie, to pomijamy go
                var locations = typek.Value;

                if (locations.Count < 2)
                    continue;

                //Definiujemy zmienną total, która będzie przechowywać całkowitą odległość pokonaną przez gracza
                double total = 0;

                //Iterujemy przez lokalizacje gracza, obliczając odległość między kolejnymi punktami
                for (int i = 1; i < locations.Count; i++)
                {
                    //Dodajemy odległość między kolejnymi lokalizacjami do zmiennej total
                    total += locations[i - 1].DistanceTo(locations[i]);
                }

                //Sprawdzamy, czy całkowita odległość jest większa niż maksymalna odległość, jeśli tak, to aktualizujemy maksymalną odległość i identyfikator gracza
                if (total > maxDistance)
                {
                    //Aktualizujemy maksymalną odległość i identyfikator gracza
                    maxDistance = total;

                    //Aktualizujemy identyfikator gracza, który pokonał najdłuższą odległość
                    maxId = typek.Key;
                }
            }

            //Zwracamy identyfikator gracza, który pokonał najdłuższą odległość
            return maxId;
        }
    }
}
