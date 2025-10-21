import { create } from "zustand";
import { useApi } from "@/api";

export type AuthenticationStore = {
  hasInitiallyCheckedAuthentication: boolean;
  isAuthenticated: boolean;
  readonly isAuthenticatedAsync: () => Promise<boolean>;
  readonly clearAuthenticationStatus: () => void;
};

const api = useApi();

export const useAuthenticationStore = create<AuthenticationStore>((set, get) => ({
  hasInitiallyCheckedAuthentication: false,
  isAuthenticated: false,
  isAuthenticatedAsync: async (): Promise<boolean> => {
    const { hasInitiallyCheckedAuthentication } = get();

    if (!hasInitiallyCheckedAuthentication) {
      try {
        const isAuthenticated = await api.authentication.checkAuthenticationStatusAsync();
        set({
          isAuthenticated,
          hasInitiallyCheckedAuthentication: true,
        });
      } catch {
        set({
          isAuthenticated: false,
          hasInitiallyCheckedAuthentication: true,
        });
      }
    }

    return get().isAuthenticated;
  },
  clearAuthenticationStatus: (): void => {
    set({
      hasInitiallyCheckedAuthentication: false,
      isAuthenticated: false,
    });
  },
}));