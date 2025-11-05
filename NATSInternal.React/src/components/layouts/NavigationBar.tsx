import React, { useState, useEffect } from "react";
import { useLocation } from "react-router";
import { getDisplayName } from "@/metadata";
import { useRouteHelper, useTsxHelper } from "@/helpers";
import styles from "./NavigationBar.module.css";

// Types.
type NavigationBarItemName = 
  | "home"
  | "customer"
  | "product"
  | "supply"
  | "order"
  | "debt"
  | "expense"
  | "report"
  | "user";

type NavigationBarItem = {
  name: NavigationBarItemName;
  fallbackDisplayName?: string;
  routePath: string;
  iconClassName: string;
};

// Component.
export default function NavigationBar(): React.ReactNode {
  // Dependencies.
  const location = useLocation();
  const { joinClassName } = useTsxHelper();

  // States.
  const [activeItemName, setActiveItemName] = useState<NavigationBarItemName | null>(null);

  // Effect.
  useEffect(() => {
    setActiveItemName(getNavigationBarItemNameFromRoutePath(location.pathname));
  }, [location]);

  // Template.
  function renderItem(item: NavigationBarItem): React.ReactNode {
    const isActive = activeItemName === item.name;
    return (
      <a
        className={joinClassName(styles.item, isActive && styles.active)}
        href={item.routePath}
        key={item.name}
      >
        <i className={item.iconClassName} />
        <span className="text-indigo-500 ms-3">{item.fallbackDisplayName ?? getDisplayName(item.name)}</span>
      </a>
    );
  }

  return (
    <nav className={styles.navigationBar}>
      {navigationBarItems.map((item) => renderItem(item))}
    </nav>
  );
}

// Static variables.
const routeHelper = useRouteHelper();
const navigationBarItems: NavigationBarItem[] = [
  {
    name: "home",
    fallbackDisplayName: "Trang chủ",
    routePath: routeHelper.getHomeRoutePath(),
    iconClassName: "bi bi-house"
  },
  { name: "customer", routePath: routeHelper.getCustomerListRoutePath(), iconClassName: "bi bi-person-circle" },
  { name: "product", routePath: routeHelper.getProductListRoutePath(), iconClassName: "bi bi-box-seam" },
  { name: "supply", routePath: routeHelper.getSupplyListRoutePath(), iconClassName: "bi bi-truck" },
  { name: "order", routePath: routeHelper.getOrderListRoutePath(), iconClassName: "bi bi-cart4" },
  { name: "debt", routePath: routeHelper.getDebtOverviewRoutePath(), iconClassName: "bi bi-hourglass-bottom" },
  { name: "expense", routePath: routeHelper.getReportRoutePath(), iconClassName: "bi bi-cash-coin" },
  {
    name: "report",
    routePath: routeHelper.getReportRoutePath(),
    fallbackDisplayName: "Báo cáo",
    iconClassName: "bi bi-graph-up-arrow"
  },
  { name: "user", routePath: routeHelper.getUserListRoutePath(), iconClassName: "bi bi-person-badge" },
];

function getNavigationBarItemNameFromRoutePath(routePath: string): NavigationBarItemName | null {
  if (routePath === routeHelper.getHomeRoutePath()) {
    return "home";
  }

  const itemNames = navigationBarItems.filter(pair => routePath.includes(pair.routePath)).map(pair => pair.name);
  if (itemNames.length === 0) {
    return null;
  }

  return itemNames[0];
}