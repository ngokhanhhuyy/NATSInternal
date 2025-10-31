import React from "react";
import styles from "./RootLayout.module.css";

// Props.
type RootLayoutProps = Omit<React.ComponentPropsWithoutRef<"div">, "id">;

// Component.
export default function RootLayout(props: RootLayoutProps): React.ReactNode {
  return (
    <div id="root-layout" className={styles.rootLayout}>
      {props.children}
    </div>
  );
}