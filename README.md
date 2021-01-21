# Pumox
## Pumox GmbH / Rekrutacja Backend Developer
### Rozwiązanie zadania testowego
### paul.piotr@gmail.com

# Instalacja

### W celu pobrania źródeł projektu należy wykonać następujące komendy GIT:

```
git clone https://github.com/paulpiotr/Pumox.git
```

### Następnie trzeba pobrać sub-moduły - komenda

```
cd Pumox/ && git submodule sync --recursive && git submodule init && git submodule update && git submodule foreach git checkout master && git submodule foreach git pull origin master
```

# Instalacja - wersja binarna

### Wersje binarne dostępne są pod adresami is skompilowane dla Windows:

https://github.com/paulpiotr/Pumox.Core.WebApplication.Release/tree/master/win-x64

https://github.com/paulpiotr/Pumox.Core.WebApplication.Release/tree/master/win-x86

### Przygotowałem również wersję instalatora pod adresem:

https://github.com/paulpiotr/Pumox.Core.WebApplication.Release/tree/master/installer-win-x86/Setup%20Files

# Instalacja - baza danych

### W projekcie użyłem instalatora lokalnej bazy danych - baza danych powinna automatycznie wstać w systemie Windows po wysłaniu pierwszego żądania.

### Plik konfiguracyjny bazy danych znajduję się w projekcie, zostanie skopiowany do katalogu użytkownika po uruchomieniu programu. Lokalizacja w standardowej lokalizacji użytkownika np.:

```
C:\Users\...nazwa użytkownika...\NetAppCommon\pumox.core.database.json
```

### W celu zmiany połączenia do bazy danych należy ustawić ciąg połączenia w PumoxCoreDatabaseContext

```
"ConnectionStrings": {
    "PumoxCoreDatabaseContext": "..."
}
```

## Po uruchomieniu aplikacji połączenie zostanie zaszyfrowane !!!

## Program działa pod adresem http://localhtost:5000 - nie konfigurowałem ssl (https)

## W razie pytań jestem do dyspozycji

