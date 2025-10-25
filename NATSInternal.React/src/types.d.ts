declare module '*.module.scss' {
  const styles: { [className: string]: string };
  export default styles;
}

declare global {
  type AwaitedReturn<T> = T extends Promise<infer U> ? U : T;
  type SyncApi<T> = {
    [K in keyof T]:
      T[K] extends (...args: infer A) => infer R
        ? (...args: A) => AwaitedReturn<R>
        : T[K];
  };
  type ColorVariant = "indigo" | "red" | "success" | "blue" | "black";
}

export { };