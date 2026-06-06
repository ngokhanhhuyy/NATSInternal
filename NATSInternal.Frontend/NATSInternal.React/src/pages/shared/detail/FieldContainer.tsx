import React from "react";
import { joinClassName } from "@/helpers";

// Props.
type FieldContainerProps = {
  className?: string;
  children: React.ReactNode | React.ReactNode[]
};

// Component.
export default function FieldContainer(props: FieldContainerProps): React.ReactNode {
  // Template.
  return (
    <div className={joinClassName("w-full flex flex-col gap-x-5 gap-y-3", props.className)}>
      {props.children}
    </div>
  );
}
