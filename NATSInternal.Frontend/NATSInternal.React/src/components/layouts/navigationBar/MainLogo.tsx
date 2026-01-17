import React from "react";
import { Link } from "react-router";
import { useRouteHelper } from "@/helpers";

// Child component.
import { StarIcon as ApplicationIcon } from "@heroicons/react/24/solid";

// Component.
export default function MainLogo(): React.ReactNode {
  // Dependencies.
  const { getHomeRoutePath } = useRouteHelper();

  // Template.
  return (
    <Link id="main-logo" to={getHomeRoutePath()}>
      <ApplicationIcon />

      <span className="text-emerald-500 dark:text-emerald-400 font-light text-2xl block">
        natsinternal
      </span>
    </Link>
  );
}