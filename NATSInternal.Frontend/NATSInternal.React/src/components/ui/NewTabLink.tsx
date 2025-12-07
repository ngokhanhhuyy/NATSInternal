import React from "react";

// Props.
export type NewTabLinkProps = Omit<React.ComponentPropsWithoutRef<"a">, "target" | "rel">;

// Component.
export default function NewTabLink(props: NewTabLinkProps): React.ReactNode {
  return <a {...props} target="_blank" rel="noopener noreferrer" />;
}