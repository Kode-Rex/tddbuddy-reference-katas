from datetime import datetime, timedelta


def convert(local: datetime, from_offset: timedelta, to_offset: timedelta) -> datetime:
    """Convert a naive local datetime from one fixed UTC offset to another.

    Inputs and outputs are both naive (tzinfo=None); offsets are signed
    timedeltas (e.g. timedelta(hours=-5), timedelta(hours=5, minutes=30)).
    """
    return local - from_offset + to_offset
