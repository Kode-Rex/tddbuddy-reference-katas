import { Survivor } from '../src/Survivor.js';

export class SurvivorBuilder {
  private _name = 'Bob';
  private _wounds = 0;
  private _zombieKills = 0;
  private _equipment: string[] = [];

  named(name: string): this { this._name = name; return this; }
  withWounds(wounds: number): this { this._wounds = wounds; return this; }
  withZombieKills(kills: number): this { this._zombieKills = kills; return this; }
  withEquipment(...items: string[]): this {
    this._equipment.push(...items);
    return this;
  }

  build(): Survivor {
    const survivor = new Survivor(this._name);
    for (let i = 0; i < this._zombieKills; i++) survivor.killZombie();
    for (const item of this._equipment) survivor.equip(item);
    for (let i = 0; i < this._wounds; i++) survivor.receiveWound();
    return survivor;
  }
}
