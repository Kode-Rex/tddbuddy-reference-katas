from tests.pipeline_builder import PipelineBuilder


def test_empty_pipeline_returns_the_input_unchanged():
    pipeline = PipelineBuilder().build()
    assert pipeline.run("hello world") == "hello world"


def test_capitalise_capitalises_each_word():
    pipeline = PipelineBuilder().capitalise().build()
    assert pipeline.run("hello world") == "Hello World"


def test_reverse_reverses_the_whole_string():
    pipeline = PipelineBuilder().reverse().build()
    assert pipeline.run("hello world") == "dlrow olleh"


def test_remove_whitespace_drops_every_space():
    pipeline = PipelineBuilder().remove_whitespace().build()
    assert pipeline.run("hello world") == "helloworld"


def test_snake_case_lowercases_and_joins_with_underscores():
    pipeline = PipelineBuilder().snake_case().build()
    assert pipeline.run("hello world") == "hello_world"


def test_snake_case_collapses_hyphens_and_whitespace_into_single_underscores():
    pipeline = PipelineBuilder().snake_case().build()
    assert pipeline.run("hello-world test") == "hello_world_test"


def test_camel_case_lowercases_the_first_word_and_title_cases_the_rest():
    pipeline = PipelineBuilder().camel_case().build()
    assert pipeline.run("Hello World") == "helloWorld"


def test_camel_case_normalises_all_uppercase_input():
    pipeline = PipelineBuilder().camel_case().build()
    assert pipeline.run("HELLO WORLD") == "helloWorld"


def test_truncate_shortens_long_input_and_appends_the_marker():
    pipeline = PipelineBuilder().truncate(5).build()
    assert pipeline.run("hello world") == "hello..."


def test_truncate_leaves_short_input_untouched():
    pipeline = PipelineBuilder().truncate(50).build()
    assert pipeline.run("hello world") == "hello world"


def test_repeat_produces_n_space_joined_copies():
    pipeline = PipelineBuilder().repeat(3).build()
    assert pipeline.run("ha") == "ha ha ha"


def test_replace_swaps_every_occurrence_of_the_target():
    pipeline = PipelineBuilder().replace("world", "there").build()
    assert pipeline.run("hello world") == "hello there"


def test_chaining_applies_transformations_in_order():
    pipeline = PipelineBuilder().capitalise().reverse().build()
    assert pipeline.run("hello world") == "dlroW olleH"


def test_chaining_snake_case_then_capitalise_uppercases_letters_after_underscores():
    pipeline = PipelineBuilder().snake_case().capitalise().build()
    assert pipeline.run("hello world") == "Hello_World"


def test_empty_input_survives_capitalise():
    pipeline = PipelineBuilder().capitalise().build()
    assert pipeline.run("") == ""
