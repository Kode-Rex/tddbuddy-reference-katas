# Weather Station — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Reading** | A single weather observation: temperature, humidity, wind speed, recorded at a point in time |
| **Station** | The aggregate root — records readings and computes statistics over its history |
| **Temperature** | Degrees Celsius, expressed as a decimal |
| **Humidity** | Percentage (0–100), expressed as a decimal |
| **WindSpeed** | Kilometres per hour, expressed as a decimal |
| **Clock** | Collaborator that returns "now" — injected so tests control time without sleeping |
| **Alert** | A condition notification when a reading crosses a configured threshold |

## Domain Rules

- A new station starts with **no readings** and **no statistics**
- **Recording a reading** requires a valid temperature, humidity (0–100), and non-negative wind speed
- Temperature has **no artificial bounds** — the domain accepts any value (sub-zero, extreme heat)
- Humidity **must be between 0 and 100** inclusive; values outside are rejected
- Wind speed **must be non-negative**; negative values are rejected
- Each accepted reading is timestamped via the injected clock
- Rejected readings **leave the station unchanged** — no reading recorded, no statistics affected
- **Statistics** are computed over all recorded readings:
  - Minimum, maximum, and average temperature
  - Minimum, maximum, and average humidity
  - Maximum wind speed
  - Average wind speed
- Requesting statistics from a station with **no readings** is rejected
- **Alerts** fire when a reading crosses a threshold:
  - High temperature alert when temperature exceeds a configured ceiling
  - Low temperature alert when temperature drops below a configured floor
  - High wind alert when wind speed exceeds a configured limit
- Alert messages are **byte-identical across languages**

## Test Scenarios

### Empty Station

1. **New station has no readings**
2. **Requesting statistics from a station with no readings is rejected**

### Recording Readings

3. **Recording a valid reading increases the reading count**
4. **Recording a reading captures the clock timestamp**
5. **Recording a reading with humidity below zero is rejected**
6. **Recording a reading with humidity above 100 is rejected**
7. **Recording a reading with negative wind speed is rejected**
8. **Rejected reading leaves the station unchanged**

### Temperature Statistics

9. **Minimum temperature is the lowest recorded temperature**
10. **Maximum temperature is the highest recorded temperature**
11. **Average temperature is the mean of all recorded temperatures**
12. **A single reading makes min, max, and average equal**

### Humidity Statistics

13. **Minimum humidity is the lowest recorded humidity**
14. **Maximum humidity is the highest recorded humidity**
15. **Average humidity is the mean of all recorded humidities**

### Wind Statistics

16. **Maximum wind speed is the highest recorded wind speed**
17. **Average wind speed is the mean of all recorded wind speeds**

### Alerts

18. **High temperature alert fires when temperature exceeds the ceiling**
19. **No high temperature alert when temperature is at or below the ceiling**
20. **Low temperature alert fires when temperature drops below the floor**
21. **No low temperature alert when temperature is at or above the floor**
22. **High wind alert fires when wind speed exceeds the limit**
23. **No high wind alert when wind speed is at or below the limit**
24. **Multiple alerts can fire for a single reading**
