import { useAuthenticationApi, type AuthenticationApi } from "./authenticationApi";
import { useCustomerApi, type CustomerApi } from "./customerApi";
import { usePhotoApi, type PhotoApi } from "./photoApi";
import { useProductApi, type ProductApi } from "./productApi";
import { useUserApi, type UserApi } from "./userApi";
import { useMetadataApi, type MetadataApi } from "./metadataApi";

interface IApi {
  authentication: AuthenticationApi;
  customer: CustomerApi;
  product: ProductApi;
  user: UserApi;
  photo: PhotoApi;
  metadata: MetadataApi;
}

export function useApi(): IApi {
  return {
    authentication: useAuthenticationApi(),
    customer: useCustomerApi(),
    product: useProductApi(),
    user: useUserApi(),
    photo: usePhotoApi(),
    metadata: useMetadataApi()
  };
}

export * from "./errors";