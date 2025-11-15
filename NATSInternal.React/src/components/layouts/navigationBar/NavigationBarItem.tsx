import React, { useMemo } from "react";
import { Link } from "react-router";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";
import styles from "./NavigationBarItem.module.css";

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
  | "user"
  | "personal";

export type NavigationBarItemData = {
  name: string;
  fallbackDisplayName?: string;
  routePath: string;
  Icon?: (props: { isActive: boolean, className?: string, title?: string }) => React.ReactNode;
};

export type NavigationBarItemProps = NavigationBarItemData & {
  isActive: boolean;
  showLabel: boolean;
  onClick(): any;
};

export default function NavigationBarItem({ Icon, ...props }: NavigationBarItemProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Computed.
  const className = useMemo(() => {
    return joinClassName(
      "flex justify-stretch items-center rounded-xl relative gap-3 px-3 py-2.5 hover:no-underline",
      props.routePath && "cursor-pointer",
      props.isActive
        ? "bg-primary text-primary-foreground border-primary shadow-sm"
        : joinClassName("bg-transparent", props.routePath && "hover:bg-primary/5 dark:hover:bg-primary/20"),
    );
  }, [props.isActive]);

  const displayName = useMemo<string | undefined>(() => getDisplayName(props.name) ?? props.fallbackDisplayName, []);

  // Template.
  return (
    <Link className={joinClassName(className, styles.navigationBarItem)} to={props.routePath} onClick={props.onClick}>
      {Icon && <Icon className="size-6 inline shrink-0 dark:stroke-white" isActive={props.isActive} />}
      <span className="inline-block md:hidden lg:inline-block">{displayName}</span>

      <div className={joinClassName(
        "bg-black/70 dark:bg-white/70 text-white dark:text-black rounded-md",
        "pointer-events-none absolute whitespace-nowrap w-fit z-1000 shadow-md ms-2 px-3 py-2 left-full",
        "backdrop-blur-xs",
        styles.tooltip
      )}>
        {displayName}
      </div>
    </Link>
  );
}