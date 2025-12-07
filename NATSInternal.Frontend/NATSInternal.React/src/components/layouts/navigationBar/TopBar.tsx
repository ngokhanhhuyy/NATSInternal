import React, { useState, useEffect } from "react";
import { useNavigationBarStore, useThemeStore } from "@/stores";
import { useTsxHelper } from "@/helpers";

// Child component.
import MainLogo from "./MainLogo";
import { Button } from "@/components/ui";
import { Bars4Icon, MoonIcon, SunIcon, Cog6ToothIcon } from "@heroicons/react/24/solid";

// Component.
export default function TopBar(): React.ReactNode {
  // Dependencies.
  const navigationBarStore = useNavigationBarStore(); 
  const { joinClassName } = useTsxHelper();

  // States.
  const [isScrolled, setIsScrolled] = useState<boolean>(false);

  // Effects.
  useEffect(() => {
    const handleScrollY = () => {
      setIsScrolled(window.scrollY != 0);
    };

    window.addEventListener("scroll", handleScrollY);

    return () => {
      window.removeEventListener("scroll", handleScrollY);
    };
  }, []);

  // Template.
  return (
    <div
      id="topbar"
      className={joinClassName(
        "bg-white/75 dark:bg-neutral-800/70 border-b border-primary/15 w-full h-(--topbar-height)",
        "flex justify-stretch items-stretch fixed top-0 z-999",
        "backdrop-blur-sm dark:backdrop-blur-md transition-colors",
        isScrolled && "shadow-xs",
      )}
    >
      <div
        id="topbar-container"
        className="max-w-384 w-full grid grid-cols-[1fr_auto_auto] items-stretch gap-3 mx-auto p-3 ps-4 md:ps-5"
      >
        {/* Main logo */}
        <MainLogo />

        {/* Theme toggle button */}
        <ThemeToggleButton />

        {/* Navigation bar toggle button */}
        <Button className="h-full aspect-[1.2] md:hidden shrink-0" onClick={navigationBarStore.toggle}>
          <Bars4Icon className="size-6" />
        </Button>
      </div>
    </div>
  );
}

function ThemeToggleButton(): React.ReactNode {
  // Dependencies.
  const themeStore = useThemeStore();

  // Template.
  let icon: React.ReactNode;
  if (themeStore.auto) {
    icon = <Cog6ToothIcon className="size-4.5" />;
  } else if (themeStore.theme === "light") {
    icon = <SunIcon className="size-5" />;
  } else {
    icon = <MoonIcon className="size-4" />;
  }

  return (
    <Button type="button" className="h-full aspect-[1.2]" onClick={themeStore.toggle}>
      {icon}
    </Button>
  );
}