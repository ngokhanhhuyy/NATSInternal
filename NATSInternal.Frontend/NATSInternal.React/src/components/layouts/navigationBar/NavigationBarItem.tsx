import React, { useMemo } from "react";
import { Link } from "react-router";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Types.
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

  const displayName = useMemo<string | undefined>(() => getDisplayName(props.name) ?? props.fallbackDisplayName, []);

  // Template.
  return (
    <Link
      className={joinClassName(props.isActive && "active")}
      to={props.routePath}
      onClick={props.onClick}
    >
      {Icon && <Icon isActive={props.isActive} />}
      <span className="inline-block md:hidden lg:inline-block">
        {displayName}
      </span>

      <div className="tooltip">
        {displayName}
      </div>
    </Link>
  );
}