from multi_threaded_santa import Pipeline, Present, PresentState


class TestSequentialPipeline:
    def test_a_single_present_flows_through_all_four_stages(self) -> None:
        pipeline = Pipeline(made_capacity=10, wrapped_capacity=10, loaded_capacity=10)
        present = Present(1)

        pipeline.process_sequentially([present])

        assert present.state == PresentState.DELIVERED
        assert len(pipeline.delivered) == 1
        assert pipeline.delivered[0].id == 1

    def test_multiple_presents_all_complete_the_full_pipeline(self) -> None:
        pipeline = Pipeline(made_capacity=10, wrapped_capacity=10, loaded_capacity=10)
        presents = [Present(i) for i in range(1, 11)]

        pipeline.process_sequentially(presents)

        assert all(p.state == PresentState.DELIVERED for p in presents)
        assert len(pipeline.delivered) == 10

    def test_presents_emerge_from_the_pipeline_in_order(self) -> None:
        pipeline = Pipeline(made_capacity=10, wrapped_capacity=10, loaded_capacity=10)
        presents = [Present(i) for i in range(1, 6)]

        pipeline.process_sequentially(presents)

        assert [p.id for p in pipeline.delivered] == [1, 2, 3, 4, 5]
