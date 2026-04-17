import { OrderIncompleteError } from './OrderIncompleteError.js';
import type { PartOption } from './PartOption.js';
import { AllPartTypes, type PartType } from './PartType.js';

export class RobotOrder {
  private readonly _parts = new Map<PartType, PartOption>();

  get parts(): ReadonlyMap<PartType, PartOption> { return this._parts; }

  configure(type: PartType, option: PartOption): void {
    this._parts.set(type, option);
  }

  validate(): void {
    const missing = AllPartTypes.filter(t => !this._parts.has(t));
    if (missing.length > 0) {
      throw new OrderIncompleteError(
        `Order is missing part types: ${missing.join(', ')}`);
    }
  }
}
