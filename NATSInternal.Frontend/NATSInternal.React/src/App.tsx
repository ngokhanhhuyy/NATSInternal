import React, { useEffect } from "react";

import Router from "./router";

export default function App(): React.ReactNode {
  // Effect (as a solution for performance issue on Firefox + Windows).
  useEffect(() => {
    const interval = setInterval(() => {
      // eslint-disable-next-line
      document.body.offsetHeight;
    }, 1000);

    return () => {
      clearInterval(interval);
    };
  }, []);

  // Template.
  return <Router />;
}
