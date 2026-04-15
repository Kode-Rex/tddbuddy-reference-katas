export interface LocalDateTime {
  year: number;
  month: number; // 1-indexed
  day: number;
  hour: number;
  minute: number;
  second: number;
}

export interface Offset {
  hours: number;
  minutes: number;
}

const MS_PER_MINUTE = 60_000;

const totalMinutes = (offset: Offset): number => offset.hours * 60 + offset.minutes;

export function convert(local: LocalDateTime, fromOffset: Offset, toOffset: Offset): LocalDateTime {
  const utcMs = Date.UTC(local.year, local.month - 1, local.day, local.hour, local.minute, local.second);
  const shiftMinutes = totalMinutes(toOffset) - totalMinutes(fromOffset);
  const target = new Date(utcMs + shiftMinutes * MS_PER_MINUTE);
  return {
    year: target.getUTCFullYear(),
    month: target.getUTCMonth() + 1,
    day: target.getUTCDate(),
    hour: target.getUTCHours(),
    minute: target.getUTCMinutes(),
    second: target.getUTCSeconds(),
  };
}
