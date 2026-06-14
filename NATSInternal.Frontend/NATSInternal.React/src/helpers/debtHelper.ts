type TextClassNameBasedOnDebtAmountOptions = {
  noDebtClassName: string | null;
};

export function getTextClassNameBasedOnDebtAmount(
  debtAmount: number,
  options?: TextClassNameBasedOnDebtAmountOptions): string | undefined
{
  const names: string[] = [];
  if (debtAmount === 0) {
    if (!options) {
      names.push("text-blue-700 dark:text-blue-400");
    } else if (options.noDebtClassName !== null) {
      names.push(options.noDebtClassName);
    }
  } else if (debtAmount > 0) {;
    names.push("text-yellow-600 dark:text-yellow-400");
  } else {
    names.push("text-red-600 dark:text-red-400");
  }

  if (names.length) {
    return names.join(" ");
  }
}
