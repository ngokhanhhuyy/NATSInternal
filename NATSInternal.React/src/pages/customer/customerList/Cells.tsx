import React from "react";
import { useNavigate, Link } from "react-router";
import { useTsxHelper } from "@/helpers";

// Child components.
import { Button } from "@/components/ui";
import { InformationCircleIcon } from "@heroicons/react/24/outline";

// Props.
type CellsProps = { model: CustomerListCustomerModel };

// Component.
export default function CustomerCells(props: CellsProps): React.ReactNode {
  // Dependencies.
  const navigate = useNavigate();
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <>
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
        <Button className="aspect-square inline-block" onClick={() => navigate(props.model.detailRoute)}>
          <InformationCircleIcon className="size-4" />
        </Button>
      </td>
    </>
  );
}