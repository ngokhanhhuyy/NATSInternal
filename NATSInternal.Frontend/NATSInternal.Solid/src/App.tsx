import { lazy } from "solid-js";
import { Router, Route } from "@solidjs/router";
import SignInPage from "./pages/authentication/signIn/SignInPage";

// Lazy components.
const HomePage = lazy(() => import("@/pages/home/HomePage"));

const App = () => {
  return (
    <Router>
      <Route path="/" component={() => <HomePage />}  />
      <Route path="/signIn" component={() => <SignInPage />} />
    </Router>
  );
};

export default App;
