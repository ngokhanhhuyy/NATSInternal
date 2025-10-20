type TsxHelper = {
  compute<T>(computer: () => T): T;
  joinClassName(...classNames: (string | undefined | null)[]): string | undefined;
};

const helper: TsxHelper = {
  compute<T>(computer: () => T): T {
    return computer();
  },
  joinClassName: (...classNames) => {
    return classNames.filter(name => name).join(" ") || undefined;
  }
};

export function useTsxHelper(): TsxHelper {
  return helper;
}