import pytest

from parking_lot import InvalidLotConfigurationError
from tests.lot_builder import LotBuilder


class TestLotConstruction:
    def test_a_lot_with_spots_across_all_types_is_valid(self) -> None:
        LotBuilder().with_motorcycle_spots(2).with_compact_spots(3).with_large_spots(1).build()

    def test_a_lot_with_zero_total_spots_raises_invalid_lot_configuration_error(self) -> None:
        with pytest.raises(InvalidLotConfigurationError, match="Lot must have at least one spot"):
            LotBuilder().with_motorcycle_spots(0).with_compact_spots(0).with_large_spots(0).build()
