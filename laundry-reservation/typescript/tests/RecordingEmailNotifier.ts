import type { EmailNotifier } from '../src/EmailNotifier.js';

export interface EmailMessage {
  to: string;
  subject: string;
  body: string;
}

export class RecordingEmailNotifier implements EmailNotifier {
  readonly sent: EmailMessage[] = [];

  send(to: string, subject: string, body: string): void {
    this.sent.push({ to, subject, body });
  }
}
