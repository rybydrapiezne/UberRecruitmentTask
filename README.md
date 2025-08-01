# UberRecruitmentTask

## Opis projektu
Projekt jest realizacją zadania rekrutacyjnego "Basic DOTS Multiplayer". Implementuje prostą grę wieloosobową z wykorzystaniem Unity Netcode for Entities.

![Gif z rozgrywki](https://github.com/rybydrapiezne/UberRecruitmentTask/blob/main/2025-07-31%2023-03-29.gif)  
*Uwaga: GIF jest trochę wolny. Płynniejszy podgląd znajduje się w pliku `.mkv` w repozytorium.*

## Wykonane zadania
- **Główne funkcjonalności**:
  - Obsługa maksymalnie dwóch graczy z użyciem Unity Netcode for Entities (dedykowany serwer, bez hostowania lobby).
  - Każdy gracz steruje unikalnym modelem humanoida z animacjami realizowanymi przez Unity Animator.
  - Gracze poruszają się niezależnie po płaszczyźnie bez interfejsu użytkownika, początku/końca gry czy konkretnego celu.
  - Kod zgodny z konwencją nazewnictwa Microsoftu dla C#, z nazwami funkcji i komentarzami po angielsku.
  - Zbudowany w Unity 6000.0.51f1 z URP.
- **Funkcjonalność opcjonalna**:
  - System zbierania monet: Jedna moneta pojawia się w losowym miejscu. Gracz zbiera ją, wchodząc w odpowiednią odległość, co powoduje przeniesienie monety w nowe miejsce. Obaj gracze mogą zbierać monety (bez zapisywania stanu)

## Co wyszło i szczegóły implementacji
1. **System ruchu graczy**:
   Na początku został stworzony MovementSystem dla pojedynczego gracza tak aby mógł poruszać sie bez problemów po plane'ie. Następnie tak przygotowany system został przebudowany tak aby działał z Unity Netcode for       Entities. Gracze są reprezentowani jako entity z odzwierciedleniem ich postaci jako Game Object w celu implementacji animacji (wyjaśnione w dalszych krokach).
2. **System sieciowy**: Po przygotowaniu systemu ruchu dla gracza stworzony został podstawowy model sieciowy z wykorzystaniem Unity Netcode for Entities i przetestowany przy użyciu przerobionego MovementSystemu. Wynikiem tego kroku były dwa humanoidy w T-pose przesuwające się po plane'ie.
3. **System animacji**: Zważywszy, że system Rukhanka troche kosztuje a zadanie przewidywało użycie Animatora, zdecydowano się na podejście hybrydowe wykorzystujące GameObject do prezentacji gracza oraz entity jako faktyczne odzwierciedlenie gracza w grze. Każdy gracz reprezentowany jest za pomocą entity, do którego dodatkowo synchronizowana jest pozycja GameObjectu faktycznego modelu gracza z animatorem. Jest to podejście wykraczające poza założenia ECS aczkolwiek jest dosyć proste i szybkie w implementacji jednakże trzeba się liczyć, że rezygnuje się w takim podejściu z pełnego usprawniania projektu z użyciem ECS i Burst Compilera.

4. **System zbierania monet (funkcjonalność opcjonalna)**: Po wykonaniu wszystkich przewidzianych w głównym zadaniu systemów stworzony został system zbierania monet. Na plane'ie poza graczami znajduję się również jedna moneta, którą gracze mogą zebrać poprzez podejście do niej. Gdy gracz znajdzie się wystarczająco blisko moneta zmienia swoje położenie. System jest przygotowany tak, że w przyszłości łatwa byłaby implementacja zapisywania stanu zebranych monet.

Ze względu na ograniczenia czasowe nie zostały zaimplementowane dodatkowe mechaniki.

## Co nie wyszło
Przede wszystkim nie udało się zaimplementować innych mechanik. W głównej mierze nacisk kładziony był na główne mechaniki przewidziane w zadaniu a dopiero w drugiej kolejności mechaniki dodatkowe. W idealnie przygotowanym systemie zbierania monet, zapisywany byłby stan zebranych monet dla każdego z graczy, jednakże ograniczenia czasowe nie pozwoliły na dalszy rozwój aczkolwiek tak jak wcześniej zostało wspomniane system przygotowany jest tak, że dalszy rozwój tej mechaniki byłby łatwy.

## Uwagi
- Użyto placeholderowych assetów do demonstracji funkcjonalności.
Linki do assetów:
Model gracza: https://assetstore.unity.com/packages/3d/characters/humanoids/lowpoly-survival-character-rio-273074
Model gracza: https://assetstore.unity.com/packages/3d/characters/humanoids/humans/human-character-dummy-178395
Model monety: https://iamsujitcu.itch.io/3d-coin




