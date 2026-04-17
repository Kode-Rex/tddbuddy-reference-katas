export enum EquipmentSlot {
  InHand = 'InHand',
  InReserve = 'InReserve',
}

export interface Equipment {
  readonly name: string;
  readonly slot: EquipmentSlot;
}
