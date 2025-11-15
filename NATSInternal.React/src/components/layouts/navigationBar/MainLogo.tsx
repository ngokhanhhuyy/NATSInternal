import React from "react";
import { Link } from "react-router";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Child component.
import { StarIcon as ApplicationIcon } from "@heroicons/react/24/solid";

// Component.
export default function MainLogo(): React.ReactNode {
  // Dependencies.
  const { getHomeRoutePath } = useRouteHelper();
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <Link
      className={joinClassName(
        "flex gap-1.5 md:gap-2.5 justify-center items-center",
        "hover:opacity-100 hover:no-underline",
      )}
      to={getHomeRoutePath()}
    >
      <ApplicationIcon className={joinClassName(
        "bg-success/10 border border-success size-10 p-1.5",
        "rounded-[50%] fill-success shrink-0",
      )} />

      <span className="text-success font-light text-2xl block md:hidden lg:block">
        natsinternal
      </span>
    </Link>
  );
}