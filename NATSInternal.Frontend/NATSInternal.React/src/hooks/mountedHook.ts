import { useState, useEffect, type EffectCallback } from "react";

export function useMounted(callback: EffectCallback): boolean {
  // States.
  const [isMounted, setIsMounted] = useState<boolean>(false);

  // Effect.
  useEffect(() => {
    if (isMounted) {
      callback();
    }

    setIsMounted(true);
  }, [isMounted]);

  return isMounted;
}