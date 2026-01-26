document.addEventListener("DOMContentLoaded", () => {
  const intInputElements = document.querySelectorAll(`input[data-accept-type]`) as NodeListOf<HTMLInputElement>;
  intInputElements.forEach(element => {
    const numberTypes = ["int", "int?", "long", "long?"];
    const acceptType = element.getAttribute("data-accept-type") ?? "";
    if (numberTypes.includes(acceptType)) {
      handleNumberInputElement(element, acceptType.endsWith("?"));
    }
  });
});

function handleNumberInputElement(element: HTMLInputElement, nullable: boolean): void {
  let minValue: number | null = null;
  const minRange = element.getAttribute("data-val-range-min");
  if (minRange) {
    minValue = Number(minRange);
    if (isNaN(minValue)) {
      minValue = null;
    }
  }

  let maxValue: number | null = null;
  const maxRange = element.getAttribute("data-val-range-max");
  if (maxRange) {
    maxValue = Number(maxRange);
    if (isNaN(maxValue)) {
      maxValue = null;
    }
  }

  let previousValue = element.value;
  element.addEventListener("input", () => {
    if (!element.value.length && !nullable) {
      element.value = "0";
      return;
    }

    const intValue = Number(element.value.replaceAll(" ", ""));
    if (isNaN(intValue)) {
      element.value = previousValue;
      return;
    }

    console.log({
      minValue,
      maxValue,
      intValue
    });
    if (minValue != null && intValue < minValue) {
      element.value = formatNumber(minValue);
      previousValue = element.value;
      return;
    } else if (maxValue != null && intValue > maxValue) {
      element.value = formatNumber(maxValue);
      previousValue = element.value;
      return;
    }

    element.value = formatNumber(intValue);
    previousValue = element.value;
  });
}

function formatNumber(number: number): string {
  return number.toString();
  // return number.toLocaleString("vi-VN").replaceAll(".", " ");
}