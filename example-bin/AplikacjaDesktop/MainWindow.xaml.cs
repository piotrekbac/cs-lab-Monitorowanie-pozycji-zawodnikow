using BibliotekaTracker;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Piotr Bacior 15 722 - WSEI Kraków

namespace AplikacjaDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    //Definiujemy klasę MainWindow, która dziedziczy po klasie Window
    public partial class MainWindow : Window
    {
        //Deklarujemy stałą PlayerCount, która określa liczbę graczy
        private const int PlayerCount = 5;

        //Tworzymy instancję klasy Tracker, która będzie śledzić lokalizacje graczy
        private readonly Tracker tracker = new();

        //Tworzymy listę graczy, którzy będą uczestniczyć w symulacji
        private readonly List<Player> players = new();

        //Tworzymy słownik, który będzie przechowywał ostatnie znane lokalizacje graczy
        private readonly Dictionary<int, Location> playerLastPosition = new();

        //Tworzymy słownik, który będzie przechowywał tagi graczy, aby wyświetlać ich identyfikatory na planszy
        private readonly Dictionary<int, TextBlock> playerLabels = new();
        //Tworzymy słownik, który będzie przechowywał kolory graczy, aby wizualizować ich ruchy na planszy
        private readonly Dictionary<int, Brush> playerColors = new()
        {
            //Przypisujemy kolory do graczy według ich identyfikatorów
            { 0, Brushes.Red },
            { 1, Brushes.Green },
            { 2, Brushes.Blue },
            { 3, Brushes.Orange },
            { 4, Brushes.Purple }
        };

        //Konstruktor klasy MainWindow, który jest wywoływany podczas tworzenia instancji okna
        public MainWindow()
        {
            //Inicjalizujemy komponenty okna, co jest standardową praktyką w aplikacjach WPF
            InitializeComponent();
        }

        //Definiujemy metodę obsługującą kliknięcie przycisku Start, która rozpoczyna symulację ruchów graczy
        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            //Tworzymy instancję klasy Random, aby generować losowe liczby
            Random rand = new();

            //Czyścimy planszę z poprzednich graczy i ich lokalizacji
            players.Clear();

            //Czyścimy planszę z poprzednich linii i kropki
            playerLastPosition.Clear();

            //Czyścimy planszę z poprzednich etykiet graczy
            NaszPlaygroundPB.Children.Clear();

            // Pobieramy aktualne rozmiary naszego pola Canvas
            double maxX = NaszPlaygroundPB.ActualWidth;
            double maxY = NaszPlaygroundPB.ActualHeight;

            // Jeśli rozmiar Canvas jest równy 0, to oznacza, że kontrolka nie została jeszcze wymierzona
            if (maxX == 0 || maxY == 0)
            {
                //Jeśli kontrolka nie jest jeszcze wymierzona, użyj domyślnych wartości
                maxX = 1000;
                maxY = 500;
            }

            //Rejestrujemy graczy w trackerze, każdy z losową początkową lokalizacją
            for (int i = 0; i < PlayerCount; i++)
            {
                //Generujemy losową lokalizację startową dla każdego gracza, gdzie X jest w zakresie 0-maxX, a Y w zakresie 0-maxY
                var startX = rand.NextDouble() * maxX;
                var startY = rand.NextDouble() * maxY;

                //Tworzymy nowego gracza z unikalnym identyfikatorem i początkową lokalizacją, a następnie rejestrujemy go w trackerze
                var player = new Player(i, new Location(startX, startY));

                //Rejestrujemy gracza w trackerze, aby śledzić jego lokalizację
                player.LocationChanged += OnLocationChanged;

                //Rejestrujemy gracza w trackerze, aby śledzić jego lokalizację
                tracker.RegisterPlayer(player);

                //Dodajemy gracza do listy graczy, aby móc później symulować jego ruchy
                players.Add(player);

                //Dodajemy gracza do słownika ostatnich znanych lokalizacji, aby śledzić jego ruchy
                playerLastPosition[i] = player.CurrentLocation;

                //Rysujemy kropkę na planszy, aby zaznaczyć początkową lokalizację gracza
                Ellipse startDot = new()
                {
                    //Ustawiamy właściwości elipsy, aby reprezentować początkową lokalizację gracza
                    Width = 8,
                    Height = 8,
                    Fill = playerColors[i]
                };

                //Ustawiamy pozycję elipsy na planszy, aby była widoczna w odpowiednim miejscu
                Canvas.SetLeft(startDot, startX - 4);
                Canvas.SetTop(startDot, startY - 4);

                //Dodajemy elipsę do planszy, aby była widoczna na ekranie
                NaszPlaygroundPB.Children.Add(startDot);


                //Rysujemy etykietę na planszy, aby zaznaczyć identyfikator gracza
                TextBlock label = new()
                {
                    //Ustawiamy właściwości etykiety, aby reprezentować identyfikator gracza
                    Text = $"ID: {i}",
                    Foreground = playerColors[i],
                    FontWeight = FontWeights.Bold,
                    FontSize = 12
                };

                //Ustawiamy pozycję etykiety na planszy, aby była widoczna w odpowiednim miejscu
                Canvas.SetLeft(label, startX + 10);
                Canvas.SetTop(label, startY - 10);

                //Dodajemy etykietę do planszy, aby była widoczna na ekranie
                NaszPlaygroundPB.Children.Add(label);

                //Dodajemy etykietę do słownika playerLabels, aby móc później aktualizować jej pozycję
                playerLabels[i] = label;
            }

            //Blokujemy przycisk Start, aby nie można było go kliknąć podczas symulacji
            if (sender is Button startButton)
                startButton.IsEnabled = false;

            //Uruchamiamy symulację ruchów graczy, gdzie każdy gracz będzie wykonywał 50 ruchów
            var tasks = players.Select(p => Task.Run(async () =>
            {
                //Dla każdego gracza wykonujemy 50 ruchów, gdzie każdy ruch jest opóźniony losowo
                for (int i = 0; i < 50; i++)
                {
                    //Generujemy losowe opóźnienie między 100 a 500 milisekund
                    double dx = (rand.NextDouble() - 0.5) * 20;
                    double dy = (rand.NextDouble() - 0.5) * 20;

                    //Generujemy losowe opóźnienie między 100 a 300 milisekund
                    if (rand.NextDouble() < 0.2)
                    {
                        await Task.Delay(rand.Next(100, 300));
                        continue;
                    }

                    //Obliczamy nową pozycję i ogranicz ją do obszaru planszy
                    double newX = p.CurrentLocation.X + dx;
                    double newY = p.CurrentLocation.Y + dy;

                    //Ograniczamy X i Y do widocznego obszaru (uwzględniając margines 4, aby kropka się nie obcięła)
                    newX = Math.Max(4, Math.Min(newX, maxX - 4));
                    newY = Math.Max(4, Math.Min(newY, maxY - 4));

                    //Obliczamy przesunięcie po ograniczeniu
                    double limitedDx = newX - p.CurrentLocation.X;
                    double limitedDy = newY - p.CurrentLocation.Y;

                    //Wywołujemy metodę ChangeLocation gracza, aby zmienić jego lokalizację o wygenerowane przesunięcia
                    p.ChangeLocation(limitedDx, limitedDy);

                    //Wypisujemy komunikat informujący o nowej lokalizacji gracza
                    await Task.Delay(rand.Next(100, 300));
                }

                //Po zakończeniu ruchów, wypisujemy końcową lokalizację gracza
            })).ToArray();

            //Czekamy na zakończenie wszystkich zadań, czyli ruchów graczy
            await Task.WhenAll(tasks);

            //Po zakończeniu symulacji, wywołujemy metodę ShowResult, aby wyświetlić wynik
            Dispatcher.Invoke(() =>
            {
                //Wyświetlamy komunikat z wynikiem, czyli identyfikatorem gracza, który przeszedł najwięcej
                int winnerId = tracker.GetPlayerWithLongestDistance();
                MessageBox.Show($"Zawodnik {winnerId} przeszedł najwięcej!", "Wynik");

                //Odblokowujemy przycisk Start, aby można było go kliknąć ponownie
                if (sender is Button startButtonEnd)
                    startButtonEnd.IsEnabled = true;
            });
        }


        //Definiujemy metodę, która będzie wywoływana, gdy zmieni się lokalizacja gracza
        private void OnLocationChanged(object? sender, Location newLocation)
        {
            //Sprawdzamy, czy sender jest instancją klasy Player, jeśli nie, to wychodzimy z metody
            if (sender is not Player player) return;

            //Sprawdzamy, czy gracz jest zarejestrowany w słowniku ostatnich znanych lokalizacji, jeśli nie, to dodajemy nową lokalizację
            if (!playerLastPosition.ContainsKey(player.Id))
                playerLastPosition[player.Id] = newLocation;

            //Aktualizujemy ostatnią znaną lokalizację gracza i rysujemy linię na planszy, aby pokazać jego ruch
            Location previous = playerLastPosition[player.Id];
            playerLastPosition[player.Id] = newLocation;

            //Rysujemy linię na planszy, aby pokazać ruch gracza
            Dispatcher.Invoke(() =>
            {
                //Tworzymy linię, która będzie łączyć poprzednią lokalizację z nową lokalizacją gracza
                Line line = new()
                {
                    //Ustawiamy właściwości linii, takie jak współrzędne początkowe i końcowe, kolor i grubość
                    X1 = previous.X,
                    Y1 = previous.Y,
                    X2 = newLocation.X,
                    Y2 = newLocation.Y,

                    //Ustawiamy kolor linii na kolor gracza, aby wizualizować jego ruch
                    Stroke = playerColors[player.Id],

                    //Ustawiamy grubość linii, aby była widoczna na planszy
                    StrokeThickness = 2
                };

                //Teraz dodajemy linię do planszy, aby była widoczna na ekranie
                NaszPlaygroundPB.Children.Add(line);

                //Rysujemy kropkę na nowej lokalizacji gracza, aby zaznaczyć jego aktualne położenie
                Ellipse dot = new()
                {
                    //Ustawiamy właściwości elipsy, aby reprezentować nową lokalizację gracza
                    Width = 8,
                    Height = 8,
                    Fill = playerColors[player.Id]
                };

                //Ustawiamy pozycję elipsy na planszy, aby była widoczna w odpowiednim miejscu
                Canvas.SetLeft(dot, newLocation.X - 4);
                Canvas.SetTop(dot, newLocation.Y - 4);

                //Dodajemy elipsę do planszy, aby była widoczna na ekranie
                NaszPlaygroundPB.Children.Add(dot);

                //Sprawdzamy, czy etykieta gracza jest zarejestrowana w słowniku playerLabels, jeśli tak, to aktualizujemy jej pozycję
                if (playerLabels.TryGetValue(player.Id, out var label))
                {
                    //Ustawiamy pozycję etykiety gracza na planszy, aby była widoczna w odpowiednim miejscu
                    Canvas.SetLeft(label, newLocation.X + 10);
                    Canvas.SetTop(label, newLocation.Y - 10);
                }

            });
        }

        //Definiujemy metodę obsługującą kliknięcie przycisku Finish, która kończy symulację i wyświetla wynik
        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            //Sprawdzamy, czy tracker ma jakichkolwiek graczy, jeśli nie, to wyświetlamy komunikat i wychodzimy z metody
            int winnerId = tracker.GetPlayerWithLongestDistance();

            //Wyświetlamy komunikat z wynikiem, czyli identyfikatorem gracza, który przeszedł najwięcej
            MessageBox.Show($"Zawodnik {winnerId} przeszedł najwięcej!", "Wynik");
        }
    }
}