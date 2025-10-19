import { createMemo, type Accessor } from "solid-js";

type ComputedState<T> = {
  get value(): T;
}

type SettableComputedState<T> = ComputedState<T> & {
  set value(newValue: T);
}

type ComputedGetter<T> = (previousComputedValue?: T) => T;
type ComputedSetter<T> = (newValue: T) => void;

type ComputedGetterAndSetter<T> = {
  getter: ComputedGetter<T>;
  setter: ComputedSetter<T>;
};

export function createComputedState<T>(computer: ComputedGetter<T>): ComputedState<T>;
export function createComputedState<T>(getterAndSetter: ComputedGetterAndSetter<T>): SettableComputedState<T>;
export function createComputedState<T>(
    arg: ComputedGetter<T> | ComputedGetterAndSetter<T>): ComputedState<T> | SettableComputedState<T> {
  let getComputedValue: Accessor<T>;
  if (typeof arg === "function") {
    getComputedValue = createMemo(arg);
    return {
      get value(): T {
        return getComputedValue();
      }
    };
  }

  getComputedValue = createMemo(arg.getter);
  return {
    get value(): T {
      return getComputedValue();
    },
    set value(newValue: T) {
      arg.setter(newValue);
    }
  };
}

