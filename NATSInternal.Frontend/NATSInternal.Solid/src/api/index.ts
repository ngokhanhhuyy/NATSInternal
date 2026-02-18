import { useAuthenticationApi, type AuthenticationApi } from "./authenticationApi";
import { useCustomerApi, type CustomerApi } from "./customerApi";
import { usePhotoApi, type PhotoApi } from "./photoApi";
import { useProductApi, type ProductApi } from "./productApi";
import { useBrandApi, type BrandApi } from "./brandApi";
import { useProductCategoryApi, type ProductCategoryApi } from "./productCateoryApi";
import { useUserApi, type UserApi } from "./userApi";
import { useMetadataApi, type MetadataApi } from "./metadataApi";

interface IApi {
  authentication: AuthenticationApi;
  customer: CustomerApi;
  product: ProductApi;
  brand: BrandApi;
  productCategory: ProductCategoryApi;
  user: UserApi;
  photo: PhotoApi;
  metadata: MetadataApi;
}

export function useApi(): IApi {
  return {
    authentication: useAuthenticationApi(),
    customer: useCustomerApi(),
    product: useProductApi(),
    brand: useBrandApi(),
    productCategory: useProductCategoryApi(),
    user: useUserApi(),
    photo: usePhotoApi(),
    metadata: useMetadataApi()
  };
}

export * from "./errors";