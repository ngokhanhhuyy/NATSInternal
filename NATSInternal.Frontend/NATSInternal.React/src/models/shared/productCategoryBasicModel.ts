import { useRouteHelper } from "@/helpers";

declare global {
  type ProductCategoryBasicModel = {
    id: string;
    name: string;
    detailRoutePath: string;
  };
}

const { getProductCategoryDetailRoutePath } = useRouteHelper();

function createProductCategoryBasicModel(responseDto: ProductCategoryBasicResponseDto): ProductCategoryBasicModel {
  return {
    id: responseDto.id,
    name: responseDto.name,
    detailRoutePath: getProductCategoryDetailRoutePath(responseDto.id)
  };
}

export { createProductCategoryBasicModel };