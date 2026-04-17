from __future__ import annotations

import re

_ESCAPED_ASTERISK = "\ufff0"
_ESCAPED_UNDERSCORE = "\ufff1"
_ESCAPED_BACKTICK = "\ufff2"
_CODE_SPAN_MARKER = "\ufff3"


def parse(markdown: str) -> str:
    if not markdown:
        return ""

    lines = markdown.split("\n")
    blocks: list[str] = []
    i = 0

    while i < len(lines):
        line = lines[i]

        # Code block (fenced)
        if line.lstrip().startswith("```"):
            i += 1
            code_lines: list[str] = []
            while i < len(lines) and not lines[i].lstrip().startswith("```"):
                code_lines.append(lines[i])
                i += 1
            if i < len(lines):
                i += 1  # skip closing fence
            blocks.append(f"<pre><code>{chr(10).join(code_lines)}</code></pre>")
            continue

        # Blank line
        if not line.strip():
            i += 1
            continue

        # Heading
        heading = _parse_heading(line)
        if heading is not None:
            blocks.append(heading)
            i += 1
            continue

        # Unordered list
        if line.startswith("- "):
            items: list[str] = []
            while i < len(lines) and lines[i].startswith("- "):
                items.append(f"<li>{_apply_inline_formatting(lines[i][2:])}</li>")
                i += 1
            blocks.append(f"<ul>{''.join(items)}</ul>")
            continue

        # Blockquote
        if line.startswith("> "):
            content = _apply_inline_formatting(line[2:])
            blocks.append(f"<blockquote>{content}</blockquote>")
            i += 1
            continue

        # Paragraph
        paragraph_lines: list[str] = []
        while (
            i < len(lines)
            and lines[i].strip()
            and not lines[i].startswith("- ")
            and not lines[i].startswith("> ")
            and not lines[i].lstrip().startswith("```")
            and not _is_heading(lines[i])
        ):
            paragraph_lines.append(lines[i])
            i += 1
        text = " ".join(paragraph_lines)
        blocks.append(f"<p>{_apply_inline_formatting(text)}</p>")

    return "".join(blocks)


def _is_heading(line: str) -> bool:
    return bool(re.match(r"^#{1,6} .+$", line))


def _parse_heading(line: str) -> str | None:
    match = re.match(r"^(#{1,6}) (.+)$", line)
    if not match:
        return None
    level = len(match.group(1))
    content = _apply_inline_formatting(match.group(2))
    return f"<h{level}>{content}</h{level}>"


def _apply_inline_formatting(text: str) -> str:
    # Escape sequences first
    text = text.replace("\\*", _ESCAPED_ASTERISK)
    text = text.replace("\\_", _ESCAPED_UNDERSCORE)
    text = text.replace("\\`", _ESCAPED_BACKTICK)

    # Extract inline code spans before other formatting
    code_spans: list[str] = []

    def _capture_code(m: re.Match[str]) -> str:
        code_spans.append(m.group(1))
        return f"{_CODE_SPAN_MARKER}{len(code_spans) - 1}{_CODE_SPAN_MARKER}"

    text = re.sub(r"`([^`]+)`", _capture_code, text)

    # Bold
    text = re.sub(r"\*\*(.+?)\*\*", r"<strong>\1</strong>", text)

    # Italic
    text = re.sub(r"_(.+?)_", r"<em>\1</em>", text)

    # Links
    text = re.sub(r"\[([^\]]+)\]\(([^)]+)\)", r'<a href="\2">\1</a>', text)

    # Restore inline code spans
    def _restore_code(m: re.Match[str]) -> str:
        return f"<code>{code_spans[int(m.group(1))]}</code>"

    text = re.sub(rf"{_CODE_SPAN_MARKER}(\d+){_CODE_SPAN_MARKER}", _restore_code, text)

    # Restore escaped characters
    text = text.replace(_ESCAPED_ASTERISK, "*")
    text = text.replace(_ESCAPED_UNDERSCORE, "_")
    text = text.replace(_ESCAPED_BACKTICK, "`")

    return text
