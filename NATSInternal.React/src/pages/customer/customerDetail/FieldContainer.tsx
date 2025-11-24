import React from "react";
import { useTsxHelper } from "@/helpers";

// Props.
type FieldContainer = { children: React.ReactNode | React.ReactNode[] };

// Component.
export default function FieldContainer(props: FieldContainer): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <div className={joinClassName(
      "w-full grid grid-cols-1 sm:grid-cols-[min-content_1fr] md:grid-cols-1",
      "xl:grid-cols-[min-content_1fr] gap-x-5 px-3"
    )}>
      {props.children}
    </div>
  );
}