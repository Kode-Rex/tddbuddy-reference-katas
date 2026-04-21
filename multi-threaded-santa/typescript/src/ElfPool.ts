/**
 * Manages a fixed-size pool of elf workers.
 * In Node.js, "workers" are concurrent async tasks, not OS threads.
 */
export class ElfPool {
  readonly elfCount: number;

  constructor(elfCount: number) {
    if (elfCount <= 0) {
      throw new Error('Elf pool must have at least one elf.');
    }
    this.elfCount = elfCount;
  }
}
