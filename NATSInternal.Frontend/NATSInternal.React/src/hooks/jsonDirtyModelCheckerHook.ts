import { useRef, useMemo } from "react";

type Model = { [key: string]: any };

export function useJSONDirtyModelChecker<TModel extends Model>(
  model: TModel | (() => TModel),
  excludedKeys?: (keyof TModel)[])
{
  // States.
  const currentModel = typeof model === "function" ? model() : model;
  const originalModelJson = useRef<string>(serializeModel(currentModel, excludedKeys));

  // Computed.
  const isDirty = useMemo<boolean>(() => {
    return serializeModel(currentModel, excludedKeys) !== originalModelJson.current;
  }, [currentModel]);

  return isDirty;
}

function serializeModel<TModel extends Model>(model: TModel, excludedKeys?: (keyof TModel)[]): string {
  if (!excludedKeys?.length) {
    return JSON.stringify(model);
  }

  const specifiedKeysExcludedModel: { [key: string]: any } = {};
  for (const [key, value] of Object.entries(model)) {
    if (!excludedKeys.includes(key)) {
      specifiedKeysExcludedModel[key] = value;
    }
  }

  return JSON.stringify(specifiedKeysExcludedModel);
}