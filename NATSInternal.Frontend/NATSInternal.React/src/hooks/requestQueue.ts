import { useRef } from "react";

export function useRequestHandlerQueue<T>(
  request: () => Promise<T>,
  handler: (response: T) => any): (() => Promise<void>)
{
  // States.
  const latestRequestId = useRef<string | null>(null);

  // Results.
  return async () => {
    const requestId = crypto.randomUUID();
    latestRequestId.current = requestId;
    const response = await request();
    if (latestRequestId.current !== requestId) {
      return;
    }

    handler(response);
  };
}
