import React, { useRef, useEffect } from "react";

export function useWatcher(callback: React.EffectCallback, dependencies: React.DependencyList): void {
  // States.
  const isInitialRendering = useRef<boolean>(true);

  // Effect.
  useEffect(() => {
    if (isInitialRendering.current) {
      isInitialRendering.current = false;
      return;
    }

    return callback();
  }, dependencies);
}