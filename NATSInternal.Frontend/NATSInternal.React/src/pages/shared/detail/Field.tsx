import React from "react";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Props.
type Props = { propertyName: string; } & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function Field({ propertyName, ...props }: Props): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  if (props.children == null) {
    return null;
  }

  return (
    <div className="flex flex-col">
      <span className="text-sm font-bold opacity-50">
        {getDisplayName(propertyName) ?? propertyName}
      </span>

      <span {...props} className={joinClassName("text-blue-700 dark:text-blue-400", props.className)} />
    </div>
  );
}