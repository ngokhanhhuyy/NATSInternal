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
    <div className="grid grid-cols-1 sm:grid-cols-[150px_1fr] lg:grid-cols-1 xl:grid-cols-[150px_1fr]">
      <span className="block opacity-50 font-bold">
        {getDisplayName(propertyName) ?? propertyName}
      </span>

      <span {...props} className={joinClassName("block", props.className)} />
    </div>
  );
}