import React from "react";

// Props.
export type NewTabPhoneLinkProps = Omit<React.ComponentPropsWithoutRef<"a">, "target" | "rel" | "children" > & {
  phoneNumber: string;
};

// Component.
export default function NewTabPhoneLink(props: NewTabPhoneLinkProps): React.ReactNode {
  // Template.
  return (
    <a href={`tel:${props.phoneNumber}`} target="_blank" rel="noopener noreferrer">
      {props.phoneNumber}
    </a>
  );
}