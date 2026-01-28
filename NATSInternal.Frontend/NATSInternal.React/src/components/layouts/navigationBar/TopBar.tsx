import React, { useState, useEffect } from "react";
import { useNavigationBarStore, useThemeStore } from "@/stores";
import { useTsxHelper } from "@/helpers";

// Child component.
import MainLogo from "./MainLogo";
import { Button } from "@/components/ui";
import { Bars4Icon, MoonIcon, SunIcon, Cog6ToothIcon } from "@heroicons/react/24/solid";

// Props.
type TopBarProps = {
  shouldRenderNavigationBarToggleButton: boolean;
};

// Component.
export default function TopBar(props: TopBarProps): React.ReactNode {
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
    <div id="topbar" className={joinClassName(isScrolled && "shadow-xs")}>
      <div id="topbar-container">
        {/* Main logo */}
        <MainLogo />

        <div />

        {/* Theme toggle button */}
        <ThemeToggleButton />

        {/* Navigation bar toggle button */}
        {props.shouldRenderNavigationBarToggleButton && (
          <Button className="h-full aspect-[1.2] md:hidden shrink-0" onClick={navigationBarStore.toggle}>
            <Bars4Icon className="size-6" />
          </Button>
        )}
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