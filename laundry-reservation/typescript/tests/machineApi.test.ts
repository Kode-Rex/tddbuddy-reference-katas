import { MachineApi } from '../src/MachineApi.js';
import { RecordingMachineDevice } from './RecordingMachineDevice.js';

const slot = new Date(Date.UTC(2026, 3, 15, 10, 0, 0));

describe('Machine API', () => {
  it('Locking a machine delegates to the device with reservation ID, date/time, and PIN', () => {
    const device = new RecordingMachineDevice();
    const api = new MachineApi();
    api.registerDevice(7, device);

    api.lock('res-1', 7, slot, 12345);

    expect(device.lockCalls).toHaveLength(1);
    const call = device.lockCalls[0]!;
    expect(call.reservationId).toBe('res-1');
    expect(call.reservationDateTime).toEqual(slot);
    expect(call.pin).toBe(12345);
  });

  it('Locking a machine returns true when the device accepts the lock', () => {
    const device = new RecordingMachineDevice();
    device.shouldAcceptLock = true;
    const api = new MachineApi();
    api.registerDevice(7, device);

    const result = api.lock('res-1', 7, slot, 12345);

    expect(result).toBe(true);
  });

  it('Locking a machine returns false when the device rejects the lock', () => {
    const device = new RecordingMachineDevice();
    device.shouldAcceptLock = false;
    const api = new MachineApi();
    api.registerDevice(7, device);

    const result = api.lock('res-1', 7, slot, 12345);

    expect(result).toBe(false);
  });

  it('Locking a machine with an existing reservation ID updates the PIN and date/time', () => {
    const device = new RecordingMachineDevice();
    const api = new MachineApi();
    api.registerDevice(7, device);
    api.lock('res-1', 7, slot, 12345);

    const newSlot = new Date(slot.getTime() + 3600000);
    const result = api.lock('res-1', 7, newSlot, 67890);

    expect(result).toBe(true);
    expect(device.lockCalls).toHaveLength(2);
    const updateCall = device.lockCalls[1]!;
    expect(updateCall.reservationId).toBe('res-1');
    expect(updateCall.reservationDateTime).toEqual(newSlot);
    expect(updateCall.pin).toBe(67890);
  });

  it('Unlocking a machine delegates to the device with the reservation ID', () => {
    const device = new RecordingMachineDevice();
    const api = new MachineApi();
    api.registerDevice(7, device);
    api.lock('res-1', 7, slot, 12345);

    api.unlock(7, 'res-1');

    expect(device.unlockCalls).toHaveLength(1);
    expect(device.unlockCalls[0]).toBe('res-1');
  });
});
