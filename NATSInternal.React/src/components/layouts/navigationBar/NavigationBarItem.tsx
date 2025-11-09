import React, { useMemo } from "react";
import { Link } from "react-router";
import { useNavigationBarStore } from "@/stores";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Types.
export type NavigationBarItemName = 
  | "home"
  | "customer"
  | "product"
  | "supply"
  | "order"
  | "debt"
  | "expense"
  | "report"
  | "user";

export type NavigationBarItemData = {
  name: NavigationBarItemName;
  fallbackDisplayName?: string;
  routePath: string;
  Icon?: (props: { isActive: boolean, className?: string, title?: string }) => React.ReactNode;
};

export type NavigationBarItemProps = NavigationBarItemData & { isActive: boolean; showLabel: boolean; };

export default function NavigationBarItem({ Icon, ...props }: NavigationBarItemProps): React.ReactNode {
  // Dependencies.
  const isExpanded = useNavigationBarStore(store => store.isExpanded);
  const { joinClassName } = useTsxHelper();

  // Computed.
  const displayName = useMemo<string | undefined>(() => getDisplayName(props.name) ?? props.fallbackDisplayName, []);

  // Template.
  return (
    <Link
      className={joinClassName(
        "flex items-center rounded-xl relative overflow-hidden gap-3 px-3 py-2.5 hover:no-underline",
        props.isActive
          ? "bg-primary text-primary-foreground border-primary shadow-sm"
          : "bg-transparent hover:bg-primary/5",
      )}
      to={props.routePath}
    >
      {Icon && <Icon className="size-5.5 inline shrink-0" isActive={props.isActive} />}
      <span className={joinClassName(
        "whitespace-nowrap overflow-hidden text-clip transition-opacity",
        !isExpanded && "opacity-0"
      )}>
        {displayName}
      </span>
    </Link>
  );
}