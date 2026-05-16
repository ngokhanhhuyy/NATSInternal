import { createProductCategoryBasicModel } from "@/models";
import { currencyHelper, routeHelper } from "@/helpers";

declare global {
  type ProductBasicModel = {
    id: number;
    name: string;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    isResupplyNeeded: boolean;
    isDiscontinued: boolean;
    thumbnailUrl: string | null;
    authorization: ProductExistingAuthorizationResponseDto | null;
    categories: ProductCategoryBasicModel[];
    formattedDefaultAmountBeforeVatPerUnit: string;
    detailRoutePath: string;
  };
}

export function createProductBasicModel(responseDto: ProductBasicResponseDto): ProductBasicModel {
  return {
    ...responseDto,
    thumbnailUrl: responseDto.thumbnailUrl,
    formattedDefaultAmountBeforeVatPerUnit: currencyHelper
      .getAmountDisplayText(responseDto.defaultAmountBeforeVatPerUnit),
    categories: responseDto.categories.map(createProductCategoryBasicModel),
    detailRoutePath: routeHelper.getProductDetailRoutePath(responseDto.id)
  };
}
