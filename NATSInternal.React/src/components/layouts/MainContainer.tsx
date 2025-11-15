import React from "react";
import { useTsxHelper } from "@/helpers";

// Props.
type MainContainerProps = React.ComponentPropsWithoutRef<"div">;

// Component.
export default function MainContainer({ className, ...props }: MainContainerProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <div id="main-container" className={joinClassName("w-full p-6", className)} {...props}>
      {props.children}
    </div>
  );
}