import React from "react";
import { getDisplayName } from "@/metadata";
import { useTsxHelper } from "@/helpers";

// Props.
type FieldProps = { name: string; marginBottom?: boolean; children: React.ReactNode | React.ReactNode[] };

// Component.
export default function Field(props: FieldProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <>
      {/* Label */}
      <div
        className={joinClassName(
          "text-sm sm:text-base md:text-sm xl:text-base min-w-35 md:min-w-40",
          "opacity-50 whitespace-nowrap",
        )}
      >
        {(props.name && getDisplayName(props.name)) ?? props.name}
      </div>

      {/* Value */}
      <div className={joinClassName((props.marginBottom ?? true) && "mb-3 sm:mb-1.5 md:mb-3 xl:mb-1.5")}>
        {props.children}
      </div>
    </>
  );
}