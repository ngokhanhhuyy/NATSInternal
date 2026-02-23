declare module '*.module.scss' {
  const styles: { [className: string]: string };
  export default styles;
}

declare global {
  type Implements<T, U extends T> = U;
  type ImplementsPartial<T extends IPageableListModel, U extends T> = { [P in keyof U]?: U[P] | undefined; };
  type AwaitedReturn<T> = T extends Promise<infer U> ? U : T;
  type ColorVariant = "primary" | "secondary" | "danger" | "success" | "hinting";
  type Truthy<T> = Exclude<T, false | 0 | "" | null | undefined>;
  interface RouteHandle {
    breadcrumbTitle?: string;
    pageTitle?: string;
    description?: string;
  }
}

export { };