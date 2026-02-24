import React from "react";

// Props.
export type NewTabWebsiteLinkProps = Omit<React.ComponentPropsWithoutRef<"a">, "target" | "rel">;

// Component.
export default function NewTabWebsiteLink(props: NewTabWebsiteLinkProps): React.ReactNode {
  return <a {...props} target="_blank" rel="noopener noreferrer" />;
}