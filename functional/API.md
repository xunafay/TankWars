# Functionele analyse van de API

De API biedt een interface voor de bot om te interacteren met de game-omgeving.
Er bestaan twee hoofdonderdelen van de API: acties en informatie.

Elke turn krijgt de bot de mogelijkheid om acties uit te voeren en informatie op te vragen.

## Acties

Of een actie succesvol is hangt af van de de instelling en van de engine. De API zal een boolean teruggeven die aangeeft of de actie succesvol was of niet.

Volgorde van acties:

- Beweging
- Rotatie
- Schieten

### Beweging

Vooruit/achteruit

### Rotatie

Draaien van de turret - Turrets kunnen in 8 posities draaien (N, NE, E, SE, S, SW, W, NW)
Draaien van de tank - Tanks kunnen in 4 posities draaien (N, E, S, W)

### Schieten

- Schieten van een kogel, de kogel zal in de richting van de turret worden afgevuurd.
- Schieten is alleen mogelijk als er munitie beschikbaar is en de turret niet in koelstatus is.

## Informatie

### Status

- Identiteit (zie specificaties)
- Capabilities (zie specificaties)
- Positie
- Inventory
- Gezondheid
- Max gezondheid
- Munitie
- Max munitie
- Koelstatus van de turret in turns (0 als de turret klaar is om te schieten)

### Omgeving

- Zichtbare map
  - Obstakels
  - Power-ups

- Zichtbare vijanden
  - Identiteit
  - Positie
  - Gezondheid
  - Capabilities

- Zichtbare kogels
  - Positie
  - Oriëntatie
  - Owner (ID van de speler die de kogel heeft afgevuurd)
  - Snelheid

## Specificaties

### Identiteit

- Unieke ID
- Auteur
- Team ID
- Naam

### Capabilities

- Kan draaien en bewegen tegelijkertijd
- Rotatie snelheid
- Beweging snelheid
- Inventory grootte
- Max munitie
- Max gezondheid
- Turret koeltijd

### Inventory

- Power-ups
  - Type
  - Effect
  - Duur (in turns)
- Size

### Positie

- X-coördinaat
- Y-coördinaat
- Oriëntatie (N, NE, E, SE, S, SW, W, NW voor turret; N, E, S, W voor tank)

### Gezondheid

Hit points (HP)
Max HP

### Munitie

- Aantal kogels
- Max munitie

### Map informatie

#### Zichtbare tiles

- Tile type
- (optioneel) obstakel gezondheid
- (Optioneel) Power-up type

#### Globale map informatie

- Power-up locaties
- Spelers
  - Identiteit
  - Levend/dood
- Turn nummer
- Map grootte
