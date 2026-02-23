import { useState, useRef, useEffect } from "react";

export type ScreenBreakpoints = {
  md: boolean;
};

export function useScreenBreakpoints(): ScreenBreakpoints {
  // States.
  const mdScreenMediaQuery = useRef(window.matchMedia("(min-width: 48rem)"));
  const [screenBreakpoints, setScreenBreakpoints] = useState<ScreenBreakpoints>(() => ({
    md: mdScreenMediaQuery.current.matches
  }));

  // Effects.
  useEffect(() => {
    const handleScreenMediaQueryChanged = () => {
      if (mdScreenMediaQuery.current.matches) {
        setScreenBreakpoints(breakpoints => ({
          ...breakpoints,
          md: true
        }));
      }
    };

    mdScreenMediaQuery.current.addEventListener("change", handleScreenMediaQueryChanged);
    return () => {
      mdScreenMediaQuery.current.removeEventListener("change", handleScreenMediaQueryChanged);
    };
  }, []);

  return screenBreakpoints;
}