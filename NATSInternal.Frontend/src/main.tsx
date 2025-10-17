import { render } from "solid-js/web";
import "@/assets/main.css";
import "bootstrap";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-icons/font/bootstrap-icons.css";
import App from "./App";

const root = document.getElementById("root");
if (root) {
  render(() => <App />, root);
}