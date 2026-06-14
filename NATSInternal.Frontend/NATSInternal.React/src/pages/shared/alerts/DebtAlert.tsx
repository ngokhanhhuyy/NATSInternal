import React from "react";
import { joinClassName } from "@/helpers";

// Props.
type DebtAlertProps = {
  debtAmount: number;
} & React.ComponentPropsWithoutRef<"span">;

// Components.
export default function DebtAlert(props: DebtAlertProps): React.ReactNode {
  // Props.
  const { debtAmount, ...domProps } = props;

  // Computed.
  const className = joinClassName(
    "alert font-bold",
    debtAmount > 0 && "alert-yellow-outline dark:alert-yellow" ,
    debtAmount < 0 && "alert-red-outline dark:alert-red",
    props.className
  );

  // Template.
  if (!debtAmount) {
    return;
  }
  
  return (
    <span {...domProps} className={className}>
      {debtAmount > 0 && "Nợ"}
      {debtAmount < 0 && "Cần hoàn tiền"}
    </span>
  );
}
