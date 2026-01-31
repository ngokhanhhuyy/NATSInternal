import React, { useState, useRef, useEffect } from "react";

export function useThrottleState<T extends Exclude<any, undefined>>(
    initialState: T | (() => T),
    interval: number = 300): [T, T, React.Dispatch<React.SetStateAction<T>>] {
  // States.
  const [state, setState] = useState(initialState);
  const [throttledState, setThrottledState] = useState(() => state);
  const processingState = useRef<T | undefined>(undefined);
  const pendingState = useRef<T | undefined>(undefined);
  const isInitialRendering = useRef(true);

  // Effect.
  useEffect(() => {
    const process = (stateToProcess: T) => {
      if (processingState.current != null) {
        pendingState.current = stateToProcess;
        return;
      }

      processingState.current = stateToProcess;
      setThrottledState(stateToProcess);
      setTimeout(() => {
        processingState.current = undefined;
        if (pendingState.current != null) {
          process(pendingState.current);
          pendingState.current = undefined;
        }
      }, interval);
    };

    if (isInitialRendering.current) {
      isInitialRendering.current = false;
      return;
    }

    process(state);
  }, [state]);

  return [state, throttledState, setState];
}