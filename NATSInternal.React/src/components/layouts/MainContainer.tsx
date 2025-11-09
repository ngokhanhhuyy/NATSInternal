import React from "react";

// Component.
export default function MainContainer(props: React.ComponentPropsWithoutRef<"div">): React.ReactNode {
  return (
    <div id="main-container" className="w-full p-3">
      {props.children}
    </div>
  );
}