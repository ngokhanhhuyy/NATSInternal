import React from "react";

declare module '*.module.scss' {
  const styles: { [className: string]: string };
  export default styles;
}

declare global {
  type AwaitedReturn<T> = T extends Promise<infer U> ? U : T;
  type SyncApi<T> = {
    [K in keyof T]:
      T[K] extends (...args: infer A) => infer R
        ? (...args: A) => AwaitedReturn<R>
        : T[K];
  };
  type ColorVariant = "primary" | "secondary" | "danger" | "success" | "hinting";
  type ComponentProps<TComponent> = TComponent extends (props: infer TProps) => React.ReactNode ? TProps : never;
  type ForwardRefComponentHandler<T> = T extends React.ForwardRefExoticComponent<
    React.PropsWithoutRef<any> & React.RefAttributes<infer R>
  > ? R : never;

  declare module "react-router" {
    interface RouteHandle {
      title: string;
      breadcrumb: { name: string; routePath: string; };
    }
  }

}

export { };