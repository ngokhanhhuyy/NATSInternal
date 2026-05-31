type DisplayAmountTextOptions = {
  suffix?: string;
};

export function getDisplayAmountText(amount: number, options?: DisplayAmountTextOptions): string {
  const formattedAmount = amount.toLocaleString("vi").replaceAll(".", " ");
  if (options?.suffix === undefined) {
    return formattedAmount;
  }

  return formattedAmount + options.suffix;
}
