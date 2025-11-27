import React from "react";

// Props.
type RowProps = { children: React.ReactNode | React.ReactNode[] };

// Component.
export default function Row(props: RowProps): React.ReactNode {
  return (
    <tr className="border-b last:border-b-0 border-black/10 dark:border-white/10 whitespace-nowrap">
      {props.children}
    </tr>
  );
}