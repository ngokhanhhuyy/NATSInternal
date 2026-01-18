document.addEventListener("DOMContentLoaded", () => {
  const introducerPanelElement = document.getElementById("customer-upsert-introducer-panel") as HTMLDivElement | null;
  if (!introducerPanelElement) {
    return;
  }

  const shouldFocus = introducerPanelElement.hasAttribute("data-should-focus");
  if (shouldFocus) {
    introducerPanelElement.scrollIntoView();
  }
});