export class CommandBuilder {
  private buffer = '';

  forward(): this  { this.buffer += 'F'; return this; }
  backward(): this { this.buffer += 'B'; return this; }
  left(): this     { this.buffer += 'L'; return this; }
  right(): this    { this.buffer += 'R'; return this; }

  build(): string { return this.buffer; }
}
