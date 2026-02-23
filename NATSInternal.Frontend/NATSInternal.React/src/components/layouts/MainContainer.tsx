import React, { useEffect } from "react";
import { useTsxHelper } from "@/helpers";

// Props.
type MainContainerProps = {
  isLoading?: boolean;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function MainContainer({ isLoading, children, ...domProps }: MainContainerProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();
  // Effect.
  useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  // Template.
  return (
    <div
      {...domProps}
      id="main-container"
      className={joinClassName(
        domProps.className,
        "transition-opacity duration-200",
        isLoading && "opacity-50 cursor-wait"
      )}
    >
      {children}
    </div>
  );
}