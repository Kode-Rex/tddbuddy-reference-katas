import type { Equipment } from './Equipment.js';
import { EquipmentSlot } from './Equipment.js';
import { EquipmentCapacityExceededException } from './Exceptions.js';
import { Level, levelFor } from './Level.js';
import { Skill } from './Skill.js';

const BaseActions = 3;
const MaxWounds = 2;
const BaseCapacity = 5;
const MaxInHand = 2;

export class Survivor {
  private readonly _equipment: Equipment[] = [];
  private readonly _skills: Skill[] = [];
  private _wounds = 0;
  private _experience = 0;

  constructor(public readonly name: string) {}

  get wounds(): number { return this._wounds; }
  get experience(): number { return this._experience; }
  get isAlive(): boolean { return this._wounds < MaxWounds; }

  get level(): Level { return levelFor(this._experience); }

  get actionsPerTurn(): number {
    const bonus = this._skills.filter(s => s === Skill.PlusOneAction).length;
    return BaseActions + bonus;
  }

  get equipmentCapacity(): number {
    const hoardBonus = this._skills.filter(s => s === Skill.Hoard).length;
    return BaseCapacity - this._wounds + hoardBonus;
  }

  get equipment(): readonly Equipment[] { return this._equipment; }
  get skills(): readonly Skill[] { return this._skills; }

  get inHandCount(): number {
    return this._equipment.filter(e => e.slot === EquipmentSlot.InHand).length;
  }

  get inReserveCount(): number {
    return this._equipment.filter(e => e.slot === EquipmentSlot.InReserve).length;
  }

  receiveWound(): boolean {
    if (!this.isAlive) return false;
    this._wounds++;
    return true;
  }

  equip(itemName: string): void {
    if (this._equipment.length >= this.equipmentCapacity) {
      throw new EquipmentCapacityExceededException(
        `Cannot carry more than ${this.equipmentCapacity} pieces of equipment.`,
      );
    }
    const slot = this.inHandCount < MaxInHand ? EquipmentSlot.InHand : EquipmentSlot.InReserve;
    this._equipment.push({ name: itemName, slot });
  }

  discard(itemName: string): void {
    const index = this._equipment.findIndex(e => e.name === itemName);
    if (index >= 0) this._equipment.splice(index, 1);
  }

  get needsToDiscard(): boolean {
    return this._equipment.length > this.equipmentCapacity;
  }

  killZombie(): void {
    this._experience++;
  }

  checkLevelUp(previousExperience: number): Level | null {
    const previousLevel = levelFor(previousExperience);
    if (this.level !== previousLevel) return this.level;
    return null;
  }

  unlockSkill(skill: Skill): void {
    this._skills.push(skill);
  }
}
