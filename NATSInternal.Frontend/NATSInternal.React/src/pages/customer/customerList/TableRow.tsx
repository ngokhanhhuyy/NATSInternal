import React from "react";
import { Link } from "react-router";
import { useTsxHelper } from "@/helpers";

// Props.
type TableRowProps = { model: CustomerListCustomerModel };

// Component.
export default function TableRow(props: TableRowProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <tr className="odd:bg-black/3 dark:odd:bg-white/3 whitespace-nowrap">
      <td className="px-3 py-2">
        <Link to={props.model.detailRoute} className="font-bold">
          {props.model.fullName}
        </Link>
      </td>

      <td className="px-3 py-2">
        {props.model.nickName && (
          <span className="opacity-50">{props.model.nickName}</span>
        )}
      </td>

      <td className={joinClassName(
        "px-3 py-2 opacity-75",
        props.model.gender === "Male" ? "text-blue-500" : "text-red-500"
      )}>
        {props.model.gender === "Male" ? "Nam" : "Nữ"}
      </td>

      <td className="px-3 py-2">
        {props.model.formattedPhoneNumber}
      </td>

      <td className="px-3 py-2">
        {props.model.formattedBirthday}
      </td>

      <td className={joinClassName("px-3 py-2", !props.model.debtRemainingAmount && "opacity-25")}>
        {props.model.formattedDebtRemainingAmount}
      </td>
    </tr>
  );
}