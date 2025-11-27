type CurrencyHelper = {
  getAmountDisplayText(amount: number): string;
};

const currencyHelper: CurrencyHelper = {
  getAmountDisplayText(amount: number): string {
    return amount.toLocaleString("vi").replaceAll(".", " ") + " vnđ";
  }
};

/**
 * A helper to convert a number as currency amount into a formated string for displaying.
 *
 * @returns An object containing the method for converting.
 */
export function useCurrencyHelper(): CurrencyHelper {
  return currencyHelper;
}