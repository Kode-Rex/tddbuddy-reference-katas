from typing import List

from string_transformer import (
    Capitalise,
    CamelCase,
    Pipeline,
    Repeat,
    Replace,
    Reverse,
    RemoveWhitespace,
    SnakeCase,
    Transformation,
    Truncate,
)


class PipelineBuilder:
    def __init__(self) -> None:
        self._steps: List[Transformation] = []

    def capitalise(self) -> "PipelineBuilder":
        self._steps.append(Capitalise())
        return self

    def reverse(self) -> "PipelineBuilder":
        self._steps.append(Reverse())
        return self

    def remove_whitespace(self) -> "PipelineBuilder":
        self._steps.append(RemoveWhitespace())
        return self

    def snake_case(self) -> "PipelineBuilder":
        self._steps.append(SnakeCase())
        return self

    def camel_case(self) -> "PipelineBuilder":
        self._steps.append(CamelCase())
        return self

    def truncate(self, n: int) -> "PipelineBuilder":
        self._steps.append(Truncate(n))
        return self

    def repeat(self, n: int) -> "PipelineBuilder":
        self._steps.append(Repeat(n))
        return self

    def replace(self, target: str, replacement: str) -> "PipelineBuilder":
        self._steps.append(Replace(target, replacement))
        return self

    def build(self) -> Pipeline:
        return Pipeline(steps=list(self._steps))
