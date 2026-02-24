import React from "react";

// Props.
export type NewTabEmailLinkProps = Omit<React.ComponentPropsWithoutRef<"a">, "target" | "rel" | "children" > & {
  email: string;
};

// Component.
export default function NewTabEmailLink(props: NewTabEmailLinkProps): React.ReactNode {
  // Template.
  return (
    <a href={"mailto:" + props.email.toLowerCase()} target="_blank" rel="noopener noreferrer">
      {props.email.toLowerCase()}
    </a>
  );
}