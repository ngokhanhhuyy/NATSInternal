import { createState } from "@/hooks";
import { useApi } from "@/api";

const api = useApi();

export type AuthenticationStore = {
  hasInitiallyCheckedAuthentication: boolean;
  isAuthenticated: boolean;
  readonly isAuthenticatedAsync: () => Promise<boolean>;
  readonly clearAuthenticationStatus: () => void;
};

const state = createState({
  hasInitiallyCheckedAuthentication: false,
  isAuthenticated: false
});

function useAuthenticationStore(): AuthenticationStore {
  return {
    get hasInitiallyCheckedAuthentication() {
      return state.hasInitiallyCheckedAuthentication;
    },
    set hasInitiallyCheckedAuthentication(hasChecked: boolean) {
      state.$update({ hasInitiallyCheckedAuthentication: hasChecked });
    },
    get isAuthenticated(): boolean {
      return state.isAuthenticated;
    },
    set isAuthenticated(authenticated: boolean) {
      state.$update({ isAuthenticated: authenticated });
    },
    isAuthenticatedAsync: async (): Promise<boolean> => {
      if (!state.hasInitiallyCheckedAuthentication) {
        try {
          const isAuthenticated = await api.authentication.checkAuthenticationStatusAsync();
          state.$update({
            isAuthenticated,
            hasInitiallyCheckedAuthentication: true,
          });
        } catch {
          state.$update({
            isAuthenticated: false,
            hasInitiallyCheckedAuthentication: true,
          });
        }
      }

      return state.isAuthenticated;
    },
    clearAuthenticationStatus: (): void => {
      state.$update({
        hasInitiallyCheckedAuthentication: false,
        isAuthenticated: false,
      });
    },
  };
}

export { useAuthenticationStore };