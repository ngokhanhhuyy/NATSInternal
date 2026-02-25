import { useAuthenticationApi, type AuthenticationApi } from "./authenticationApi";
import { useCustomerApi, type CustomerApi } from "./customerApi";
import { usePhotoApi, type PhotoApi } from "./photoApi";
import { useProductApi, type ProductApi } from "./productApi";
import { useBrandApi, type BrandApi } from "./brandApi";
import { useProductCategoryApi, type ProductCategoryApi } from "./productCateoryApi";
import { useCountryApi, type CountryApi } from "./countryApi";
import { useUserApi, type UserApi } from "./userApi";
import { useMetadataApi, type MetadataApi } from "./metadataApi";

interface IApi {
  authentication: AuthenticationApi;
  customer: CustomerApi;
  product: ProductApi;
  brand: BrandApi;
  productCategory: ProductCategoryApi;
  country: CountryApi;
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
    country: useCountryApi(),
    user: useUserApi(),
    photo: usePhotoApi(),
    metadata: useMetadataApi()
  };
}

export * from "./errors";