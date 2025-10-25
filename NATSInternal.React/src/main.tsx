import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App.tsx";
import "./style.css";

createRoot(document.getElementById("root") as HTMLDivElement).render(
  <StrictMode>
    <App/>
  </StrictMode>,
);
