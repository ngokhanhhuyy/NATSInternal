import React, { useMemo } from "react";
import { Link } from "react-router";
import { useNavigationBarStore } from "@/stores";
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
  routePath?: string;
  childItems?: (NavigationBarItemData & { routePath: string })[];
  Icon?: (props: { isActive: boolean, className?: string, title?: string }) => React.ReactNode;
};

export type NavigationBarItemProps = NavigationBarItemData & { isActive: boolean; showLabel: boolean; };
type NavigationBarChildItemProps = Omit<NavigationBarItemData, "childItems"> & { routePath: string; };

export default function NavigationBarItem({ Icon, ...props }: NavigationBarItemProps): React.ReactNode {
  // Dependencies.
  const navigationBarStore = useNavigationBarStore();
  const { joinClassName } = useTsxHelper();

  // Computed.
  const className = useMemo(() => {
    return joinClassName(
      styles.navigationBarItem,
      "flex justify-stretch items-center rounded-xl relative gap-3 px-3 py-2.5 hover:no-underline",
      !navigationBarStore.isExpanded && styles.collapsed,
      props.routePath && "cursor-pointer",
      props.isActive
        ? "bg-primary text-primary-foreground border-primary shadow-sm"
        : joinClassName("bg-transparent", props.routePath && "hover:bg-primary/5"),
    );
  }, [props.isActive]);

  const displayName = useMemo<string | undefined>(() => computeDisplayName(props.name, props.fallbackDisplayName), []);

  // Template.
  const children = (
    <>
      {Icon && <Icon className="size-6 inline shrink-0" isActive={props.isActive} />}
      {navigationBarStore.isExpanded && (<span>{displayName}</span>)}

      <div className={joinClassName(
        "bg-neutral-700 text-white rounded-lg pointer-events-none transition-opacity duration-100",
        "absolute whitespace-nowrap w-fit z-1000 shadow-md ms-2 px-2 py-0.5 left-full opacity-0",
        styles.tooltip
      )}>
        {displayName}
      </div>
    </>
  );

  if (props.childItems?.length) {
    return (
      <>
        <div className={className}>
          {children}
        </div>

        <div className={joinClassName(
          "flex flex-col gap-2",
          navigationBarStore.isExpanded && "ms-5.5 border-l border-primary/20"
        )}>
          {props.childItems?.map((item) => (
            <ChildItem
              name={item.name}
              fallbackDisplayName={item.fallbackDisplayName}
              routePath={item.routePath}
              Icon={item.Icon}
              key={item.routePath}
            />
          ))}
        </div>
      </>
    );
  }

  if (props.routePath) {
    return (
      <Link className={className} to={props.routePath}>
        {children}
      </Link>
    );
  }

  return null;
}

function ChildItem({ Icon, ...props }: NavigationBarChildItemProps): React.ReactNode {
  // Dependencies.
  const isExpanded = useNavigationBarStore(store => store.isExpanded);
  const { joinClassName } = useTsxHelper();

  // Computed.
  const displayName = useMemo<string | undefined>(() => computeDisplayName(props.name, props.fallbackDisplayName), []);

  // Template.
  return (
    <Link
      to={props.routePath}
      className={joinClassName(
        "flex items-center gap-3 px-2 py-1 rounded-lg",
        "hover:bg-primary/5 hover:opacity-100 hover:no-underline",
        isExpanded ? "justify-stretch ms-2 opacity-50" : "justify-center"
      )}
    >
      {Icon && <Icon className="size-4 inline shrink-0" isActive={false} />}
      {isExpanded && (
        <span className="text-sm">
          {displayName}
        </span>
      )}
    </Link>
  );
}

function computeDisplayName(name: string, fallbackDisplayName?: string): string | undefined {
  return getDisplayName(name) ?? fallbackDisplayName;
}