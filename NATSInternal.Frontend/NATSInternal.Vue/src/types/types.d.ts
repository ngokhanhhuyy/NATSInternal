declare global {
  type Implements<T, U extends T> = U;
  type ImplementsPartial<T extends IPaginatedList, U extends T> = { [P in keyof U]?: U[P] | undefined; };
  type AwaitedReturn<T> = T extends Promise<infer U> ? U : T;
  export type ExtractComponentEmit<C> = [EmitFnFromInstance<C>] extends [never]
    ? EmitFnFromSetupCtx<C>
    : EmitFnFromInstance<C>;
}

// Emit types.
type EmitFnFromInstance<C> =
  C extends abstract new (...args: any) => { $emit: infer E }
    ? (E extends AnyFn ? E : never)
    : never;

type EmitFnFromSetupCtx<C> =
  C extends (...args: any[]) => any
    ? NonNullable<Parameters<C>[1]> extends { emit: infer E }
      ? (E extends AnyFn ? E : never)
      : never
    : never;
export { };