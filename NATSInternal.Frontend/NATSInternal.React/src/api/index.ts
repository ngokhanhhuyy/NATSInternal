import { authenticationApi, type AuthenticationApi } from "./authenticationApi";
import { customerApi, type CustomerApi } from "./customerApi";
import { photoApi, type PhotoApi } from "./photoApi";
import { productApi, type ProductApi } from "./productApi";
import { productCategoryApi, type ProductCategoryApi } from "./productCateoryApi";
import { userApi, type UserApi } from "./userApi";
import { metadataApi, type MetadataApi } from "./metadataApi";

interface IApi {
  authentication: AuthenticationApi;
  customer: CustomerApi;
  product: ProductApi;
  productCategory: ProductCategoryApi;
  user: UserApi;
  photo: PhotoApi;
  metadata: MetadataApi;
}

export const api: IApi = {
  authentication: authenticationApi,
  customer: customerApi,
  product: productApi,
  productCategory: productCategoryApi,
  user: userApi,
  photo: photoApi,
  metadata: metadataApi
};

export * from "./errors";
