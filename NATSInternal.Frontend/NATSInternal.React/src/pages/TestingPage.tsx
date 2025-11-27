import { useCallback, useId } from "react";
import { useNavigate } from "react-router";
import { useApi } from "@/api";
import { useAuthenticationStore } from "@/stores";
import { useRouteHelper } from "@/helpers";

// Child components.
import MainContainer from "@/components/layouts/MainContainer";
import { Button } from "@/components/ui";
import * as form from "@/components/form";

// Component.
export default function TestingPage() {
  // Dependencies.
  const navigate = useNavigate();
  const setIsAuthenticated = useAuthenticationStore(store => store.setIsAuthenticated);
  const api = useApi();
  const id = useId();
  const { getSignInRoutePath } = useRouteHelper();

  // Callbacks.
  const signOut = useCallback(async () => {
    await api.authentication.clearAccessCookieAsync();
    setIsAuthenticated(false);
    navigate(getSignInRoutePath());
  }, [setIsAuthenticated]);

  // Template.
  return (
    <MainContainer>
      {id}
      <Button onClick={signOut}>Đăng xuất</Button>
      <div className="flex gap-3 my-3 flex-wrap">
        {variants.map((variant, index) => <Button variant={variant} key={index}>{variant.toUpperCase()}</Button>)}
      </div>

      <div className="flex flex-col flex-wrap gap-3 my-3">
        <form.FormField>
          <form.TextInput placeholder="Some value" value="" onValueChanged={() => { }} />
        </form.FormField>
      </div>
      {Array.from({ length: 10000 }, (_, index) => index).map((index) => index + "_abcxyz").join(" ")}
    </MainContainer>
  );
}

const variants: ColorVariant[] = ["primary", "secondary", "danger", "success", "hinting"];