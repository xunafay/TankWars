# Functionele analyse van de Engine

De engine bevat de flow om een game te spelen/simuleren. 

## Initialize

De engine heeft volgende argumenten nodig om te kunnen starten:

- Map
- Tanks

## Beurt verloop

In een turn moeten volgende zaken gebeuren in volgende volgorde:

1. Reset tank states (HasFired, HasMoved...)
2. Doe alle beweeg tank acties
3. Doe alle draai acties
4. Doe alle schiet acties
5. Beweeg alle kogels
6. Check of het spel klaar is

### Reset tank states

Alvorens er iets kan gebeuren moet alle state terug gereset worden. Vermoedelijk in de vorm van een `tank.ResetState()`

### Beweeg tanks

Als een bot vraagt om zijn tank te bewegen moet er rekening gehouden worden met volgende zaken:

- Blijft de tank binnen de map?
- Is de tile waar de tank naartoe wilt bewegen betreedbaar?
- Is de tile al bezet door een andere tank?
- Is er al een andere tank die naar deze tile wilt bewegen?

Als alle voorwaarden goed zijn gaat de tank naar de volgende tile.

### Draai turrets

Als een bot vraagt om de tank te draaien, draait de tank naar de nieuwe richting.

### Schiet voor elke tank

Als een bot vraagt om de tank te doen schieten moet er rekening gehouden worden met volgende zaken:

- Heeft de tank nog een cooldown op zijn turret?
- Heeft de tank nog ammo?
- Staat de tank op een tile waarvan deze kan schieten?

Als alle voorwaarden goed zijn schiet de tank en wordt de cooldown gezet + de ammo verlaagt.

### Beweeg alle kogels

Een kogel beweegt elke turn x tiles in de richting waarin deze geschoten is. Voor elke tile waar de kogel door gaat moeten de volgende checks gebeuren:

- Is de kogel uit de map gevlogen?
- Staat er een tank op de tile?
- Stopt de tile de kogel?

Indien de kogel gestopt wordt door een tank moet de tank zijn health aangepast worden op basis van de tile waar de tank op staat.

Als de kogel een tile raakt die de kogel stopt gaat de tile zijn health omlaag, een tile die geen health meer heeft wordt vervangen door een andere tile (nog te bepalen welke) die geen kogels stopt.

Als de kogel uit de map gaat wordt de kogel uit het spel verwijderd.

### Check of het spel klaar is

Na dat alle acties zijn uitgevoerd, checkt de engine of het spel klaar is. Een spel is klaar als het in één van de volgende states is:

- Alle tanks zijn kapot
- Er is nog maar 1 team levend

## Aanvullende informatie

- Een tank die destroyed is blijft voor de rest van het spel staan op de tile waar die is gedestroyed.
- Een destroyed tank stopt ook kogels