document.addEventListener("DOMContentLoaded", () => {
  const togglerButtonElement = document.getElementById("navbar-toggle-button") as HTMLButtonElement | null;
  const navBarElement = document.getElementById("navbar") as HTMLElement;
  if (!togglerButtonElement || !navBarElement) {
    return
  }
  
  document.addEventListener("pointerdown", (event: PointerEvent) => {
    if (!navBarElement.classList.contains("expanded")) {
      return;
    }

    if (navBarElement === (event.target as HTMLElement)) {
      navBarElement.classList.remove("expanded");
    }
    
    if (navBarElement.classList.contains("expanded")) {
      document.documentElement.style.maxHeight = "100vh";
      document.documentElement.style.overflow = "hidden";
    } else {
      document.documentElement.removeAttribute("style");
    }
  });
  
  const mdScreenMediaQuery = window.matchMedia("(min-width: 48rem)");
  const handleMdScreenQueryMatchChanged = () => {
    if (mdScreenMediaQuery.matches) {
      navBarElement.classList.remove("expanded");
    }
  };
  handleMdScreenQueryMatchChanged();
  mdScreenMediaQuery.addEventListener("change", handleMdScreenQueryMatchChanged);
  
  togglerButtonElement.addEventListener("click", () => {
    if (navBarElement.classList.contains("expanded")) {
      navBarElement.classList.remove("expanded");
    } else {
      navBarElement.classList.add("expanded");
    }
  });
});