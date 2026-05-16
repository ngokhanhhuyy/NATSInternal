export function getAmountDisplayText(amount: number): string {
  return amount.toLocaleString("vi").replaceAll(".", " ") + " vnđ";
}
