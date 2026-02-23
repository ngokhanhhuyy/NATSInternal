import React from "react";

// Props.
type FieldContainerProps = { children: React.ReactNode | React.ReactNode[] };

// Component.
export default function FieldContainer(props: FieldContainerProps): React.ReactNode {
  // Template.
  return (
    <div className="w-full flex flex-col gap-x-5 gap-y-3 px-3">
      {props.children}
    </div>
  );
}