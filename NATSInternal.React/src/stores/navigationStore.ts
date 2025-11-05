import { createStore } from "zustand";
import { useRouteHelper } from "@/helpers";

// Types.
type NavigationBarItem = 
  | "home"
  | "authentication"
  | "users"
  | "products"
  | "orders"
  | "treatments";

type BreadcrumbItem = {
  name: string;
  routePath?: string;
};

export type NavigationStore = {
  readonly navigationBarSelectedItem: NavigationBarItem;
  readonly breadcrumbItems: BreadcrumbItem[];
  updateNavigationBarSelectedItem(routePath: string): void;
  setBreadcrumbItems(breadcrumbItems: BreadcrumbItem[]): void;
};

// Helpers.
const { getHomeRoutePath } = useRouteHelper();

// Store.
export const useNavigationStore = createStore<NavigationStore>((set, get) => ({
  navigationBarSelectedItem: getNavigationBarItemFromRoutePath("/"),
  breadcrumbItems: [
    { name: "Trang chủ", routePath: getHomeRoutePath() },
  ],
  updateNavigationBarSelectedItem(routePath: string): void {
    set({ navigationBarSelectedItem: getNavigationBarItemFromRoutePath(routePath) });
  },
  setBreadcrumbItems(breadcrumbItems: BreadcrumbItem[]): void {
    set({ breadcrumbItems: [get().breadcrumbItems[0], ...breadcrumbItems] });
  },
}));

function getNavigationBarItemFromRoutePath(routePath: string): NavigationBarItem {
  const pathSegments = routePath.split("/");
  if (pathSegments.length === 0 || !pathSegments[0]) {
    return "home";
  }

  return pathSegments[0] as NavigationBarItem;
}