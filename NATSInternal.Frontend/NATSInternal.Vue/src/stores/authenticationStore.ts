import { ref } from "vue";
import { defineStore } from "pinia";
import { useApi, AuthenticationError } from "@/api";

const api = useApi();
const initialIsAuthenticated = await isAuthenticatedAsync();

export const useAuthenticationStore = defineStore("authenticationStore", () => {
  const isAuthenticated = ref<boolean>(initialIsAuthenticated);
  return { isAuthenticated };
});

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