import { useCallback } from "react";
import { useNavigate } from "react-router";
import { useApi } from "@/api";
import { useRouteHelper } from "@/helpers";


export default function HomePage() {
  // Dependencies.
  const navigate = useNavigate();
  const api = useApi();
  const { getSignInRoutePath } = useRouteHelper();

  // Callbacks.
  const signOut = useCallback(async () => {
    await api.authentication.clearAccessCookieAsync();
    navigate(getSignInRoutePath());
  }, []);

  return (
    <div className="bg-white" style={{ width: "100vw", height: "100vh" }}>
      <button onClick={signOut}>Đăng xuất</button>
    </div>
  );
}