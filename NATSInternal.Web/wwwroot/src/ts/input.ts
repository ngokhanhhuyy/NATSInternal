const regexpPatternAttributeName = "data-regexp-pattern";

document.addEventListener("DOMContentLoaded", () => {
  const regexpPatternInputElements = document
    .querySelectorAll(`input[${regexpPatternAttributeName}]`) as NodeListOf<HTMLInputElement>;
  regexpPatternInputElements.forEach(element => {
    const regexpPatternAsString = element.getAttribute(regexpPatternAttributeName);
    if (!regexpPatternAsString) {
      return;
    }
    
    const regexpPattern = new RegExp(regexpPatternAsString);
    element.addEventListener("input", (event: Event) => {
      const value = (event.target as HTMLInputElement).value;
      if (!regexpPattern.test("value")) {
        
      }
    });
  });
});