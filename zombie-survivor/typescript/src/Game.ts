import type { Clock } from './Clock.js';
import type { HistoryEntry } from './HistoryEntry.js';
import { DuplicateSurvivorNameException } from './Exceptions.js';
import { Level, maxLevel } from './Level.js';
import { Skill } from './Skill.js';
import { Survivor } from './Survivor.js';

export class Game {
  private readonly _clock: Clock;
  private readonly _survivors: Survivor[] = [];
  private readonly _history: HistoryEntry[] = [];
  private _gameLevel: Level = Level.Blue;
  private _ended = false;

  constructor(clock: Clock) {
    this._clock = clock;
    this.recordEvent('Game started.');
  }

  get survivors(): readonly Survivor[] { return this._survivors; }
  get history(): readonly HistoryEntry[] { return this._history; }
  get gameLevel(): Level { return this._gameLevel; }
  get hasEnded(): boolean { return this._ended; }

  addSurvivor(survivor: Survivor): void {
    if (this._survivors.some(s => s.name === survivor.name)) {
      throw new DuplicateSurvivorNameException(
        `A survivor named '${survivor.name}' already exists.`,
      );
    }
    this._survivors.push(survivor);
    this.recordEvent(`Survivor added: ${survivor.name}.`);
  }

  woundSurvivor(survivor: Survivor): void {
    const wasAlive = survivor.isAlive;
    const changed = survivor.receiveWound();
    if (!changed) return;

    this.recordEvent(`Wound received: ${survivor.name}.`);

    if (wasAlive && !survivor.isAlive) {
      this.recordEvent(`Survivor died: ${survivor.name}.`);
      this.checkGameEnd();
    }
  }

  equipSurvivor(survivor: Survivor, itemName: string): void {
    survivor.equip(itemName);
    this.recordEvent(`Equipment acquired: ${survivor.name} picked up ${itemName}.`);
  }

  killZombie(survivor: Survivor): void {
    const previousXp = survivor.experience;
    survivor.killZombie();
    const levelUp = survivor.checkLevelUp(previousXp);
    if (levelUp !== null) {
      this.recordEvent(`Level up: ${survivor.name} reached ${levelUp}.`);
      if (levelUp === Level.Yellow) {
        survivor.unlockSkill(Skill.PlusOneAction);
      }
      this.updateGameLevel();
    }
  }

  private updateGameLevel(): void {
    let highest = Level.Blue;
    for (const s of this._survivors) {
      if (s.isAlive) highest = maxLevel(highest, s.level);
    }
    if (highest !== this._gameLevel) {
      this._gameLevel = highest;
      this.recordEvent(`Game level changed to ${this._gameLevel}.`);
    }
  }

  private checkGameEnd(): void {
    if (this._survivors.length > 0 && this._survivors.every(s => !s.isAlive)) {
      this._ended = true;
      this.recordEvent('Game ended.');
    }
  }

  private recordEvent(description: string): void {
    this._history.push({ timestamp: this._clock.now(), description });
  }
}
