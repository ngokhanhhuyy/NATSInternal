import { useAuthenticationApi, type AuthenticationApi } from "./authenticationApi";
import { useCustomerApi, type CustomerApi } from "./customerApi";
import { usePhotoApi, type PhotoApi } from "./photoApi";
import { useUserApi, type UserApi } from "./userApi";
import { useMetadataApi, type MetadataApi } from "./metadataApi";

interface IApi {
  authentication: AuthenticationApi;
  customer: CustomerApi;
  user: UserApi;
  photo: PhotoApi;
  metadata: MetadataApi;
}

export function useApi(): IApi {
  return {
    authentication: useAuthenticationApi(),
    customer: useCustomerApi(),
    user: useUserApi(),
    photo: usePhotoApi(),
    metadata: useMetadataApi()
  };
}

export * from "./errors";
