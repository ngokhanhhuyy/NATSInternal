import type { RouteLocationRaw, type RouteLocationNormalizedLoadedGeneric } from "vue-router";
import { useRouteHelper, type RouteLocationRaw } from "@/helpers";

const routeHelper = useRouteHelper();

interface BreadcrumbItem {
  text: string;
  to: RouteLocationRaw;
}

declare module "vue-router" {
  interface RouteMeta {
    pageTitle?: string;
    breadcrumbItem: BreadcrumbItem | ((route: RouteLocationNormalizedLoadedGeneric) => BreadcrumbItem);
  }
}
