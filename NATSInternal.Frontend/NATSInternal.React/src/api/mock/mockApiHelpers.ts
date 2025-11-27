export function toAsyncMicrotask<T extends any[], R>(fn: (...args: T) => R) {
  return (...args: T) =>
    new Promise<R>((resolve, reject) => {
      queueMicrotask(() => {
        try { resolve(fn(...args)); }
        catch (e) { reject(e); }
      });
    });
}