import React, { useState, useEffect } from "react";
import { useNavigationBarStore } from "@/stores";
import { useTsxHelper } from "@/helpers";
import styles from "./TopBar.module.css";

// Child component.
import MainLogo from "./MainLogo";
import { Button } from "@/components/ui";
import { Bars4Icon } from "@heroicons/react/24/solid";

// Component.
export default function TopBar(): React.ReactNode {
  // Dependencies.
  const navigationBarStore = useNavigationBarStore(); 
  const { joinClassName } = useTsxHelper();

  // States.
  const [hasShadow, setHasShadow] = useState<boolean>(false);

  // Effects.
  useEffect(() => {
    const handleScrollY = () => {
      setHasShadow(window.scrollY != 0);
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
        "bg-white/80 backdrop-blur-xs border-b border-primary/15 p-3 ps-4 md:hidden w-full h-(--topbar-height)",
        "flex justify-between items-stretch gap-3 fixed top-0 z-999",
        hasShadow && "shadow-lg",
        styles.topBar
      )}
    >
      <MainLogo />
      <Button className="px-3" onClick={navigationBarStore.toggle}>
        <Bars4Icon className="size-6" />
      </Button>
    </div>
  );
}