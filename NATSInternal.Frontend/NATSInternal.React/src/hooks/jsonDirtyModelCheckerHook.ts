import { useRef, useMemo } from "react";

type Model = { [key: string]: any };

export function useJSONDirtyModelChecker<TModel extends Model>(
  model: TModel | (() => TModel),
  excludedKeys?: (keyof TModel)[]): [boolean, (newOriginalModel: TModel | (() => TModel)) => void]
{
  // States.
  const currentModel = typeof model === "function" ? model() : model;
  const originalModelJson = useRef<string>(serializeModel(currentModel, excludedKeys));

  // Computed.
  const isDirty = useMemo<boolean>(() => {
    return serializeModel(currentModel, excludedKeys) !== originalModelJson.current;
  }, [currentModel]);

  // Callbacks.
  const setNewOriginalModel = (newOriginalModel: TModel | (() => TModel)) => {
    originalModelJson.current = serializeModel(newOriginalModel, excludedKeys);
  };

  return [isDirty, setNewOriginalModel];
}

function serializeModel<TModel extends Model>(model: TModel | (() => TModel), excludedKeys?: (keyof TModel)[]): string {
  const computedModel = typeof model === "function" ? model() : model;

  if (!excludedKeys?.length) {
    return JSON.stringify(computedModel);
  }

  const specifiedKeysExcludedModel: { [key: string]: any } = {};
  for (const [key, value] of Object.entries(computedModel)) {
    if (!excludedKeys.includes(key)) {
      specifiedKeysExcludedModel[key] = value;
    }
  }

  return JSON.stringify(specifiedKeysExcludedModel);
}
