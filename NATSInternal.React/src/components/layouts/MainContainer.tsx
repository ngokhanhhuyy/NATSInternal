import React from "react";
import styles from "./MainContainer.module.css";

// Component.
export default function MainContainer(props: React.ComponentPropsWithoutRef<"div">): React.ReactNode {
  return <div className={styles.mainContainer}>{props.children}</div>;
}