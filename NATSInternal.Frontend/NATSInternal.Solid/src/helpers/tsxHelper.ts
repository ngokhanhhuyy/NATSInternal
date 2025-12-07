export type TsxHelper = {
  compute<T>(computer: () => T): T;
  joinClass(...classNames: (string | undefined | null | false)[]): string | undefined;
};

const helper: TsxHelper = {
  compute<T>(computer: () => T): T {
    return computer();
  },
  joinClass: (...classNames) => {
    return classNames.filter(name => name).join(" ") || undefined;
  }
};

export function useTsxHelper(): TsxHelper {
  return helper;
}