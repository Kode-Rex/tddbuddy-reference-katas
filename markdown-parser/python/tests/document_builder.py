from __future__ import annotations


class DocumentBuilder:
    def __init__(self) -> None:
        self._lines: list[str] = []

    def with_line(self, line: str) -> DocumentBuilder:
        self._lines.append(line)
        return self

    def with_blank_line(self) -> DocumentBuilder:
        self._lines.append("")
        return self

    def with_heading(self, level: int, text: str) -> DocumentBuilder:
        self._lines.append(f"{'#' * level} {text}")
        return self

    def with_paragraph(self, text: str) -> DocumentBuilder:
        self._lines.append(text)
        return self

    def with_list_item(self, text: str) -> DocumentBuilder:
        self._lines.append(f"- {text}")
        return self

    def with_blockquote(self, text: str) -> DocumentBuilder:
        self._lines.append(f"> {text}")
        return self

    def with_code_block(self, content: str) -> DocumentBuilder:
        self._lines.append("```")
        self._lines.append(content)
        self._lines.append("```")
        return self

    def build(self) -> str:
        return "\n".join(self._lines)
