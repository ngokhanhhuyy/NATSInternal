import { create } from "zustand";

type Theme = "light" | "dark";
export type ThemeStore = {
  theme: Theme,
  auto: boolean,
  getNextToggleTheme(): Theme | "auto";
  toggle(): void;
  toggleTheme(): void;
  toggleAuto(): void;
  setTheme(theme: Theme): void;
  setAuto(auto: boolean): void;
};

export const useThemeStore = create<ThemeStore>((set, get) => {
  type ThemeData = { theme: Theme, auto: boolean };
  const mediaQuery = window.matchMedia("(prefers-color-scheme: dark)");
  const themeData: ThemeData = { theme: "light", auto: true };
  const saveData = () => localStorage.setItem("themeData", JSON.stringify(themeData));
  const themeDataJson = localStorage.getItem("themeData");
  if (themeDataJson) {
    const parsedData = JSON.parse(themeDataJson) as ThemeData;
    themeData.theme = parsedData.theme;
    themeData.auto = parsedData.auto;
  }

  const handleMediaQueryMatchChanged = () => {
    if (!get().auto) {
      return;
    }

    applyThemeToDOM(mediaQuery.matches ? "dark" : "light");
  };

  mediaQuery.addEventListener("change", handleMediaQueryMatchChanged);

  return {
    theme: themeData.theme,
    auto: themeData.auto,
    getNextToggleTheme(): Theme | "auto" {
      if (get().auto) {
        return "light";
      }

      if (get().theme === "light") {
        return "dark";
      }

      return "auto";
    },
    toggle(): void {
      const nextToggleTheme = get().getNextToggleTheme();
      if (nextToggleTheme === "auto") {
        get().setAuto(true);
        return;
      }

      get().setTheme(nextToggleTheme);
    },
    toggleTheme(): void {
      get().setTheme(get().theme === "light" ? "dark" : "light");
    },
    toggleAuto(): void {
      get().setAuto(!get().auto);
    },
    setTheme(theme: Theme): void {
      applyThemeToDOM(theme);
      set({ theme, auto: false });
      saveData();
    },
    setAuto(auto: boolean): void {
      const preferTheme = mediaQuery.matches ? "dark" : "light";
      if (auto) {
        applyThemeToDOM(preferTheme);
      }

      set({
        theme: auto ? preferTheme : get().theme,
        auto
      });

      saveData();
    }
  };
});

function applyThemeToDOM(theme: Theme): void {
  if (theme === "light") {
    document.documentElement.classList.remove("dark");
    if (document.documentElement.classList.length === 0) {
      document.documentElement.removeAttribute("class");
    }

    return;
  }

  document.documentElement.classList.add("dark");
}