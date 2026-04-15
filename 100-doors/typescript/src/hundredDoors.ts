export function openDoors(numDoors: number): readonly number[] {
  const isOpen = new Array<boolean>(numDoors + 1).fill(false);
  for (let pass = 1; pass <= numDoors; pass++) {
    for (let door = pass; door <= numDoors; door += pass) {
      isOpen[door] = !isOpen[door];
    }
  }

  const open: number[] = [];
  for (let door = 1; door <= numDoors; door++) {
    if (isOpen[door]) open.push(door);
  }
  return open;
}
