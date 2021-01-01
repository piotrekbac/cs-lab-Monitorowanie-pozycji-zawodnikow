# Monitorowanie pozycji zawodników

* Autor: _Krzysztof Molenda_
* Wersja: 2020-12-05

## Problem

Tworzysz oprogramowanie do monitorowania przemieszczania się _K_ zawodników (np. _K_=5) po wirtualnym boisku o rozmiarze _N_ x _M_ (np. 500x1000).

W klasie `Location` rejestrujesz aktualny czas oraz położenie zawodnika na boisku.

Zawodnika opisuje klasa `Player` - jego `id` oraz aktualne położenie. Zawodnik przesuwa się po boisku w różnych kierunkach (o rozsądne odległości - raz szybciej, innym razem wolniej). Zawodnik nie opuszcza boiska, czasami nie przemieszcza się. Fakt przesunięcia się zawodnika realizuje wywołanie metody `ChangeLocation` zaimplementowanej w klasie `Player`.

W klasie `Tracker` zapamiętujesz aktualne pozycje zawodników (dowolnej liczby). Fakt ten realizuje metoda `OnLocationChange` obsługująca zdarzenia zmiany miejsca zawodnika (zawodnicy zgłaszają zmiany swoich pozycji).

## Zadanie

Zaimplementuj ten prosty system komunikowania się zawodników z monitorem śledzącym ich ruchy z wykorzystaniem `event`-ów (w projekcie typu _Class Library_).

Napisz prostą aplikację konsolową - symulator weryfikujący poprawność:

* utwórz _K_ zawodników,
* zawodnicy rejestrują się w monitorze śledzącym ich ruchy,
* zmuś zawodników do losowego poruszania się lub nie poruszania się (zmiany pozycji rozsądnie, bez odległych skoków) - zmiany pozycji zawodników uruchom w oddzielnych wątkach.

Po zakończeniu symulacji określ, który z zawodników pokonał największą odległość.

## Zadanie dodatkowe

Zrealizuj opakowanie wizualne - w formie aplikacji okienkowej. Aplikacja powinna wyświetlać ruchy zawodników na wirtualnej planszy. Po zakończeniu może wyświetlać ścieżkę danego zawodnika. Możesz również zrealizować inne własne pomysły.

> ⚠️ Przykładowe wykonanie zadań znajdziesz w folderze [./example-bin](./example-bin) - dwa pliki `.exe` odpowiednio dla aplikacji konsolowej i desktopowej, bazujące na kodzie wspólnej biblioteki (wymagane środowisko .Net5).

---

## Co dalej ...

Jeśli wykonałeś powyższe proste zadanie, zrozumiałeś, jak działa komunikacja między obiektami z wykorzystaniem zdarzeń i ich obsługi.

W C#4 do **.Net Framework** dodano _Location API_: [System.Device.Location](https://docs.microsoft.com/pl-pl/dotnet/api/system.device.location?view=netframework-4.8).

> UWAGA: Nie ma tego API w .Net Standard ani .Net Core.

W tej przestrzeni nazw zdefiniowano podobną infrastrukturę do tej z zadania.

* Zapoznaj się z typami zdefiniowanymi w [System.Device.Location](https://docs.microsoft.com/pl-pl/dotnet/api/system.device.location?view=netframework-4.8)
* Rzuć okiem na projekty przykładowe, skompiluj, uruchom, zmodyfikuj:
  * <https://github.com/microsoft/Windows-universal-samples/tree/master/Samples/Geolocation>
  * <https://github.com/Microsoft/Windows-appsample-lunch-scheduler>
  * <https://github.com/microsoft/windows-appsample-trafficapp/>
