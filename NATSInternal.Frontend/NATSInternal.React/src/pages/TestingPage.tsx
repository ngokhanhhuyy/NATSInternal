import { useCallback } from "react";
import { useNavigate } from "react-router";
import { useApi } from "@/api";
import { useAuthenticationStore } from "@/stores";
import { useRouteHelper } from "@/helpers";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import { Button } from "@/components/ui";

// Component.
export default function TestingPage() {
  // Dependencies.
  const navigate = useNavigate();
  const setIsAuthenticated = useAuthenticationStore(store => store.setIsAuthenticated);
  const api = useApi();
  const { getSignInRoutePath } = useRouteHelper();

  // Callbacks.
  const signOut = useCallback(async () => {
    await api.authentication.clearAccessCookieAsync();
    setIsAuthenticated(false);
    navigate(getSignInRoutePath());
  }, [setIsAuthenticated]);

  // Template.
  return (
    <MainContainer description="Trang kiểm tra dành để kiểm tra các component.">
      <div className="panel">
        <div className="panel-header">
          <span className="panel-header-title">
            Testing
          </span>

          <Button className="btn-panel-header btn-sm" onClick={signOut}>
            Đăng xuất
          </Button>
        </div>

        <div className="panel-body flex flex-col gap-3 p-3">
          <div className="flex gap-3">
            <Button className="btn-primary">Primary</Button>
            <Button className="btn-primary-outline">Primary Outline</Button>
          </div>
          
          <div className="flex gap-3">
            <Button className="btn-danger">Danger</Button>
            <Button className="btn-danger-outline">Danger Outline</Button>
          </div>
          
          <div className="flex gap-3">
            <Button>Normal</Button>
          </div>
        </div>
      </div>

      <div className="panel">
        <div className="panel-header">
          <span className="panel-header-title">
            Paragraph
          </span>
        </div>

        <div className="panel-body flex flex-col gap-3 p-3">
          {Array.from({ length: 100 }, (_, index) => index).map((index) => index + "_abcxyz").join(" ")}
        </div>
      </div>
    </MainContainer>
  );
}