import { createProductCategoryBasicModel, createPhotoUpsertModel } from "@/models";
import { getProductDetailRoutePath } from "@/helpers";

declare global {
  type ProductUpsertModel = {
    id: number;
    name: string;
    description: string;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    resupplyThresholdQuantity: number;
    isForRetail: boolean;
    isDiscontinued: boolean;
    categories: ProductCategoryBasicModel[];
    photos: PhotoUpsertModel[];
    get detailRoutePath(): string;
    toCreateRequestDto(): ProductCreateRequestDto;
    toUpdateRequestDto(): ProductUpdateRequestDto;
  };
}

export function createProductUpsertModel(responseDto?: ProductDetailResponseDto): ProductUpsertModel {
  return {
    id: responseDto?.id ?? 0,
    name: responseDto?.name ?? "",
    description: responseDto?.description ?? "",
    unit: responseDto?.unit ?? "",
    defaultAmountBeforeVatPerUnit: responseDto?.defaultAmountBeforeVatPerUnit ?? 0,
    defaultVatPercentagePerUnit: responseDto?.defaultVatPercentagePerUnit ?? 0,
    stockingQuantity: responseDto?.stockingQuantity ?? 0,
    resupplyThresholdQuantity: responseDto?.resupplyThresholdQuantity ?? 0,
    isForRetail: responseDto?.isForRetail ?? true,
    isDiscontinued: responseDto?.isDiscontinued ?? false,
    categories: responseDto?.categories.map(createProductCategoryBasicModel) ?? [],
    photos: responseDto?.photos.map(createPhotoUpsertModel) ?? [],
    get detailRoutePath(): string {
      return getProductDetailRoutePath(this.id);
    },
    toCreateRequestDto(): ProductCreateRequestDto {
      return createUpsertRequestDto(this);
    },
    toUpdateRequestDto(): ProductUpdateRequestDto {
      return {
        ...createUpsertRequestDto(this),
        isDiscontinued: this.isDiscontinued
      };
    }
  };
}

function createUpsertRequestDto(model: ProductUpsertModel): AbstractProductUpsertRequestDto {
  return {
    name: model.name,
    description: model.description,
    unit: model.unit,
    defaultAmountBeforeVatPerUnit: model.defaultAmountBeforeVatPerUnit,
    defaultVatPercentagePerUnit: model.defaultVatPercentagePerUnit,
    stockingQuantity: model.stockingQuantity,
    resupplyThresholdQuantity: model.resupplyThresholdQuantity || null,
    isForRetail: model.isForRetail,
    categoryIds: model.categories.map(pc => pc.id),
    photos: model.photos.map(photo => photo.toRequestDto())
  };
}
