export interface EmailNotifier {
  send(to: string, subject: string, body: string): void;
}
