import { useApi, AuthenticationError } from "@/api";

export type AuthenticationStore = {
  isAuthenticated: boolean;
};

const initialIsAuthenticated = await isAuthenticatedAsync();

export function useAuthenticationStore() {
  return createAuthenticationStoreAsync();
}

function createAuthenticationStoreAsync(): AuthenticationStore {
  const isAuthenticated = $state(initialIsAuthenticated);
  return { isAuthenticated };
}

async function isAuthenticatedAsync(): Promise<boolean> {
  const api = useApi();
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