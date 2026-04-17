export interface SmsNotifier {
  send(to: string, message: string): void;
}
