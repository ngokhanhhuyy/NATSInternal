import React, { useState, useRef, useCallback, useEffect } from "react";
import { useLocation } from "react-router";
import { useNavigationBarStore } from "@/stores";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import NavigationBarItem from "./NavigationBarItem";
import type { NavigationBarItemName, NavigationBarItemData } from "./NavigationBarItem";
import CurrentUser from "./CurrentUser";

import {
  HomeIcon as HomeOutlineIcon,
  UserCircleIcon as CustomerOutlineIcon,
  ArchiveBoxIcon as ProductOutlineIcon,
  TruckIcon as SupplyOutlineIcon,
  ShoppingCartIcon as OrderOutlineIcon,
  CurrencyDollarIcon as DebtOutlineIcon,
  CreditCardIcon as ExpenseOutlineIcon,
  ChartPieIcon as ReportOutlineIcon,
  IdentificationIcon as UserOutlineIcon } from "@heroicons/react/24/outline";
import {
  HomeIcon as HomeSolidIcon,
  UserCircleIcon as CustomerSolidIcon,
  ArchiveBoxIcon as ProductSolidIcon,
  TruckIcon as SupplySolidIcon,
  ShoppingCartIcon as OrderSolidIcon,
  CurrencyDollarIcon as DebtSolidIcon,
  CreditCardIcon as ExpenseSolidIcon,
  ChartPieIcon as ReportSolidIcon,
  IdentificationIcon as UserSolidIcon,
  StarIcon as ApplicationIcon } from "@heroicons/react/24/solid";


// Component.
export default function NavigationBar(props: { asPlaceholder?: boolean; }): React.ReactNode {
  // Dependencies.
  const location = useLocation();
  const navigationBarStore = useNavigationBarStore();
  const { getHomeRoutePath } = useRouteHelper();
  const { joinClassName } = useTsxHelper();

  // States.
  const minWidthMediaQuery = useRef<MediaQueryList>(window.matchMedia("(min-width: 64rem)"));
  const [activeItemName, setActiveItemName] = useState<NavigationBarItemName | null>(null);

  // Callback.
  const handleWindowSizeChange = useCallback(() => {
    if (minWidthMediaQuery.current.matches) {
      navigationBarStore.expand();
    } else {
      navigationBarStore.collapse();
    }
  }, []);

  // Effect.
  useEffect(() => {
    if (props.asPlaceholder) {
      return;
    }
    handleWindowSizeChange();
    minWidthMediaQuery.current.addEventListener("change", handleWindowSizeChange);

    return () => minWidthMediaQuery.current.removeEventListener("change", handleWindowSizeChange);
  }, []);

  useEffect(() => {
    if (props.asPlaceholder) {
      return;
    }

    setActiveItemName(getNavigationBarItemNameFromRoutePath(location.pathname));
  }, [location]);

  // Template.
  return (
    <nav className={joinClassName(
      "px-[15px] py-3 overflow-hidden overflow-y-auto shrink-0 relative gap-1 transition-[width] duration-300",
      props.asPlaceholder ? "invisible hidden 2xl:flex shrink-0" : "flex flex-col items-stretch",
      navigationBarStore.isExpanded ? "w-[220px]" : "w-[70.25px]"
    )}>
      {/* Application icon and name */ }
      <a
        className="flex gap-2.5 justify-center items-center mb-2 py-1.5 hover:opacity-100 hover:no-underline"
        href={getHomeRoutePath()}
      >
        <ApplicationIcon className={joinClassName(
          "bg-success/10 border border-success size-10 p-1.5",
          "rounded-[50%] fill-success shrink-0",
        )} />
        <span className={joinClassName(
          "text-success font-light text-2xl origin-left scale-x-110 transition-all duration-100",
          !navigationBarStore.isExpanded && "hidden"
        )}>
          natsinternal
        </span>
      </a>

      {/* Navigation links */ }
      <div id="navigation-bar-item-list" className="flex flex-col overflow-auto relative">
        {!props.asPlaceholder && navigationBarItems.map((item) => (
          <NavigationBarItem
            name={item.name}
            fallbackDisplayName={item.fallbackDisplayName}
            routePath={item.routePath}
            Icon={item.Icon}
            isActive={activeItemName === item.name}
            showLabel={navigationBarStore.isExpanded}
            key={item.name}
          />
        ))}
      </div>

      {/* Current user */}
      <CurrentUser />
    </nav>
  );
}

// Static variables.
const routeHelper = useRouteHelper();
const navigationBarItems: NavigationBarItemData[] = [
  {
    name: "home",
    fallbackDisplayName: "Trang chủ",
    routePath: routeHelper.getHomeRoutePath(),
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? HomeSolidIcon : HomeOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
  {
    name: "customer",
    routePath: routeHelper.getCustomerListRoutePath(),
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? CustomerSolidIcon : CustomerOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
  {
    name: "product",
    routePath: routeHelper.getProductListRoutePath(),
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? ProductSolidIcon : ProductOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
  {
    name: "supply",
    routePath: routeHelper.getSupplyListRoutePath(),
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? SupplySolidIcon : SupplyOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
  {
    name: "order",
    routePath: routeHelper.getOrderListRoutePath(),
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? OrderSolidIcon : OrderOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
  {
    name: "debt",
    routePath: routeHelper.getDebtOverviewRoutePath(),
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? DebtSolidIcon : DebtOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
  {
    name: "expense",
    routePath: routeHelper.getExpenseListRoutePath(),
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? ExpenseSolidIcon : ExpenseOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
  {
    name: "report",
    routePath: routeHelper.getReportRoutePath(),
    fallbackDisplayName: "Báo cáo",
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? ReportSolidIcon : ReportOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
  {
    name: "user",
    routePath: routeHelper.getUserListRoutePath(),
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? UserSolidIcon : UserOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
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