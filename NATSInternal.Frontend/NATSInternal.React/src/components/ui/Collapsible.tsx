import React, { useRef, useEffect } from "react";
import { useTsxHelper } from "@/helpers";

// Props.
export type CollapsibleProps = {
  isCollapsed: boolean;
} & React.ComponentPropsWithoutRef<"div">;

// Component.
export default function Collapsible({ isCollapsed, ...props }: CollapsibleProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // States.
  const elementRef = useRef<HTMLDivElement>(null!);

  // Effect.
  useEffect(() => {
    const scrollHeight = elementRef.current.scrollHeight;
    const styleMaxHeight = isCollapsed ? "0px" : `${scrollHeight}px`;
    elementRef.current.style.maxHeight = styleMaxHeight;
  }, [isCollapsed]);

  // Template.
  return (
    <div
      {...props}
      ref={elementRef}
      className={joinClassName(
        "collapsible h-fit transition-[max-height] duration-200 ease-in-out overflow-hidden",
        props.className
      )}
    />
  );
}