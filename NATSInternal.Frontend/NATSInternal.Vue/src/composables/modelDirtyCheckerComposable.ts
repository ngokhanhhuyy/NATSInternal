import { ref, computed } from "vue";

// Result type.
type ModelDirtyResult = {
  get isDirty(): boolean;
};

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

// Composable.
export function useDirtyModelChecker<TModel extends object, TKey extends keyof TModel>(
    model: TModel | (() => TModel),
    excludedKeys?: TKey[]): ModelDirtyResult {
  const originalModelJson = ref(serializeModel(model));

  const result = computed(() => {
    const currentModelJson = serializeModel(model, excludedKeys);
    return originalModelJson.value !== currentModelJson;
  });
  
  return {
    get isDirty(): boolean {
      return result.value;
    }
  };
}