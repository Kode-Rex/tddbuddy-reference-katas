from character_copy import copy

from .recording_destination import RecordingDestination
from .string_source import StringSource


def test_immediate_newline_copies_nothing():
    source = StringSource("")
    destination = RecordingDestination()

    copy(source, destination)

    assert destination.written == ""


def test_single_character_then_newline_copies_that_character():
    source = StringSource("a")
    destination = RecordingDestination()

    copy(source, destination)

    assert destination.written == "a"


def test_multiple_characters_then_newline_copies_all_of_them_in_order():
    source = StringSource("abc")
    destination = RecordingDestination()

    copy(source, destination)

    assert destination.written == "abc"


def test_copy_preserves_whitespace_before_the_terminator():
    source = StringSource("a b")
    destination = RecordingDestination()

    copy(source, destination)

    assert destination.written == "a b"


def test_copy_does_not_read_past_the_terminator():
    source = StringSource("abc")
    destination = RecordingDestination()

    copy(source, destination)

    assert source.read_count == 4


def test_copy_writes_exactly_as_many_characters_as_were_read_before_the_terminator():
    source = StringSource("quip")
    destination = RecordingDestination()

    copy(source, destination)

    assert len(destination.written) == 4
