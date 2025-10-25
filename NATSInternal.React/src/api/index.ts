import { useAuthenticationApi, type AuthenticationApi } from "./authenticationApi";
import { useUserApi, type UserApi } from "./userApi";
import { useMetadataApi, type MetadataApi } from "./metadataApi";

interface IApi {
  authentication: AuthenticationApi;
  user: UserApi;
  metadata: MetadataApi;
}

export function useApi(): IApi {
  return {
    authentication: useAuthenticationApi(),
    user: useUserApi(),
    metadata: useMetadataApi()
  };
}

export * from "./errors";