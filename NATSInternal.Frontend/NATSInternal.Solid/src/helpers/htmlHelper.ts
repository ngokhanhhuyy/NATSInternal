type HTMLHelper = {
  joinClassName(...classNames: (string | undefined | null)[]): string | undefined;
}

const helper: HTMLHelper = {
  joinClassName: (...classNames) => {
    return classNames.filter(name => name).join(" ") || undefined;
  }
};

export function useHTMLHelper(): HTMLHelper {
  return helper;
}