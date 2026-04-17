import type { PartOption } from '../src/PartOption.js';
import type { PartType } from '../src/PartType.js';
import { RobotOrder } from '../src/RobotOrder.js';

export class RobotOrderBuilder {
  private head: PartOption = 'StandardVision';
  private body: PartOption = 'Square';
  private arms: PartOption = 'Hands';
  private movement: PartOption = 'Wheels';
  private power: PartOption = 'Solar';
  private readonly excluded = new Set<PartType>();

  withHead(option: PartOption): this { this.head = option; return this; }
  withBody(option: PartOption): this { this.body = option; return this; }
  withArms(option: PartOption): this { this.arms = option; return this; }
  withMovement(option: PartOption): this { this.movement = option; return this; }
  withPower(option: PartOption): this { this.power = option; return this; }
  without(type: PartType): this { this.excluded.add(type); return this; }

  build(): RobotOrder {
    const order = new RobotOrder();
    if (!this.excluded.has('Head')) order.configure('Head', this.head);
    if (!this.excluded.has('Body')) order.configure('Body', this.body);
    if (!this.excluded.has('Arms')) order.configure('Arms', this.arms);
    if (!this.excluded.has('Movement')) order.configure('Movement', this.movement);
    if (!this.excluded.has('Power')) order.configure('Power', this.power);
    return order;
  }
}
