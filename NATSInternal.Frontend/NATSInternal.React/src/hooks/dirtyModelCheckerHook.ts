import { useRef } from "react";

// Utility for model serialization.
const serializeModel = <TModel extends object, TKey extends keyof TModel>
(model: TModel, excludedKeys?: TKey[]): string => {
  if (excludedKeys && excludedKeys.length) {
    return JSON.stringify(model, (key, value) => {
      if (!excludedKeys.includes(key as TKey)) {
        return value;
      }
    });
  }

  return JSON.stringify(model);
};

// Hook.
export function useDirtyModelChecker<TModel extends object, TKey extends keyof TModel>(
    originalModel: TModel,
    currentModel: TModel | undefined,
    excludedKeys?: TKey[]): boolean {
  // States.
  const originalModelJson = useRef(serializeModel(originalModel));
  return (() => {
    const currentModelJson = currentModel
      ? serializeModel(currentModel, excludedKeys)
      : "";
    return originalModelJson.current !== currentModelJson;
  })();
}