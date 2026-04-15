from datetime import date

from .library_builder import LibraryBuilder

DAY_0 = date(2026, 1, 1)


def test_new_library_has_no_members():
    library, _, _ = LibraryBuilder().opened_on(DAY_0).build()
    assert library.members == []


def test_registering_a_member_adds_them_to_the_library():
    library, _, _ = LibraryBuilder().opened_on(DAY_0).build()
    member = library.register("Ada Lovelace")
    assert len(library.members) == 1
    assert library.members[0] is member
    assert member.name == "Ada Lovelace"
