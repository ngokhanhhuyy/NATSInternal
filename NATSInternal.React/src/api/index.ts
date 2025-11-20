import { useAuthenticationApi, type AuthenticationApi } from "./authenticationApi";
import { useCustomerApi, type CustomerApi } from "./customerApi";
import { useUserApi, type UserApi } from "./userApi";
import { useMetadataApi, type MetadataApi } from "./metadataApi";

interface IApi {
  authentication: AuthenticationApi;
  customer: CustomerApi,
  user: UserApi;
  metadata: MetadataApi;
}

export function useApi(): IApi {
  return {
    authentication: useAuthenticationApi(),
    customer: useCustomerApi(),
    user: useUserApi(),
    metadata: useMetadataApi()
  };
}

export * from "./errors";