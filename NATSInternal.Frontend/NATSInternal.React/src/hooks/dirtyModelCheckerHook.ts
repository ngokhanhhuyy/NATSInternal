import { useRef } from "react";

// Utility for model serialization.
const serializeModel = <TModel extends object, TKey extends keyof TModel>
    (model: TModel | (() => TModel), excludedKeys?: TKey[]): string => {
  const computedModel = typeof model === "function" ? model() : model;
  if (excludedKeys && excludedKeys.length) {
    return JSON.stringify(computedModel, (key, value) => {
      if (!excludedKeys.includes(key as TKey)) {
        return value;
      }
    });
  }

  return JSON.stringify(model);
};

// Hook.
export function useDirtyModelChecker<TModel extends object, TKey extends keyof TModel>(
    model: TModel | (() => TModel),
    excludedKeys?: TKey[]): boolean {
  // States.

  const originalModelJson = useRef(serializeModel(model));
  return (() => {
    const currentModelJson = serializeModel(model, excludedKeys);
    return originalModelJson.current !== currentModelJson;
  })();
}