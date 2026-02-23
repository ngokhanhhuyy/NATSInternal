import React, { useRef, useEffect } from "react";

export function useWatcher(callback: React.EffectCallback, dependencies: React.DependencyList): void {
  // States.
  const isInitialRendering = useRef<boolean>(true);

  // Effect.
  useEffect(() => {
    console.log(isInitialRendering.current);
    if (isInitialRendering.current) {
      isInitialRendering.current = false;
      return;
    }

    callback();
  }, dependencies);
}