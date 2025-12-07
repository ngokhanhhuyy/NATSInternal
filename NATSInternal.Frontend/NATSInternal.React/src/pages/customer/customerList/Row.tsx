import React from "react";
import { Link } from "react-router";
import { useTsxHelper } from "@/helpers";

// Child components.
import { InformationCircleIcon } from "@heroicons/react/24/outline";

// Props.
type RowProps = { model: CustomerListCustomerModel };

// Component.
export default function Row(props: RowProps): React.ReactNode {
  // Dependencies.
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <tr className={joinClassName(
      "odd:bg-black/3 dark:odd:bg-white/3",
      "border-b last:border-b-0 border-black/10 dark:border-white/10 whitespace-nowrap"
    )}>
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

      <td className="px-3 py-2">
        {props.model.formattedDebtRemainingAmount}
      </td>

      <td className="px-3 py-2 text-end">
        <Link className="button small aspect-square w-fit p-2.5" to={props.model.detailRoute}>
          <InformationCircleIcon className="size-4" />
        </Link>
      </td>
    </tr>
  );
}