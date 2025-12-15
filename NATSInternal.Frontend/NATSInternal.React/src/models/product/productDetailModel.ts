import {
  createBrandBasicModel,
  createPhotoBasicModel,
  createProductCategoryBasicModel,
  createUserBasicModel
} from "@/models";
import { useCurrencyHelper, useDateTimeHelper, useRouteHelper } from "@/helpers";

declare global {
  type ProductDetailModel = Readonly<{
    id: string;
    name: string;
    description: string | null;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    isForRetail: boolean;
    isDiscontinued: boolean;
    createdDateTime: string;
    createdUser: UserBasicModel;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicModel | null;
    category: ProductCategoryBasicModel | null;
    brand: BrandBasicModel | null;
    photos: PhotoBasicModel[];
    authorization: ProductExistingAuthorizationResponseDto;
    formattedDefaultAmountBeforeVatPerUnit: string;
    updateRoutePath: string;
  }>;
}

const { getAmountDisplayText } = useCurrencyHelper();
const { getDisplayDateTimeString } = useDateTimeHelper();
const { getProductUpdateRoutePath } = useRouteHelper();

export function createProductDetailModel(responseDto: ProductGetDetailResponseDto): ProductDetailModel {
  return {
    id: responseDto.id,
    name: responseDto.name,
    description: responseDto.description,
    unit: responseDto.unit,
    defaultAmountBeforeVatPerUnit: responseDto.defaultAmountBeforeVatPerUnit,
    defaultVatPercentagePerUnit: responseDto.defaultVatPercentagePerUnit,
    stockingQuantity: responseDto.stockingQuantity,
    isForRetail: responseDto.isForRetail,
    isDiscontinued: responseDto.isDiscontinued,
    createdDateTime: getDisplayDateTimeString(responseDto.createdDateTime),
    createdUser: createUserBasicModel(responseDto.createdUser),
    lastUpdatedDateTime: responseDto.lastUpdatedDateTime && getDisplayDateTimeString(responseDto.lastUpdatedDateTime),
    lastUpdatedUser: responseDto.lastUpdatedUser && createUserBasicModel(responseDto.lastUpdatedUser),
    category: responseDto.category && createProductCategoryBasicModel(responseDto.category),
    brand: responseDto.brand && createBrandBasicModel(responseDto.brand),
    photos: responseDto.photos.map(dto => createPhotoBasicModel(dto)),
    formattedDefaultAmountBeforeVatPerUnit: getAmountDisplayText(responseDto.defaultAmountBeforeVatPerUnit),
    updateRoutePath: getProductUpdateRoutePath(responseDto.id),
    authorization: responseDto.authorization
  };
}