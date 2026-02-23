import type { FC } from "react";

type DataLoader<TModel> = (...args: any[]) => Promise<TModel>;
type ComponentWithDataLoader<TProps, TDataLoader extends DataLoader<TModel>, TModel> = FC<TProps> & {
  dataLoader: TDataLoader;
};

export function createComponentWithDataLoader<TProps, TDataLoader extends DataLoader<TModel>, TModel>
  ({ Component, dataLoader }: { Component: FC<TProps>; dataLoader: TDataLoader })
{
  (Component as ComponentWithDataLoader<TProps, TDataLoader, TModel>).dataLoader = dataLoader;
  return Component;
}