from enum import Enum


class PartOption(Enum):
    # Head
    STANDARD_VISION = "StandardVision"
    INFRARED_VISION = "InfraredVision"
    NIGHT_VISION = "NightVision"

    # Body
    SQUARE = "Square"
    ROUND = "Round"
    TRIANGULAR = "Triangular"
    RECTANGULAR = "Rectangular"

    # Arms
    HANDS = "Hands"
    PINCHERS = "Pinchers"
    BOXING_GLOVES = "BoxingGloves"

    # Movement
    WHEELS = "Wheels"
    LEGS = "Legs"
    TRACKS = "Tracks"

    # Power
    SOLAR = "Solar"
    RECHARGEABLE_BATTERY = "RechargeableBattery"
    BIOMASS = "Biomass"
