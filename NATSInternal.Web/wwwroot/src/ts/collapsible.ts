document.addEventListener("DOMContentLoaded", () => {
  const collapsibleElements = document.querySelectorAll(collapsibleTogglerClassName);
  collapsibleElements.forEach(element => {
    if (!(element instanceof HTMLButtonElement)) {
      return;
    }
    
    const buttonElement = element as HTMLButtonElement;
    const targetId = element.getAttribute("target-id");
    if (!targetId) {
      return;
    }

    buttonElement.addEventListener("click", () => {
      const targetElement = document.querySelector(`#${targetId}.${collapsibleClassName}`);
      if (!targetElement) {
        return;
      }
      
      if (targetElement.classList.contains(collapsibleExpandedClassName)) {
        targetElement.classList.remove(collapsibleExpandedClassName)
      } else {
        targetElement.classList.add(collapsibleExpandedClassName)
      }
    });
  });
});

const collapsibleTogglerClassName = ".collapsible-toggler";
const collapsibleClassName = "collapsible";
const collapsibleExpandedClassName = "collapsible-expanded";