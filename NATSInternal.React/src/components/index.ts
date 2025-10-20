import { useApi } from "@/api";
import { createState, createComputedState } from "@/hooks";
import { useTsxHelper, useJsonHelper, useRouteHelper } from "@/helpers";
import { useAuthenticationStore } from "@/stores/authenticationStore";

export type ComponentDependencies = {
  api: ReturnType<typeof useApi>;
  hooks: {
    createState: typeof createState;
    createComputedState: typeof createComputedState;
  },
  stores: {
    authentication: ReturnType<typeof useAuthenticationStore>
  },
  helpers: {
    htmlHelper: ReturnType<typeof useTsxHelper>;
    jsonHelper: ReturnType<typeof useJsonHelper>;
    routeHelper: ReturnType<typeof useRouteHelper>;
  }
};

export const componentDependencies: ComponentDependencies = {
  api: useApi(),
  hooks: {
    createState,
    createComputedState,
  },
  stores: {
    authentication: useAuthenticationStore(),
  },
  helpers: {
    htmlHelper: useTsxHelper(),
    jsonHelper: useJsonHelper(),
    routeHelper: useRouteHelper()
  }
}

export type Component<TProps extends object> = (dependencies: ComponentDependencies, props: TProps) => JSX.Element;

export function createComponent<TProps extends object>(component: Component<TProps>): (props: TProps) => JSX.Element {
  return (props: TProps) => component(componentDependencies, props);
}