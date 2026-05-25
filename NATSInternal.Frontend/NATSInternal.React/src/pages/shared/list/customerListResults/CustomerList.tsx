import React from "react";
import { joinClassName } from "@/helpers";

// Child components.
import CustomerItem from "./CustomerItem";

// Props.
type CustomerListProps = {
  className?: string;
  model: CustomerListModel;
  isReloading: boolean;
  renderItemChildren?(customer: CustomerBasicModel): React.ReactNode;
  computeItemClassName?(customer: CustomerBasicModel): string | undefined;
  openLinkInNewTab?: boolean;
};

export default function CustomerList(props: CustomerListProps): React.ReactNode {
  // Template.
  return (
    <ul className={joinClassName(
      "list-group",
      props.isReloading && "opacity-50 pointer-events-none",
      props.className
    )}>
      {props.model.items.length > 0 ? props.model.items.map((customer) => (
        <CustomerItem
          className={props.computeItemClassName?.(customer)}
          model={customer}
          openLinkInNewTab={props.openLinkInNewTab}
          key={customer.id}
        >
          {props.renderItemChildren?.(customer)}
        </CustomerItem>
      )) : (
        <li className="list-group-item opacity-50 px-3 py-10">
          Không có kết quả
        </li>
      )}
    </ul>
  );
}
