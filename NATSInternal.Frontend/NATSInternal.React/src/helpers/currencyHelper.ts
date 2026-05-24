export function getDisplayAmountText(amount: number, options?: { excludeSuffix: boolean }): string {
  const formattedAmount = amount.toLocaleString("vi").replaceAll(".", " ");
  if (options?.excludeSuffix) {
    return formattedAmount;
  }

  return `${formattedAmount} vnđ`;
}
