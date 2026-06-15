import { getProductCategoryDetailRoutePath } from "@/helpers";

declare global {
  type ProductCategoryBasicModel = {
    id: number;
    name: string;
    productCount: number | null;
    detailRoutePath: string;
  };
}

function createProductCategoryBasicModel(responseDto: ProductCategoryBasicResponseDto): ProductCategoryBasicModel {
  return {
    id: responseDto.id,
    name: responseDto.name,
    productCount: responseDto.productCount,
    detailRoutePath: getProductCategoryDetailRoutePath(responseDto.id)
  };
}

export { createProductCategoryBasicModel };
