import { useState, useEffect } from "react";

export function useRerendingTrigger(reloadedCallback?: () => any): [number, (() => void)] {
  // States.
  const [key, setKey] = useState<number>(0);

  // Effect.
  useEffect(() => {
    if (key === 0) {
      return;
    }
    
    reloadedCallback?.();
  }, [key]);

  return [key, () => setKey(k => k += 1)];
}