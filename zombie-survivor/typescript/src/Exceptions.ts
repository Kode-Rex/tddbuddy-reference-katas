export class EquipmentCapacityExceededException extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'EquipmentCapacityExceededException';
  }
}

export class DuplicateSurvivorNameException extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'DuplicateSurvivorNameException';
  }
}
