declare global {
  type ClonableModel<TModel extends object> = TModel & {
    $clone(changedData: CloneableModelChangedData<TModel>): ClonableModel<TModel>;
  };

  type CloneableModelChangedData<TModel extends object> = Omit<
    Partial<ReadOnlyPropertiesOmitted<MethodsOmitted<TModel>>>,
    "$clone"
  >;
}

export function createCloneMethod<TModel extends ClonableModel<object>>(getTarget: () => TModel) {
  return (changedData: CloneableModelChangedData<TModel>) => ({ ...getTarget(), ...changedData });
}