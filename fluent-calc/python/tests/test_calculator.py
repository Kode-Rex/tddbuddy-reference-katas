from fluent_calc import Calculator


def test_a_new_calculators_result_is_zero():
    assert Calculator().result() == 0


def test_seeding_sets_the_starting_value():
    assert Calculator().seed(10).result() == 10


def test_plus_adds_to_the_seeded_value():
    assert Calculator().seed(10).plus(5).result() == 15


def test_minus_subtracts_from_the_seeded_value():
    assert Calculator().seed(10).minus(4).result() == 6


def test_operations_chain_in_order():
    assert Calculator().seed(10).plus(5).plus(5).result() == 20


def test_subsequent_seed_calls_are_ignored():
    assert Calculator().seed(10).seed(99).plus(5).result() == 15


def test_plus_before_seed_is_ignored():
    assert Calculator().plus(5).seed(10).result() == 10


def test_undo_reverses_the_most_recent_operation():
    assert Calculator().seed(10).plus(5).undo().result() == 10


def test_undo_twice_reverses_two_operations():
    assert Calculator().seed(10).plus(5).minus(2).undo().undo().result() == 10


def test_undo_with_nothing_to_undo_is_a_no_op():
    assert Calculator().undo().result() == 0
    assert Calculator().seed(10).undo().undo().result() == 10


def test_redo_replays_the_most_recently_undone_operation():
    assert (
        Calculator().seed(10).plus(5).minus(2).undo().undo().redo().result() == 15
    )


def test_redo_with_nothing_to_redo_is_a_no_op():
    assert Calculator().seed(10).plus(5).redo().result() == 15


def test_a_new_operation_after_undo_clears_the_redo_stack():
    assert Calculator().seed(10).plus(5).undo().plus(3).redo().result() == 13


def test_the_full_undo_redo_example_from_the_spec():
    assert (
        Calculator().seed(10).plus(5).minus(2).undo().undo().redo().result() == 15
    )


def test_save_clears_history_so_undo_has_no_effect():
    assert (
        Calculator()
        .seed(10)
        .plus(5)
        .minus(2)
        .save()
        .undo()
        .redo()
        .undo()
        .plus(5)
        .result()
        == 18
    )
