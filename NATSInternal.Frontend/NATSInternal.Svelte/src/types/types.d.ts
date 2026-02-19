declare global {
  type Implements<T, U extends T> = U;
  type ImplementsPartial<T extends IPaginatedList, U extends T> = { [P in keyof U]?: U[P] | undefined; };
  type AwaitedReturn<T> = T extends Promise<infer U> ? U : T;
}

export { };