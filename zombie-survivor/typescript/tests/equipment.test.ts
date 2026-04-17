import { describe, it, expect } from 'vitest';
import { EquipmentCapacityExceededException } from '../src/Exceptions.js';
import { SurvivorBuilder } from './SurvivorBuilder.js';

describe('Equipment', () => {
  it('New survivor can carry up to five pieces of equipment', () => {
    const survivor = new SurvivorBuilder().build();
    expect(survivor.equipmentCapacity).toBe(5);
  });

  it('Survivor can hold up to two items in hand', () => {
    const survivor = new SurvivorBuilder()
      .withEquipment('Bat', 'Pistol')
      .build();
    expect(survivor.inHandCount).toBe(2);
  });

  it('Remaining equipment goes in reserve', () => {
    const survivor = new SurvivorBuilder()
      .withEquipment('Bat', 'Pistol', 'Medkit')
      .build();
    expect(survivor.inHandCount).toBe(2);
    expect(survivor.inReserveCount).toBe(1);
  });

  it('Equipping a sixth item is rejected', () => {
    const survivor = new SurvivorBuilder()
      .withEquipment('Bat', 'Pistol', 'Medkit', 'Axe', 'Shield')
      .build();
    expect(() => survivor.equip('Grenade'))
      .toThrow(EquipmentCapacityExceededException);
    expect(() => survivor.equip('Grenade'))
      .toThrow('Cannot carry more than 5 pieces of equipment.');
  });

  it('One wound reduces carrying capacity to four', () => {
    const survivor = new SurvivorBuilder().withWounds(1).build();
    expect(survivor.equipmentCapacity).toBe(4);
  });

  it('Wound with full equipment requires discarding one item', () => {
    const survivor = new SurvivorBuilder()
      .withEquipment('Bat', 'Pistol', 'Medkit', 'Axe', 'Shield')
      .build();
    survivor.receiveWound();
    expect(survivor.needsToDiscard).toBe(true);
    survivor.discard('Shield');
    expect(survivor.needsToDiscard).toBe(false);
    expect(survivor.equipment.length).toBe(4);
  });
});
