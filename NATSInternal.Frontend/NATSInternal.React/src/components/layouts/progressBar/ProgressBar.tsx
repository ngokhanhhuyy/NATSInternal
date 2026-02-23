import React, { useState, useRef, useEffect } from "react";
import { useNavigation } from "react-router";
import { useTsxHelper } from "@/helpers";
import styles from "./ProgressBar.module.css";

type State = "idle" | "loading" | "hiding";

export default function ProgressBar(): React.ReactNode {
  // Dependencies.
  const navigation = useNavigation();
  const { joinClassName } = useTsxHelper();

  // States.
  const [state, setState] = useState<State>("loading");
  const previousState = useRef<State | null>(null);
  const elementRef = useRef<HTMLDivElement | null>(null);

  // Callbacks.
  const handleTransitionEnded = (event: React.TransitionEvent) => {
    if (event.propertyName === "opacity" && state === "hiding") {
      setState("idle");
    }
  };

  // Effect.
  useEffect(() => {
    switch (navigation.state) {
      case "loading":
        if (elementRef.current) {
          elementRef.current.style.transition = "none";
          void elementRef.current.offsetWidth;
          elementRef.current.style.removeProperty("transition");
        }

        setState("loading");
        break;
      case "idle":
        setState("hiding");
        break;
    }

    previousState.current = state;
  }, [navigation.state]);

  // Template.
  return (
    <>
      <div
        id="progress-bar"
        className={joinClassName(styles.progressBar, styles[state])}
        ref={elementRef}
        onTransitionEnd={handleTransitionEnded}
      >
        {state}
      </div>

      <div className={styles.progressBar} style={{ width: "100%" }}></div>
    </>
  );
}