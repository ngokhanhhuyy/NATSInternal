import { useAuthenticationApi, type AuthenticationApi } from "./authenticationApi";

interface IApi {
  authentication: AuthenticationApi;
}

export function useApi(): IApi {
  return {
    authentication: useAuthenticationApi()
  };
}

export * from "./dtos";
export * from "./errors";