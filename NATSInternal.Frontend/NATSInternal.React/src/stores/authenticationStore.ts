import { create } from "zustand";
import { useApi, AuthenticationError } from "@/api";

export type AuthenticationStore = {
  isAuthenticated: boolean;
  readonly setIsAuthenticated: (authenticated: boolean) => void;
};

const api = useApi();
const initialIsAuthenticated = await isAuthenticatedAsync();

export const useAuthenticationStore = create<AuthenticationStore>((set) => ({
  isAuthenticated: initialIsAuthenticated,
  setIsAuthenticated: (authenticated: boolean): void => {
    set({ isAuthenticated: authenticated });
  },
}));

async function isAuthenticatedAsync(): Promise<boolean> {
  try {
    await api.authentication.checkAuthenticationStatusAsync();
    return true;
  } catch (error) {
    if (error instanceof AuthenticationError) {
      return false;
    }

    throw error;
  }
}