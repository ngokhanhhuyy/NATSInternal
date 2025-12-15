import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App.tsx";
import "bootstrap-icons/font/bootstrap-icons.css";

createRoot(document.getElementById("root") as HTMLDivElement).render(
  <StrictMode>
    <App/>
  </StrictMode>,
);
