import type { SmsNotifier } from '../src/SmsNotifier.js';

export interface SmsMessage {
  to: string;
  message: string;
}

export class RecordingSmsNotifier implements SmsNotifier {
  readonly sent: SmsMessage[] = [];

  send(to: string, message: string): void {
    this.sent.push({ to, message });
  }
}
