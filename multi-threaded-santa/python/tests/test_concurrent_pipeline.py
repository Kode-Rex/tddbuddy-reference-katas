from multi_threaded_santa import Pipeline, Present, PresentState


class TestConcurrentPipeline:
    def test_multiple_workers_can_process_a_stage_concurrently(self) -> None:
        pipeline = Pipeline(made_capacity=50, wrapped_capacity=50, loaded_capacity=50)
        presents = [Present(i) for i in range(1, 21)]

        pipeline.process_concurrently(presents, make_workers=3, wrap_workers=2, load_workers=2)

        assert all(p.state == PresentState.DELIVERED for p in presents)
        assert len(pipeline.delivered) == 20

    def test_the_sleigh_constraint_allows_only_one_delivery_at_a_time(self) -> None:
        pipeline = Pipeline(made_capacity=50, wrapped_capacity=50, loaded_capacity=50)
        presents = [Present(i) for i in range(1, 31)]

        pipeline.process_concurrently(presents, make_workers=4, wrap_workers=3, load_workers=2)

        assert len(pipeline.delivered) == 30

    def test_loading_pauses_while_the_sleigh_is_delivering(self) -> None:
        pipeline = Pipeline(made_capacity=20, wrapped_capacity=20, loaded_capacity=20)
        presents = [Present(i) for i in range(1, 16)]

        pipeline.process_concurrently(presents, make_workers=2, wrap_workers=2, load_workers=2)

        assert all(p.state == PresentState.DELIVERED for p in presents)

    def test_all_presents_are_delivered_when_the_pipeline_completes(self) -> None:
        pipeline = Pipeline(made_capacity=100, wrapped_capacity=100, loaded_capacity=100)
        presents = [Present(i) for i in range(1, 51)]

        pipeline.process_concurrently(presents, make_workers=4, wrap_workers=3, load_workers=2)

        assert len(pipeline.delivered) == 50
        delivered_ids = sorted(p.id for p in pipeline.delivered)
        assert delivered_ids == list(range(1, 51))
