import { getProductCategoryDetailRoutePath } from "@/helpers";

declare global {
  type ProductCategoryBasicModel = {
    id: number;
    name: string;
    detailRoutePath: string;
  };
}

function createProductCategoryBasicModel(responseDto: ProductCategoryBasicResponseDto): ProductCategoryBasicModel {
  return {
    id: responseDto.id,
    name: responseDto.name,
    detailRoutePath: getProductCategoryDetailRoutePath(responseDto.id)
  };
}

export { createProductCategoryBasicModel };
