import {
  createBrandBasicModel,
  createPhotoBasicModel,
  createProductCategoryBasicModel,
  createStockBasicModel,
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
    isForRetail: boolean;
    isDiscontinued: boolean;
    createdDateTime: string;
    createdUser: UserBasicModel;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicModel | null;
    category: ProductCategoryBasicModel | null;
    stock: StockBasicModel;
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
    isForRetail: responseDto.isForRetail,
    isDiscontinued: responseDto.isDiscontinued,
    createdDateTime: getDisplayDateTimeString(responseDto.createdDateTime),
    createdUser: createUserBasicModel(responseDto.createdUser),
    lastUpdatedDateTime: responseDto.lastUpdatedDateTime && getDisplayDateTimeString(responseDto.lastUpdatedDateTime),
    lastUpdatedUser: responseDto.lastUpdatedUser && createUserBasicModel(responseDto.lastUpdatedUser),
    category: responseDto.category && createProductCategoryBasicModel(responseDto.category),
    brand: responseDto.brand && createBrandBasicModel(responseDto.brand),
    stock: createStockBasicModel(responseDto.stock),
    photos: responseDto.photos.map(dto => createPhotoBasicModel(dto)),
    formattedDefaultAmountBeforeVatPerUnit: getAmountDisplayText(responseDto.defaultAmountBeforeVatPerUnit),
    updateRoutePath: getProductUpdateRoutePath(responseDto.id),
    authorization: responseDto.authorization
  };
}