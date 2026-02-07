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

      <span>
        natsinternal
      </span>
    </Link>
  );
}