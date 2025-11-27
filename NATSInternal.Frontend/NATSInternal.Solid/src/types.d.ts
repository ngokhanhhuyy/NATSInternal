declare global {
  type MethodsOmitted<T extends object> = {
    [K in keyof T as T[K] extends Function ? never : K]: T[K];
  };

  type ReadOnlyPropertiesOmitted<T extends object> = {
    [K in keyof T as T extends Readonly<Record<K, T[K]>> ? never : K]: T[K]
  };
}

export { };