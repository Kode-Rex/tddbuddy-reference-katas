from hundred_doors import open_doors


def test_zero_doors_leaves_no_doors_open():
    assert open_doors(0) == []


def test_one_door_is_open_after_the_single_pass():
    assert open_doors(1) == [1]


def test_ten_doors_leaves_the_perfect_squares_one_four_and_nine_open():
    assert open_doors(10) == [1, 4, 9]


def test_one_hundred_doors_leaves_the_ten_perfect_squares_open():
    assert open_doors(100) == [1, 4, 9, 16, 25, 36, 49, 64, 81, 100]
