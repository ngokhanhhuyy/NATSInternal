import { createPhotoBasicModel, createProductCategoryBasicModel, createUserBasicModel } from "@/models";
import { getDisplayDateTimeString, getProductUpdateRoutePath, getAmountDisplayText } from "@/helpers";

declare global {
  type ProductDetailModel = Readonly<{
    id: number;
    name: string;
    description: string | null;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    resupplyThresholdQuantity: number;
    isForRetail: boolean;
    isDiscontinued: boolean;
    createdDateTime: string;
    createdUser: UserBasicModel;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicModel | null;
    deletedUser: UserBasicModel | null;
    deletedDateTime: string | null;
    categories: ProductCategoryBasicModel[];
    photos: PhotoBasicModel[];
    authorization: ProductExistingAuthorizationResponseDto;
    formattedDefaultAmountBeforeVatPerUnit: string;
    updateRoutePath: string;
  }>;
}

export function createProductDetailModel(responseDto: ProductDetailResponseDto): ProductDetailModel {
  return {
    id: responseDto.id,
    name: responseDto.name,
    description: responseDto.description,
    unit: responseDto.unit,
    defaultAmountBeforeVatPerUnit: responseDto.defaultAmountBeforeVatPerUnit,
    defaultVatPercentagePerUnit: responseDto.defaultVatPercentagePerUnit,
    stockingQuantity: responseDto.stockingQuantity,
    resupplyThresholdQuantity: responseDto.resupplyThresholdQuantity,
    isForRetail: responseDto.isForRetail,
    isDiscontinued: responseDto.isDiscontinued,
    createdDateTime: getDisplayDateTimeString(responseDto.createdDateTime),
    createdUser: createUserBasicModel(responseDto.createdUser),
    lastUpdatedDateTime: responseDto.lastUpdatedDateTime &&
      getDisplayDateTimeString(responseDto.lastUpdatedDateTime),
    lastUpdatedUser: responseDto.lastUpdatedUser && createUserBasicModel(responseDto.lastUpdatedUser),
    deletedDateTime: responseDto.deletedDateTime && getDisplayDateTimeString(responseDto.deletedDateTime),
    deletedUser: responseDto.deletedUser && createUserBasicModel(responseDto.deletedUser),
    categories: responseDto.categories.map(createProductCategoryBasicModel),
    photos: responseDto.photos.map(dto => createPhotoBasicModel(dto)),
    formattedDefaultAmountBeforeVatPerUnit: getAmountDisplayText(responseDto.defaultAmountBeforeVatPerUnit),
    updateRoutePath: getProductUpdateRoutePath(responseDto.id),
    authorization: responseDto.authorization
  };
}
