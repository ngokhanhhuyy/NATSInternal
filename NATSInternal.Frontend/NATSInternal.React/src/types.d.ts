import React from "react";

declare module '*.module.scss' {
  const styles: { [className: string]: string };
  export default styles;
}

declare global {
  type Implements<T, U extends T> = U;
  type ImplementsPartial<T extends IPaginatedList, U extends T> = { [P in keyof U]?: U[P] | undefined; };
  type AwaitedReturn<T> = T extends Promise<infer U> ? U : T;
  type ColorVariant = "primary" | "secondary" | "danger" | "success" | "hinting";
  type ComponentProps<TComponent> = TComponent extends (props: infer TProps) => React.ReactNode ? TProps : never;
  type Truthy<T> = Exclude<T, false | 0 | "" | null | undefined>;

  declare module "react-router" {
    interface RouteHandle {
      title: string;
      breadcrumb: { name: string; routePath: string; };
    }
  }

}

export { };