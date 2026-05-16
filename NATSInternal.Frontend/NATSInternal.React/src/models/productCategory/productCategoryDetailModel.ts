import { createProductCategoryBasicModel } from "@/models";
import { getProductCategoryUpdateRoutePath } from "@/helpers";

declare global {
  type ProductCategoryDetailModel = Readonly<{
    id: number;
    name: string;
    authorization: ProductCategoryExistingAuthorizationResponseDto | null;
    updateRoutePath: string;
    toBasicModel(): ProductCategoryBasicModel;
  }>;
}

export function createProductCategoryDetailModel(responseDto: ProductCategoryDetailResponseDto): ProductCategoryDetailModel {
  return {
    id: responseDto.id,
    name: responseDto.name,
    authorization: responseDto.authorization,
    updateRoutePath: getProductCategoryUpdateRoutePath(responseDto.id),
    toBasicModel(): ProductCategoryBasicModel {
      return createProductCategoryBasicModel(this);
    }
  };
}
