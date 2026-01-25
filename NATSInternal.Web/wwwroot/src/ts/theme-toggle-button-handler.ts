type Theme = "light" | "dark";

class ThemeHandler {
  public theme: Theme;
  public auto: boolean;
  private static readonly localStorageKey = "theme";
  private static mediaQuery = window.matchMedia("(prefers-color-scheme: dark)");
  
  public constructor() {
    const savedTheme = localStorage.getItem(ThemeHandler.localStorageKey) as Theme | null;
    if (savedTheme) {
      this.theme = savedTheme;
      this.auto = false;
    } else {
      this.theme = ThemeHandler.mediaQuery.matches ? "dark" : "light";
      this.auto = true;
    }
    
    this.applyThemeToDOM(this.theme);
  }
  
  public get nextToggleTheme(): Theme | "auto" {
    if (this.auto) {
      return "light";
    }

    if (this.theme === "light") {
      return "dark";
    }

    return "auto";
  }
  
  public toggle(): void {
    const nextToggleTheme = this.nextToggleTheme;
    if (nextToggleTheme === "auto") {
      this.setAuto(true);
      return;
    }

    this.setTheme(nextToggleTheme);
  }
  
  private setTheme(theme: Theme): void {
    this.theme = theme;
    this.auto = false;
    this.applyThemeToDOM(this.theme);
    this.saveToLocalStorage();
  }
  
  private setAuto(auto: boolean): void {
    const preferTheme = ThemeHandler.mediaQuery.matches ? "dark" : "light";
    if (auto) {
      this.applyThemeToDOM(preferTheme);
    }

    this.theme = auto ? preferTheme : this.theme;
    this.auto = auto;
    this.clearFromLocalStorage();
  }

  private applyThemeToDOM(theme: Theme): void {
    if (theme === "light") {
      document.documentElement.classList.remove("dark");
      if (document.documentElement.classList.length === 0) {
        document.documentElement.removeAttribute("class");
      }

      return;
    }

    document.documentElement.classList.add("dark");
  }
  
  private saveToLocalStorage(): void {
    localStorage.setItem(ThemeHandler.localStorageKey, this.theme);
  }
  
  private clearFromLocalStorage(): void {
    localStorage.removeItem(ThemeHandler.localStorageKey);
  }
}

const themeHandler = new ThemeHandler();
document.addEventListener("DOMContentLoaded", () => {
  const buttonElement = document.getElementById("theme-toggle-button");
  if (!buttonElement) {
    return;
  }
  
  const iconElement = buttonElement.querySelector("i");
  const changeIconElementClassName = () => {
    if (!iconElement) {
      return;
    }

    if (themeHandler.auto) {
      iconElement.className = "bi bi-gear";
    } else if (themeHandler.theme === "light") {
      iconElement.className = "bi bi-brightness-high";
    } else if (themeHandler.theme === "dark") {
      iconElement.className = "bi bi-moon";
    }
  }
  
  changeIconElementClassName();
  
  const handleButtonClicked = () => {
    themeHandler.toggle();
    changeIconElementClassName();
  }
  
  buttonElement.addEventListener("click", handleButtonClicked);
});