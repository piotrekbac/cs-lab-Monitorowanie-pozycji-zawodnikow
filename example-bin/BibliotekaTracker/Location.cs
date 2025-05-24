namespace BibliotekaTracker
{
    
    //Piotr Bacior 15 722 - WSEI Kraków

    public class Location
    {
        //Deklarujemy właściwość X, czyl innymi słowy współrzędną X
        public double X { get; }

        //Deklarujemy właściwość Y, czyl innymi słowy współrzędną Y
        public double Y { get; }

        //Deklarujemy właściwość Timestamp, czyli znacznik czasu - dokładny czas utworzenia obiektu location
        public DateTime Timestamp { get; }

        //Teraz przechodzimy do realizacji konstruktora, który przyjmuje dwa argumenty typu double, czyli współrzędne X i Y
        public Location(double x, double y)
        {
            //Przypisujemy wartości przekazane do konstruktora do właściwości X i Y
            X = x;
            Y = y;

            //Ustawiamy znacznik czasu na aktualny czas
            Timestamp = DateTime.Now;
        }

        //Deklarujemy metodę DistanceTo, która oblicza odległość do innej lokalizacji
        public double DistanceTo(Location other)
        {
            //Przypisujemy różnicę między współrzędnymi X i Y do zmiennych dx i dy, a następnie obliczamy odległość
            double dx = X - other.X;
            double dy = Y - other.Y;

            //Zwracamy pierwiastek z sumy kwadratów różnic współrzędnych
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
