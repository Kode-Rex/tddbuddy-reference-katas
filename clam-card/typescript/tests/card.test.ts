import { describe, it, expect } from 'vitest';
import { UnknownStationError } from '../src/card.js';
import { CardBuilder, RideBuilder } from './cardBuilder.js';

const aMichaelCard = () =>
  new CardBuilder()
    .onDay(new Date('2024-01-01T00:00:00Z'))
    .withZone('A', 'Asterisk', 'Amersham', 'Aldgate', 'Angel', 'Anerley', 'Antelope')
    .withZone('B', 'Bison', 'Bugel', 'Balham', 'Bullhead', 'Barbican');

describe('Card', () => {
  it('one-way Zone A journey is charged the Zone A single fare', () => {
    const card = aMichaelCard().build();

    const ride = card.travelFrom('Asterisk').to('Aldgate');

    expect(ride.fare).toBe(2.5);
    expect(ride.zone).toBe('A');
  });

  it('one-way Zone A-to-B journey is charged the Zone B single fare', () => {
    const card = aMichaelCard().build();

    const ride = card.travelFrom('Asterisk').to('Barbican');

    expect(ride.fare).toBe(3.0);
    expect(ride.zone).toBe('B');
  });

  it('one-way Zone B-to-A journey is charged the Zone B single fare', () => {
    const card = aMichaelCard().build();

    const ride = card.travelFrom('Bison').to('Asterisk');

    expect(ride.fare).toBe(3.0);
    expect(ride.zone).toBe('B');
  });

  it('one-way Zone B journey is charged the Zone B single fare', () => {
    const card = aMichaelCard().build();

    const ride = card.travelFrom('Bison').to('Barbican');

    expect(ride.fare).toBe(3.0);
    expect(ride.zone).toBe('B');
  });

  it('two single journeys accumulate on totalCharged', () => {
    const card = aMichaelCard().build();

    card.travelFrom('Asterisk').to('Aldgate');
    card.travelFrom('Asterisk').to('Balham');

    expect(card.totalCharged()).toBe(5.5);
    expect(card.rides()).toHaveLength(2);
    expect(card.rides()[0]!.fare).toBe(2.5);
    expect(card.rides()[1]!.fare).toBe(3.0);
  });

  it('Zone A daily cap is $7.00', () => {
    const card = aMichaelCard().build();

    const r1 = card.travelFrom('Asterisk').to('Aldgate');
    const r2 = card.travelFrom('Aldgate').to('Angel');
    const r3 = card.travelFrom('Angel').to('Antelope');
    const r4 = card.travelFrom('Antelope').to('Asterisk');

    expect(r1.fare).toBe(2.5);
    expect(r2.fare).toBe(2.5);
    expect(r3.fare).toBe(2.0);
    expect(r4.fare).toBe(0.0);
    expect(card.totalCharged()).toBe(7.0);
  });

  it('Zone B daily cap is $8.00', () => {
    const card = aMichaelCard().build();

    const r1 = card.travelFrom('Asterisk').to('Barbican');
    const r2 = card.travelFrom('Barbican').to('Balham');
    const r3 = card.travelFrom('Balham').to('Bison');
    const r4 = card.travelFrom('Bison').to('Asterisk');

    expect(r1.fare).toBe(3.0);
    expect(r2.fare).toBe(3.0);
    expect(r3.fare).toBe(2.0);
    expect(r4.fare).toBe(0.0);
    expect(card.totalCharged()).toBe(8.0);
  });

  it('reaching the Zone A cap does not affect Zone B fares', () => {
    const card = aMichaelCard().build();

    card.travelFrom('Asterisk').to('Aldgate');
    card.travelFrom('Aldgate').to('Angel');
    card.travelFrom('Angel').to('Antelope');
    card.travelFrom('Antelope').to('Asterisk');

    const nextB = card.travelFrom('Asterisk').to('Barbican');

    expect(nextB.fare).toBe(3.0);
  });

  it('reaching the Zone B cap does not affect Zone A fares', () => {
    const card = aMichaelCard().build();

    card.travelFrom('Asterisk').to('Barbican');
    card.travelFrom('Barbican').to('Balham');
    card.travelFrom('Balham').to('Bison');
    card.travelFrom('Bison').to('Asterisk');

    const nextA = card.travelFrom('Asterisk').to('Aldgate');

    expect(nextA.fare).toBe(2.5);
  });

  it('travelling from an unknown station raises', () => {
    const card = aMichaelCard().build();

    expect(() => card.travelFrom('Moonbase')).toThrow(UnknownStationError);
    expect(() => card.travelFrom('Moonbase')).toThrow(
      "station is not on this card's network",
    );
  });

  it('travelling to an unknown station raises', () => {
    const card = aMichaelCard().build();

    expect(() => card.travelFrom('Asterisk').to('Moonbase')).toThrow(UnknownStationError);
    expect(() => card.travelFrom('Asterisk').to('Moonbase')).toThrow(
      "station is not on this card's network",
    );
  });

  it('each ride records its zone and fare', () => {
    const card = aMichaelCard().build();

    card.travelFrom('Asterisk').to('Barbican');
    card.travelFrom('Asterisk').to('Aldgate');

    const expectedB = new RideBuilder()
      .from('Asterisk').to('Barbican').chargedAt('B').withFare(3.0).build();
    const expectedA = new RideBuilder()
      .from('Asterisk').to('Aldgate').chargedAt('A').withFare(2.5).build();

    expect(card.rides()).toEqual([expectedB, expectedA]);
  });
});
