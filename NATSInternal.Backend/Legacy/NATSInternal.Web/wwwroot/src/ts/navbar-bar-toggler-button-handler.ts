document.addEventListener("DOMContentLoaded", () => {
  const togglerButtonElement = document.getElementById("navbar-toggle-button") as HTMLButtonElement | null;
  const navBarElement = document.getElementById("navbar") as HTMLElement;
  if (!togglerButtonElement || !navBarElement) {
    return
  }
  
  const expand = (): void => {
    navBarElement.classList.add("expanded");
    document.documentElement.style.maxHeight = "100vh";
    document.documentElement.style.overflow = "hidden";
  };
  
  const collapse = (): void => {
    navBarElement.classList.remove("expanded");
    document.documentElement.removeAttribute("style");
  };
  
  document.addEventListener("pointerdown", (event: PointerEvent) => {
    if (!navBarElement.classList.contains("expanded")) {
      return;
    }

    if (navBarElement === (event.target as HTMLElement)) {
      collapse();
    }
  });
  
  const mdScreenMediaQuery = window.matchMedia("(min-width: 48rem)");
  const handleMdScreenQueryMatchChanged = () => {
    if (mdScreenMediaQuery.matches) {
      collapse();
    }
  };
  
  handleMdScreenQueryMatchChanged();
  mdScreenMediaQuery.addEventListener("change", handleMdScreenQueryMatchChanged);
  
  togglerButtonElement.addEventListener("click", () => {
    if (navBarElement.classList.contains("expanded")) {
      collapse();
    } else {
      expand();
    }
  });
});