from dataclasses import dataclass
from enum import Enum
from typing import Optional


class TestStatus(Enum):
    PASS = "PASS"
    FAIL = "FAIL"
    ERROR = "ERROR"


@dataclass(frozen=True)
class TestResult:
    name: str
    status: TestStatus
    message: Optional[str] = None
