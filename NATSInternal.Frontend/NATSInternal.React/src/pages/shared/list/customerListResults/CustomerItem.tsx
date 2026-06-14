import React from "react";
import { Link } from "react-router";
import { joinClassName, compute } from "@/helpers";

// Child components.
import { NewTabWebsiteLink } from "@/components/ui";
import DebtAlert from "@/pages/shared/alerts/DebtAlert";
import { UserIcon } from "@heroicons/react/24/outline";

// Props.
type CustomerItemProps = {
  className?: string;
  model: CustomerBasicModel;
  openLinkInNewTab?: boolean;
  children?: React.ReactNode;
};

// Components.
export default function CustomerItem(props: CustomerItemProps): React.ReactNode {
  // Computed.
  const linkClassName = compute<string>(() => {
    const names: string[] = ["font-bold", "whitespace-nowrap"];
    if (props.model.debtAmount === 0) {
      names.push("text-blue-700 dark:text-blue-400");
    } else if (props.model.debtAmount > 0) {;
      names.push("text-yellow-600 dark:text-yellow-400");
    } else {
      names.push("text-red-600 dark:text-red-400");
    }

    return names.join(" ");
  });

  // Template.
  return (
    <li className={joinClassName("list-group-item items-center p-2", props.className)}>
      <div className={joinClassName(
        "grid gap-3 justify-center items-start",
        props.children ? "grid-cols-[auto_1fr_auto]" : "grid-cols-[auto_1fr]"
      )}>
        <div className="img-thumbnail size-12 flex justify-center items-center">
          <UserIcon className="size-6 opacity-50" />
        </div>

        <div className="flex flex-col items-start">
          <div className="flex flex-wrap gap-x-2 items-center">
            {props.openLinkInNewTab ? (
              <NewTabWebsiteLink href={props.model.detailRoutePath} className={linkClassName}>
                {props.model.fullName}
              </NewTabWebsiteLink>
            ) : (
              <Link to={props.model.detailRoutePath} className={linkClassName}>
                {props.model.fullName}
              </Link>
            )}

            <DebtAlert className="alert-sm" debtAmount={props.model.debtAmount} />
          </div>

          <span className="text-sm opacity-50">
            {props.model.nickName}
          </span>
        </div>

        {props.children}
      </div>
    </li>
  );
}
