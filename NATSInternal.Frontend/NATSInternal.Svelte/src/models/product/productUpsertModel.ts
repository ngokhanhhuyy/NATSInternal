import { createBrandBasicModel, createProductCategoryBasicModel, createPhotoCreateOrUpdateModel } from "@/models";
import { useRouteHelper } from "@/helpers";

declare global {
  type ProductUpsertModel = {
    id: string;
    name: string;
    description: string;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    isForRetail: boolean;
    isDiscontinued: boolean;
    brand: BrandBasicModel | null;
    category: ProductCategoryBasicModel | null;
    stock: ProductUpsertStockModel;
    photos: PhotoCreateOrUpdateModel[];
    get detailRoutePath(): string;
    toCreateRequestDto(): ProductCreateRequestDto;
    toUpdateRequestDto(): ProductUpdateRequestDto;
  };

  type ProductUpsertStockModel = {
    stockingQuantity: number;
    resupplyThresholdQuantity: number;
  };
}

const { getProductDetailRoutePath } = useRouteHelper();

export function createProductUpsertModel(responseDto?: ProductGetDetailResponseDto): ProductUpsertModel {
  return {
    id: responseDto?.id ?? "",
    name: responseDto?.name ?? "",
    description: responseDto?.description ?? "",
    unit: responseDto?.unit ?? "",
    defaultAmountBeforeVatPerUnit: responseDto?.defaultAmountBeforeVatPerUnit ?? 0,
    defaultVatPercentagePerUnit: responseDto?.defaultVatPercentagePerUnit ?? 0,
    isForRetail: responseDto?.isForRetail ?? true,
    isDiscontinued: responseDto?.isDiscontinued ?? false,
    brand: (responseDto?.brand && createBrandBasicModel(responseDto.brand)) ?? null,
    category: (responseDto?.category && createProductCategoryBasicModel(responseDto.category)) ?? null,
    stock: createProductUpsertStockModel(responseDto?.stock),
    photos: responseDto?.photos.map(dto => createPhotoCreateOrUpdateModel(dto)) ?? [],
    get detailRoutePath(): string {
      return getProductDetailRoutePath(this.id);
    },
    toCreateRequestDto(): ProductCreateRequestDto {
      return createUpsertRequestDto(this);
    },
    toUpdateRequestDto(): ProductUpdateRequestDto {
      return {
        ...createUpsertRequestDto(this),
        id: this.id,
        isDiscontinued: this.isDiscontinued
      };
    }
  };
}

function createProductUpsertStockModel(responseDto?: StockBasicResponseDto): ProductUpsertStockModel {
  return {
    stockingQuantity: responseDto?.stockingQuantity ?? 0,
    resupplyThresholdQuantity: responseDto?.resupplyThresholdQuantity ?? 0
  };
}

function createUpsertRequestDto(model: ProductUpsertModel): AbstractProductUpsertRequestDto {
  return {
    name: model.name,
    description: model.description,
    unit: model.unit,
    defaultAmountBeforeVatPerUnit: model.defaultAmountBeforeVatPerUnit,
    defaultVatPercentagePerUnit: model.defaultVatPercentagePerUnit,
    isForRetail: model.isForRetail,
    brandId: model.brand?.id ?? null,
    categoryName: model.category?.name ?? null,
    photos: model.photos.map(photo => photo.toRequestDto())
  };
}