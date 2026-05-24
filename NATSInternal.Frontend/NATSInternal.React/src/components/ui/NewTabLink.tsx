import React from "react";

// Props.
export type NewTabLinkProps = Omit<React.ComponentPropsWithoutRef<"a">, "target" | "rel" > & {
  url: string;
};

// Component.
export default function NewTabLink(props: NewTabLinkProps): React.ReactNode {
  // Template.
  return (
    <a {...props} href={props.url} target="_blank" rel="noopener noreferrer"/> 
  );
}
