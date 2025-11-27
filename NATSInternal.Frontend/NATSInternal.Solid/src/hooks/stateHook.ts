import { createSignal, type Setter } from "solid-js";

export type StateSetter<T> = Setter<T>;
export type StateUpdaterParameter<T> = Partial<MethodsOmitted<T>> | ((previousState: T) => Partial<MethodsOmitted<T>>);
export type StateUpdater<T> = (arg: StateUpdaterParameter<T>) => T;
export type State<T> = Readonly<T & {
  $set: StateSetter<T>; 
  $update: StateUpdater<T>;
}>;

export function createState<T>(initialStateGetter: () => T): State<T>;
export function createState<T extends Exclude<unknown, Function>>(initialState: T): State<T>;
export function createState<T extends object>(arg: T | (() => T)): State<T> {
  let storeInitialValue: T;
  if (typeof arg === "function") {
    storeInitialValue = (arg as () => T)();
  } else {
    storeInitialValue = arg;
  }

  const [getSignal, setSignal] = createSignal(storeInitialValue, { equals: false });

  type StateUpdaterFunctionParameter = (previousState: T) => Partial<MethodsOmitted<T>>;
  function $update(arg: Partial<MethodsOmitted<T>> | StateUpdaterFunctionParameter): void {
    let updatedValue: T;
    if (typeof arg === "function") {
      updatedValue = setSignal((previousSignal) => ({
        ...previousSignal,
        ...(arg as StateUpdaterFunctionParameter)(previousSignal) }));
    } else {
      updatedValue = setSignal((previousSignal) => ({ ...previousSignal, ...arg }));
    }
  }

  const state = { $set: setSignal, $update };

  for (const [key, _] of Object.entries(getSignal())) {
    Object.defineProperty(state, key, {
      get() {
        return getSignal()[key];
      }
    });
  }

  return state as State<T>;
}