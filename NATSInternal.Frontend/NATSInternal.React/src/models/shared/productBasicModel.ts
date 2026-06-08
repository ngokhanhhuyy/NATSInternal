import { createProductCategoryBasicModel } from "@/models";
import { getDisplayAmountText, getProductDetailRoutePath } from "@/helpers";

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

export function createProductBasicModel(arg: ProductDetailModel | ProductBasicResponseDto): ProductBasicModel {
  let isResupplyNeeded: boolean;
  let thumbnailUrl: string | null;
  let categories: ProductCategoryBasicModel[];
  if (isProductDetailModel(arg)) {
    isResupplyNeeded = arg.stockingQuantity <= arg.resupplyThresholdQuantity;
    thumbnailUrl = arg.photos.find(p => p.isThumbnail)?.url ?? null;
    categories = arg.categories;
  } else {
    isResupplyNeeded = arg.isResupplyNeeded;
    thumbnailUrl = arg.thumbnailUrl;
    categories = arg.categories.map(createProductCategoryBasicModel);
  }

  return {
    ...arg,
    isResupplyNeeded,
    thumbnailUrl,
    formattedDefaultAmountBeforeVatPerUnit: getDisplayAmountText(arg.defaultAmountBeforeVatPerUnit),
    categories,
    detailRoutePath: getProductDetailRoutePath(arg.id)
  };
}

function isProductDetailModel(arg: ProductDetailModel | ProductBasicResponseDto): arg is ProductDetailModel {
  const detailModelProperties: (keyof ProductDetailModel)[] = ["deletedDateTime", "deletedUser"];
  for (const property of detailModelProperties) {
    if (!Object.hasOwn(arg, property)) {
      return false;
    }
  }

  return true;
}
