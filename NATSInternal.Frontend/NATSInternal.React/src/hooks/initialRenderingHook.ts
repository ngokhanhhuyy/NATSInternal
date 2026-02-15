import { useState, useEffect } from "react";

export function useInitialRendering(): boolean {
  // States.
  const [isInitialRendering, setIsInitialRendering] = useState<boolean>(true);

  // Effect.
  useEffect(() => {
    if (isInitialRendering) {
      setIsInitialRendering(false);
    }
  }, []);

  return isInitialRendering;
}