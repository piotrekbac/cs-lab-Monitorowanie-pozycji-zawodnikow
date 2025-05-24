using BibliotekaTracker;

//Piotr Bacior 15 722 - WSEI Kraków 
class Program
{
    static void Main()
    {
        //Inicjalizujemy liczbę graczy, tracker i generator liczb losowych 

        //Liczbę graczy ustawiamy na 5
        int K = 5;

        //Tworzymy instancję klasy Tracker, która będzie śledzić lokalizacje graczy
        var tracker = new Tracker();

        //Tworzymy instancję klasy Random, która będzie używana do generowania losowych lokalizacji
        var rand = new Random();

        //Tworzymy listę graczy, którzy będą uczestniczyć w symulacji
        var players = new List<Player>();

        //Rejestrujemy K graczy w trackerze, każdy z losową początkową lokalizacją
        for (int i = 0; i < K; i++)
        {
            //Generujemy losową lokalizację startową dla każdego gracza, gdzie X jest w zakresie 0-500, a Y w zakresie 0-1000
            var startLoc = new Location(rand.Next(500), rand.Next(1000));

            //Tworzymy nowego gracza z unikalnym identyfikatorem i początkową lokalizacją, a następnie rejestrujemy go w trackerze
            var p = new Player(i, startLoc);

            //Rejestrujemy gracza w trackerze, aby śledzić jego lokalizację
            tracker.RegisterPlayer(p);

            //Dodajemy gracza do listy graczy, aby móc później symulować jego ruchy
            players.Add(p);
        }

        //Teraz przechodzimy do symulacji ruchów graczy, gdzie każdy gracz będzie wykonywał 50 ruchów
        var tasks = players.Select(p => Task.Run(() =>
        {
            //Dla każdego gracza wykonujemy 50 ruchów, gdzie każdy ruch jest opóźniony losowo
            for (int j = 0; j < 50; j++)
            {
                //Generujemy losowe opóźnienie między 100 a 500 milisekund
                Thread.Sleep(rand.Next(100, 500));

                //Generujemy losowe przesunięcie w osi X i Y, gdzie wartości są w zakresie -10 do 10
                double dx = (rand.NextDouble() - 0.5) * 20;
                double dy = (rand.NextDouble() - 0.5) * 20;

                //Wywołujemy metodę ChangeLocation gracza, aby zmienić jego lokalizację o wygenerowane przesunięcia
                p.ChangeLocation(dx, dy);
            }

        //Po zakończeniu ruchów, wypisujemy końcową lokalizację gracza
        })).ToArray();

        //Czekamy na zakończenie wszystkich zadań, czyli ruchów graczy
        Task.WaitAll(tasks);

        //Po zakończeniu symulacji, wypisujemy końcową lokalizację każdego gracza
        int maxPlayer = tracker.GetPlayerWithMaxDistance();

        //Wypisujemy komunikat informujący, który gracz pokonał największą odległość
        Console.WriteLine($"Zawodnik {maxPlayer} pokonał największą odległość.");
    }
}
