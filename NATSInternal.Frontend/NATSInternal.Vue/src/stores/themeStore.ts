import { reactive, watch, toRefs, toRaw } from "vue";
import { defineStore } from "pinia";
import { usePreferredDark } from "@vueuse/core";

type Theme = "light" | "dark";
type ThemeData = { theme: Theme, auto: boolean };

export const useThemeStore = defineStore("themeStore", () => {
  // Dependencies.
  const isDarkPreferred = usePreferredDark();

  // States.
  const states = reactive<ThemeData>(((): ThemeData => {
    const savedTheme = localStorage.getItem("theme") as Theme | null;

    if (savedTheme) {
      return {
        theme: savedTheme,
        auto: false
      };
    } else {
      return {
        theme: isDarkPreferred.value ? "dark" : "light",
        auto: true
      };
    }
  })());

  // Functions.
  const saveData = () => localStorage.setItem("theme", states.theme);
  const clearData = () => localStorage.removeItem("theme");

  const applyThemeToDOM = (theme: Theme): void => {
    if (theme === "light") {
      document.documentElement.classList.remove("dark");
      if (document.documentElement.classList.length === 0) {
        document.documentElement.removeAttribute("class");
      }

      return;
    }

    document.documentElement.classList.add("dark");
  };

  const getNextToggleTheme = (): Theme | "auto" => {
    if (states.auto) {
      return "light";
    }

    if (states.theme === "light") {
      return "dark";
    }

    return "auto";
  };

  const toggle = (): void => {
    console.log("beforeClicked", toRaw(states));
    const nextToggleTheme = getNextToggleTheme();
    if (nextToggleTheme === "auto") {
      states.auto = true;
      return;
    }

    states.theme = nextToggleTheme;
    states.auto = false;
    console.log("afterClicked", toRaw(states));
  };

  const toggleTheme = (): void => {
    states.theme = states.theme === "light" ? "dark" : "light";
  };

  const toggleAuto = (): void => {
    states.auto = !states.auto;
  };

  const setTheme = (theme: Theme): void => {
    states.theme = theme;
    states.auto = false;
  };

  const setAuto = (auto: boolean): void => {
    states.auto = auto;
  };

  // Watch.
  watch([isDarkPreferred, states], () => {
    if (states.auto) {
      const preferTheme = isDarkPreferred.value ? "dark" : "light";
      states.theme = preferTheme;
      applyThemeToDOM(preferTheme);
      clearData();
      return;
    }

    applyThemeToDOM(states.theme);
    saveData();
  }, { immediate: true });

  return {
    ...toRefs(states),
    getNextToggleTheme,
    toggle,
    toggleTheme,
    toggleAuto,
    setTheme,
    setAuto
  };
});