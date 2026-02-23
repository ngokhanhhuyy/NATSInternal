import { useRef, useMemo, useCallback, useDeferredValue } from "react";

// Types.
type OriginalModelSetter<TModel extends object> = (newOriginalModel: TModel | (() => TModel)) => void;
type Comparer<TModel extends object> = (originalModel: TModel, currentModel: TModel) => boolean;

// Hooks
export function useDirtyModelChecker<TModel extends object>(
    model: TModel | (() => TModel),
    comparer: Comparer<TModel>): [boolean, OriginalModelSetter<TModel>] {
  // States.
  const computedModel = typeof model === "function" ? model() : model;
  const originalModel = useRef<TModel>(computedModel);
  const deferredModel = useDeferredValue<TModel>(computedModel, originalModel.current);

  // Computed.
  const isDirty = useMemo(() => !comparer(originalModel.current, deferredModel), [deferredModel]);

  // Callbacks.
  const setOriginalModel = useCallback<OriginalModelSetter<TModel>>((newOriginalModel: TModel | (() => TModel)) => {
    originalModel.current = newOriginalModel as TModel;
  }, []);

  return [isDirty, setOriginalModel];
}