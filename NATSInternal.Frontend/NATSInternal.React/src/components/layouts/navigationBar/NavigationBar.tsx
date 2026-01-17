import React, { useState, useRef, useEffect } from "react";
import { useLocation } from "react-router";
import { useNavigationBarStore } from "@/stores";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child components.
import NavigationBarItem from "./NavigationBarItem";
import type { NavigationBarItemData } from "./NavigationBarItem";

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
  IdentificationIcon as UserSolidIcon } from "@heroicons/react/24/solid";


// Component.
export default function NavigationBar(): React.ReactNode {
  // Dependencies.
  const location = useLocation();
  const navigationBarStore = useNavigationBarStore();
  const { joinClassName } = useTsxHelper();

  // States.
  const mdScreenMediaQuery = useRef(window.matchMedia("(min-width: 48rem)"));
  const navigationBarElementRef = useRef<HTMLElement>(null!);
  const navigationBarContainerElementRef = useRef<HTMLDivElement>(null!);
  const [activeItemName, setActiveItemName] = useState<string | null>(null);

  // Effect.
  useEffect(() => {
    const handleMdScreenQueryMatchChanged = () => {
      if (mdScreenMediaQuery.current.matches) {
        navigationBarStore.collapse();
      }
    };

    handleMdScreenQueryMatchChanged();
    mdScreenMediaQuery.current.addEventListener("change", handleMdScreenQueryMatchChanged);

    return () => {
      mdScreenMediaQuery.current.addEventListener("change", handleMdScreenQueryMatchChanged);
    };
  }, []);

  useEffect(() => {
    const handleClicked = (event: PointerEvent) => {
      if (!navigationBarStore.isExpanded) {
        return;
      }

      if (navigationBarElementRef.current === (event.target as HTMLElement)) {
        navigationBarStore.collapse();
      }
    };

    document.addEventListener("pointerdown", handleClicked);

    if (navigationBarStore.isExpanded) {
      document.documentElement.style.maxHeight = "100vh";
      document.documentElement.style.overflow = "hidden";
    } else {
      document.documentElement.removeAttribute("style");
    }

    return () => document.removeEventListener("pointerdown", handleClicked);
  }, [navigationBarStore.isExpanded]);

  useEffect(() => {
    for (const item of navigationBarItems) {
      if (location.pathname.startsWith(item.routePath)) {
        setActiveItemName(item.name);
      }
    }
  }, [location.pathname]);

  // Template.
  return (
    <nav
      id="navbar"
      className={joinClassName(navigationBarStore.isExpanded && "expanded")}
      ref={navigationBarElementRef}
    >
      <div id="navbar-container" ref={navigationBarContainerElementRef}>
        {/* Navigation links */}
        <div id="navbar-item-list">
          {navigationBarItems.map((item) => (
            <NavigationBarItem
              name={item.name}
              fallbackDisplayName={item.fallbackDisplayName}
              routePath={item.routePath}
              Icon={item.Icon}
              isActive={activeItemName === item.name}
              showLabel={navigationBarStore.isExpanded}
              onClick={navigationBarStore.collapse}
              key={item.name}
            />
          ))}
        </div>
      </div>
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