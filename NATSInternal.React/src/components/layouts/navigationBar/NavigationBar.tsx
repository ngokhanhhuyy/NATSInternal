import React, { useState, useRef, useCallback, useEffect } from "react";
import { useLocation } from "react-router";
import { useNavigationBarStore } from "@/stores";
import { useApi } from "@/api";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import NavigationBarItem from "./NavigationBarItem";
import type { NavigationBarItemData } from "./NavigationBarItem";
// import CurrentUser from "./CurrentUser";

import {
  HomeIcon as HomeOutlineIcon,
  UserCircleIcon as CustomerOutlineIcon,
  ArchiveBoxIcon as ProductOutlineIcon,
  TruckIcon as SupplyOutlineIcon,
  ShoppingCartIcon as OrderOutlineIcon,
  CurrencyDollarIcon as DebtOutlineIcon,
  CreditCardIcon as ExpenseOutlineIcon,
  ChartPieIcon as ReportOutlineIcon,
  IdentificationIcon as UserOutlineIcon,
  UserIcon as PersonalOutlineIcon } from "@heroicons/react/24/outline";
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
  UserIcon as PersonalSolidIcon,
  StarIcon as ApplicationIcon, } from "@heroicons/react/24/solid";


// Component.
export default function NavigationBar(): React.ReactNode {
  // Dependencies.
  const location = useLocation();
  const navigationBarStore = useNavigationBarStore();
  const { getHomeRoutePath } = useRouteHelper();
  const { joinClassName } = useTsxHelper();

  // States.
  const minWidthMediaQuery = useRef<MediaQueryList>(window.matchMedia("(min-width: 64rem)"));
  const [activeItemName, setActiveItemName] = useState<string | null>(null);

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
    handleWindowSizeChange();
    minWidthMediaQuery.current.addEventListener("change", handleWindowSizeChange);

    return () => minWidthMediaQuery.current.removeEventListener("change", handleWindowSizeChange);
  }, []);

  useEffect(() => {
    setActiveItemName(getNavigationBarItemNameFromRoutePath(location.pathname));
  }, [location]);

  // Template.
  return (
    <nav className={joinClassName(
      "shrink-0",
      navigationBarStore.isExpanded ? "w-[190px]" : "w-fit"
    )}>
      <div id="navbar-container" className="flex flex-col justify-center items-stretch gap-1 sticky top-3 pb-[-50px]">
        {/* Application icon and name */ }
        <a
          className={joinClassName(
            "flex gap-2.5 justify-around items-center mb-2 py-1.5 hover:opacity-100 hover:no-underline",
            navigationBarStore.isExpanded && "px-2"
          )}
          href={getHomeRoutePath()}
        >
          <ApplicationIcon className={joinClassName(
            "bg-success/10 border border-success size-10 p-1.5",
            "rounded-[50%] fill-success shrink-0",
          )} />
          <span className={joinClassName(
            "text-success font-light text-2xl origin-right scale-x-110",
            !navigationBarStore.isExpanded && "hidden"
          )}>
          natsinternal
        </span>
        </a>

        {/* Navigation links */}
        <div id="navigation-bar-item-list" className="flex flex-col items-stretch">
          {navigationBarItems.map((item) => (
            <NavigationBarItem
              name={item.name}
              fallbackDisplayName={item.fallbackDisplayName}
              routePath={item.routePath}
              childItems={item.childItems}
              Icon={item.Icon}
              isActive={activeItemName === item.name}
              showLabel={navigationBarStore.isExpanded}
              key={item.name}
            />
          ))}
        </div>

        {/* Current user */}
        {/* <CurrentUser /> */}
      </div>
    </nav>
  );
}

// Static variables.
const api = useApi();
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
  {
    name: "personal",
    fallbackDisplayName: "Cá nhân",
    childItems: [
      {
        name: "myProfile",
        fallbackDisplayName: "Hồ sơ của tôi",
        routePath: routeHelper.getUserProfileRoutePath((await api.authentication.getCallerDetailAsync()).id),
        Icon: ({ className, title }) => <UserOutlineIcon className={className} title={title} />
      }
    ],
    Icon: ({ isActive, className, title }) => {
      const Component = isActive ? PersonalSolidIcon : PersonalOutlineIcon;
      return <Component className={className} title={title} />;
    }
  },
];

function getNavigationBarItemNameFromRoutePath(routePath: string): string | null {
  if (routePath === routeHelper.getHomeRoutePath()) {
    return "home";
  }

  const itemNames = navigationBarItems
    .filter(pair => pair.routePath && routePath.includes(pair.routePath))
    .map(pair => pair.name);
  if (itemNames.length === 0) {
    return null;
  }

  return itemNames[0];
}