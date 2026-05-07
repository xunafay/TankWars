# Functionele analyse van de Engine

De engine bevat alle logica om een game te spelen/simuleren. 

## Initialize

De engine heeft volgende argumenten nodig om te kunnen starten:

- Map
- Tanks

## DoTurn

In een turn moeten volgende zaken gebeuren in volgende volgorde:

1. Reset tank states (HasFired, HasMoved...)
2. Doe alle beweeg tank acties
3. Beweeg alle reeds geschoten kogels
4. Doe alle draai acties
5. Doe alle schiet acties
6. Check of het spel klaar is