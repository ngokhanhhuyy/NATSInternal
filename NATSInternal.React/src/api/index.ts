import { type AuthenticationApi } from "./authenticationApi";
import { useMockAuthenticationApi } from "./mock/mockAuthenticationApi";
import { type UserApi } from "./userApi";
import { useMockUserApi } from "./mock/mockUserApi";

interface IApi {
  authentication: AuthenticationApi;
  user: UserApi;
}

export function useApi(): IApi {
  return {
    authentication: useMockAuthenticationApi(),
    user: useMockUserApi()
  };
}

export * from "./errors";