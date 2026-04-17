import { Game } from '../src/Game.js';
import { Survivor } from '../src/Survivor.js';
import { FixedClock } from './FixedClock.js';

type GameAction = (game: Game) => void;

export class HistoryBuilder {
  private readonly _startTime = new Date(Date.UTC(2026, 0, 1, 12, 0, 0));
  private readonly _actions: GameAction[] = [];

  withSurvivor(name: string): this {
    this._actions.push(game => game.addSurvivor(new Survivor(name)));
    return this;
  }

  withWound(survivorName: string): this {
    this._actions.push(game => {
      const survivor = game.survivors.find(s => s.name === survivorName)!;
      game.woundSurvivor(survivor);
    });
    return this;
  }

  withZombieKill(survivorName: string, count = 1): this {
    this._actions.push(game => {
      const survivor = game.survivors.find(s => s.name === survivorName)!;
      for (let i = 0; i < count; i++) game.killZombie(survivor);
    });
    return this;
  }

  build(): { game: Game; clock: FixedClock } {
    const clock = new FixedClock(this._startTime);
    const game = new Game(clock);
    for (const action of this._actions) action(game);
    return { game, clock };
  }
}
