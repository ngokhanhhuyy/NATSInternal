import React, { useState, useEffect } from "react";
import { useNavigationBarStore } from "@/stores";
import { useTsxHelper } from "@/helpers";

// Child component.
import MainLogo from "./MainLogo";
import { Bars4Icon } from "@heroicons/react/24/solid";

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
        "bg-white/80 border-b border-primary/15 w-full h-(--topbar-height)",
        "flex justify-stretch items-stretch fixed top-0 z-999 backdrop-blur-sm transition-colors",
        isScrolled && "shadow-lg",
      )}
    >
      <div id="topbar-container" className="max-w-384 w-full flex justify-between items-stretch gap-3 p-3 ps-4 md:ps-5">
        <MainLogo />
        <button
          className={joinClassName(
            "border px-3 rounded-lg md:hidden",
            !isScrolled ? "border-primary/15" : "border-secondary/15"
          )}
          onClick={navigationBarStore.toggle}
        >
          <Bars4Icon className="size-6" />
        </button>
      </div>
    </div>
  );
}