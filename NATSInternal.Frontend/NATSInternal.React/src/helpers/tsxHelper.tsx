export function compute<T>(computer: () => T): T {
  return computer();
};

export function joinClassName(...classNames: (string | false | null | undefined)[]): string | undefined {
  return classNames.filter(name => name).join(" ") || undefined;
};
