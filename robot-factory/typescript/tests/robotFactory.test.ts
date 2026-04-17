import { describe, expect, it } from 'vitest';
import { Factory } from '../src/Factory.js';
import { Money } from '../src/Money.js';
import { OrderIncompleteError } from '../src/OrderIncompleteError.js';
import { PartNotAvailableError } from '../src/PartNotAvailableError.js';
import { FakePartSupplier } from './FakePartSupplier.js';
import { RobotOrderBuilder } from './RobotOrderBuilder.js';
import { SupplierBuilder } from './SupplierBuilder.js';

function allPartsSupplier(name: string, price: number): FakePartSupplier {
  return new FakePartSupplier(name)
    .withPart('Head', 'StandardVision', new Money(price))
    .withPart('Body', 'Square', new Money(price))
    .withPart('Arms', 'Hands', new Money(price))
    .withPart('Movement', 'Wheels', new Money(price))
    .withPart('Power', 'Solar', new Money(price));
}

describe('Robot Factory', () => {
  // --- Order Validation ---

  it('order missing a part type is rejected as incomplete', () => {
    const order = new RobotOrderBuilder().without('Power').build();
    const supplier = new SupplierBuilder().named('AlphaParts').build();
    const factory = new Factory([supplier]);

    expect(() => factory.costRobot(order)).toThrow(OrderIncompleteError);
    expect(() => factory.costRobot(order)).toThrow('Order is missing part types: Power');
  });

  it('order with all five part types is accepted', () => {
    const order = new RobotOrderBuilder().build();
    const supplier = allPartsSupplier('AlphaParts', 10);
    const factory = new Factory([supplier]);

    expect(() => factory.costRobot(order)).not.toThrow();
  });

  // --- Costing — Single Supplier ---

  it('cost returns the suppliers price for each part', () => {
    const order = new RobotOrderBuilder().build();
    const supplier = allPartsSupplier('AlphaParts', 25);
    const factory = new Factory([supplier]);

    const breakdown = factory.costRobot(order);

    expect(breakdown.parts).toHaveLength(5);
    expect(breakdown.parts.every(q => q.price.amount === 25)).toBe(true);
  });

  it('cost total is the sum of all part prices', () => {
    const order = new RobotOrderBuilder().build();
    const supplier = allPartsSupplier('AlphaParts', 20);
    const factory = new Factory([supplier]);

    const breakdown = factory.costRobot(order);

    expect(breakdown.total.amount).toBe(100);
  });

  // --- Costing — Multiple Suppliers ---

  it('cost selects the cheapest quote when two suppliers carry the same part', () => {
    const order = new RobotOrderBuilder().build();
    const expensive = allPartsSupplier('Expensive', 50);
    const cheap = allPartsSupplier('Cheap', 10);
    const medium = allPartsSupplier('Medium', 30);
    const factory = new Factory([expensive, cheap, medium]);

    const breakdown = factory.costRobot(order);

    expect(breakdown.parts.every(q => q.supplierName === 'Cheap')).toBe(true);
  });

  it('cost selects parts from different suppliers when each is cheapest for different parts', () => {
    const order = new RobotOrderBuilder()
      .withHead('StandardVision')
      .withBody('Square')
      .build();

    const alpha = new SupplierBuilder().named('Alpha')
      .withPart('Head', 'StandardVision', 10)
      .withPart('Body', 'Square', 50)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const beta = new SupplierBuilder().named('Beta')
      .withPart('Head', 'StandardVision', 50)
      .withPart('Body', 'Square', 10)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const gamma = new SupplierBuilder().named('Gamma')
      .withPart('Head', 'StandardVision', 30)
      .withPart('Body', 'Square', 30)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const factory = new Factory([alpha, beta, gamma]);

    const breakdown = factory.costRobot(order);

    const head = breakdown.parts.find(p => p.type === 'Head')!;
    const body = breakdown.parts.find(p => p.type === 'Body')!;
    expect(head.supplierName).toBe('Alpha');
    expect(body.supplierName).toBe('Beta');
  });

  it('cost breakdown shows the winning supplier for each part', () => {
    const order = new RobotOrderBuilder().build();
    const alpha = allPartsSupplier('Alpha', 15);
    const beta = allPartsSupplier('Beta', 25);
    const gamma = allPartsSupplier('Gamma', 35);
    const factory = new Factory([alpha, beta, gamma]);

    const breakdown = factory.costRobot(order);

    expect(breakdown.parts.every(q => q.supplierName === 'Alpha')).toBe(true);
  });

  // --- Costing — Partial Catalogs ---

  it('cost succeeds when a part is available from only one of three suppliers', () => {
    const order = new RobotOrderBuilder()
      .withHead('NightVision')
      .build();

    const alpha = new SupplierBuilder().named('Alpha')
      .withPart('Head', 'NightVision', 100)
      .withPart('Body', 'Square', 20)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const beta = new SupplierBuilder().named('Beta')
      .withPart('Body', 'Square', 20)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const gamma = new SupplierBuilder().named('Gamma')
      .withPart('Body', 'Square', 20)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const factory = new Factory([alpha, beta, gamma]);

    const breakdown = factory.costRobot(order);

    const head = breakdown.parts.find(p => p.type === 'Head')!;
    expect(head.supplierName).toBe('Alpha');
  });

  it('cost fails with part not available when no supplier carries a required part', () => {
    const order = new RobotOrderBuilder()
      .withHead('InfraredVision')
      .build();

    const alpha = new SupplierBuilder().named('Alpha')
      .withPart('Body', 'Square', 20)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const beta = new SupplierBuilder().named('Beta')
      .withPart('Body', 'Square', 20)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const gamma = new SupplierBuilder().named('Gamma')
      .withPart('Body', 'Square', 20)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const factory = new Factory([alpha, beta, gamma]);

    expect(() => factory.costRobot(order)).toThrow(PartNotAvailableError);
    expect(() => factory.costRobot(order)).toThrow('Part not available: InfraredVision');
  });

  // --- Costing — Edge Cases ---

  it('cost with identical prices from two suppliers picks either', () => {
    const order = new RobotOrderBuilder().build();
    const alpha = allPartsSupplier('Alpha', 20);
    const beta = allPartsSupplier('Beta', 20);
    const gamma = allPartsSupplier('Gamma', 30);
    const factory = new Factory([alpha, beta, gamma]);

    const breakdown = factory.costRobot(order);

    expect(breakdown.parts.every(q =>
      q.supplierName === 'Alpha' || q.supplierName === 'Beta')).toBe(true);
    expect(breakdown.total.amount).toBe(100);
  });

  it('cost with three suppliers each cheapest for different parts', () => {
    const order = new RobotOrderBuilder().build();

    const alpha = new SupplierBuilder().named('Alpha')
      .withPart('Head', 'StandardVision', 5)
      .withPart('Body', 'Square', 50)
      .withPart('Arms', 'Hands', 50)
      .withPart('Movement', 'Wheels', 10)
      .withPart('Power', 'Solar', 50)
      .build();

    const beta = new SupplierBuilder().named('Beta')
      .withPart('Head', 'StandardVision', 50)
      .withPart('Body', 'Square', 5)
      .withPart('Arms', 'Hands', 50)
      .withPart('Movement', 'Wheels', 50)
      .withPart('Power', 'Solar', 10)
      .build();

    const gamma = new SupplierBuilder().named('Gamma')
      .withPart('Head', 'StandardVision', 50)
      .withPart('Body', 'Square', 50)
      .withPart('Arms', 'Hands', 5)
      .withPart('Movement', 'Wheels', 50)
      .withPart('Power', 'Solar', 50)
      .build();

    const factory = new Factory([alpha, beta, gamma]);

    const breakdown = factory.costRobot(order);

    expect(breakdown.parts.find(p => p.type === 'Head')!.supplierName).toBe('Alpha');
    expect(breakdown.parts.find(p => p.type === 'Body')!.supplierName).toBe('Beta');
    expect(breakdown.parts.find(p => p.type === 'Arms')!.supplierName).toBe('Gamma');
    expect(breakdown.parts.find(p => p.type === 'Movement')!.supplierName).toBe('Alpha');
    expect(breakdown.parts.find(p => p.type === 'Power')!.supplierName).toBe('Beta');
    expect(breakdown.total.amount).toBe(35);
  });

  // --- Purchasing ---

  it('purchase calls the winning suppliers purchase method for each part', () => {
    const order = new RobotOrderBuilder().build();
    const supplier = allPartsSupplier('AlphaParts', 10);
    const filler1 = allPartsSupplier('Filler1', 50);
    const filler2 = allPartsSupplier('Filler2', 50);
    const factory = new Factory([supplier, filler1, filler2]);

    factory.purchaseRobot(order);

    expect(supplier.purchaseLog).toHaveLength(5);
  });

  it('purchase returns the list of purchased parts with their suppliers', () => {
    const order = new RobotOrderBuilder().build();
    const supplier = allPartsSupplier('AlphaParts', 10);
    const filler1 = allPartsSupplier('Filler1', 50);
    const filler2 = allPartsSupplier('Filler2', 50);
    const factory = new Factory([supplier, filler1, filler2]);

    const parts = factory.purchaseRobot(order);

    expect(parts).toHaveLength(5);
    expect(parts.every(p => p.supplierName === 'AlphaParts')).toBe(true);
  });

  it('purchase is rejected when the order is incomplete', () => {
    const order = new RobotOrderBuilder().without('Arms').build();
    const supplier = allPartsSupplier('AlphaParts', 10);
    const filler1 = allPartsSupplier('Filler1', 50);
    const filler2 = allPartsSupplier('Filler2', 50);
    const factory = new Factory([supplier, filler1, filler2]);

    expect(() => factory.purchaseRobot(order)).toThrow(OrderIncompleteError);
  });

  it('purchase fails when a part is not available from any supplier', () => {
    const order = new RobotOrderBuilder()
      .withHead('NightVision')
      .build();

    const alpha = new SupplierBuilder().named('Alpha')
      .withPart('Body', 'Square', 20)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const beta = new SupplierBuilder().named('Beta')
      .withPart('Body', 'Square', 20)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const gamma = new SupplierBuilder().named('Gamma')
      .withPart('Body', 'Square', 20)
      .withPart('Arms', 'Hands', 20)
      .withPart('Movement', 'Wheels', 20)
      .withPart('Power', 'Solar', 20)
      .build();

    const factory = new Factory([alpha, beta, gamma]);

    expect(() => factory.purchaseRobot(order)).toThrow(PartNotAvailableError);
    expect(() => factory.purchaseRobot(order)).toThrow('Part not available: NightVision');
  });

  // --- Supplier Behavior ---

  it('supplier that does not carry a part returns no quote', () => {
    const supplier = new SupplierBuilder().named('Empty').build();

    expect(supplier.getQuote('Head', 'StandardVision')).toBeNull();
  });

  it('supplier returns a quote for a part it carries', () => {
    const supplier = new SupplierBuilder().named('Alpha')
      .withPart('Head', 'StandardVision', 42)
      .build();

    const quote = supplier.getQuote('Head', 'StandardVision');

    expect(quote).not.toBeNull();
    expect(quote!.price.amount).toBe(42);
    expect(quote!.supplierName).toBe('Alpha');
  });

  it('supplier purchase records the transaction', () => {
    const supplier = new SupplierBuilder().named('Alpha')
      .withPart('Head', 'StandardVision', 42)
      .build();

    supplier.purchase('Head', 'StandardVision');

    expect(supplier.purchaseLog).toHaveLength(1);
    expect(supplier.purchaseLog[0]!.type).toBe('Head');
    expect(supplier.purchaseLog[0]!.option).toBe('StandardVision');
    expect(supplier.purchaseLog[0]!.price.amount).toBe(42);
    expect(supplier.purchaseLog[0]!.supplierName).toBe('Alpha');
  });

  // --- Full Assembly ---

  it('full robot with three suppliers each cheapest for some parts costs correctly', () => {
    const order = new RobotOrderBuilder()
      .withHead('InfraredVision')
      .withBody('Round')
      .withArms('BoxingGloves')
      .withMovement('Tracks')
      .withPower('Biomass')
      .build();

    const alpha = new SupplierBuilder().named('Alpha')
      .withPart('Head', 'InfraredVision', 100)
      .withPart('Body', 'Round', 200)
      .withPart('Arms', 'BoxingGloves', 50)
      .withPart('Movement', 'Tracks', 300)
      .withPart('Power', 'Biomass', 150)
      .build();

    const beta = new SupplierBuilder().named('Beta')
      .withPart('Head', 'InfraredVision', 80)
      .withPart('Body', 'Round', 250)
      .withPart('Arms', 'BoxingGloves', 75)
      .withPart('Movement', 'Tracks', 200)
      .withPart('Power', 'Biomass', 175)
      .build();

    const gamma = new SupplierBuilder().named('Gamma')
      .withPart('Head', 'InfraredVision', 120)
      .withPart('Body', 'Round', 150)
      .withPart('Arms', 'BoxingGloves', 60)
      .withPart('Movement', 'Tracks', 350)
      .withPart('Power', 'Biomass', 100)
      .build();

    const factory = new Factory([alpha, beta, gamma]);

    const breakdown = factory.costRobot(order);

    expect(breakdown.parts.find(p => p.type === 'Head')!.supplierName).toBe('Beta');
    expect(breakdown.parts.find(p => p.type === 'Body')!.supplierName).toBe('Gamma');
    expect(breakdown.parts.find(p => p.type === 'Arms')!.supplierName).toBe('Alpha');
    expect(breakdown.parts.find(p => p.type === 'Movement')!.supplierName).toBe('Beta');
    expect(breakdown.parts.find(p => p.type === 'Power')!.supplierName).toBe('Gamma');
    expect(breakdown.total.amount).toBe(580);
  });

  it('full robot purchased from mixed suppliers each part from its cheapest source', () => {
    const order = new RobotOrderBuilder()
      .withHead('NightVision')
      .withBody('Rectangular')
      .withArms('Pinchers')
      .withMovement('Legs')
      .withPower('RechargeableBattery')
      .build();

    const alpha = new SupplierBuilder().named('Alpha')
      .withPart('Head', 'NightVision', 90)
      .withPart('Body', 'Rectangular', 110)
      .withPart('Arms', 'Pinchers', 40)
      .withPart('Movement', 'Legs', 200)
      .withPart('Power', 'RechargeableBattery', 130)
      .build();

    const beta = new SupplierBuilder().named('Beta')
      .withPart('Head', 'NightVision', 70)
      .withPart('Body', 'Rectangular', 130)
      .withPart('Arms', 'Pinchers', 55)
      .withPart('Movement', 'Legs', 160)
      .withPart('Power', 'RechargeableBattery', 140)
      .build();

    const gamma = new SupplierBuilder().named('Gamma')
      .withPart('Head', 'NightVision', 85)
      .withPart('Body', 'Rectangular', 95)
      .withPart('Arms', 'Pinchers', 60)
      .withPart('Movement', 'Legs', 180)
      .withPart('Power', 'RechargeableBattery', 120)
      .build();

    const factory = new Factory([alpha, beta, gamma]);

    const parts = factory.purchaseRobot(order);

    expect(parts.find(p => p.type === 'Head')!.supplierName).toBe('Beta');
    expect(parts.find(p => p.type === 'Body')!.supplierName).toBe('Gamma');
    expect(parts.find(p => p.type === 'Arms')!.supplierName).toBe('Alpha');
    expect(parts.find(p => p.type === 'Movement')!.supplierName).toBe('Beta');
    expect(parts.find(p => p.type === 'Power')!.supplierName).toBe('Gamma');

    expect(alpha.purchaseLog.filter(p => p.type === 'Arms')).toHaveLength(1);
    expect(beta.purchaseLog).toHaveLength(2);
    expect(gamma.purchaseLog).toHaveLength(2);
  });
});
