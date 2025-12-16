import { createBrandBasicModel, createProductCategoryBasicModel, createPhotoCreateOrUpdateModel } from "@/models";

declare global {
  type ProductCreateModel = ProductUpsertModel;
  type ProductUpdateModel = ProductUpsertModel & { id: string; isDiscontinued: boolean; };
}

function createProductUpsertModel(responseDto?: ProductGetDetailResponseDto): ProductUpsertModel {
  return {
    name: responseDto?.name ?? "",
    description: responseDto?.description ?? "",
    unit: responseDto?.unit ?? "",
    defaultAmountBeforeVatPerUnit: responseDto?.defaultAmountBeforeVatPerUnit ?? 0,
    defaultVatPercentagePerUnit: responseDto?.defaultVatPercentagePerUnit ?? 0,
    isForRetail: responseDto?.isForRetail ?? true,
    brand: (responseDto?.brand && createBrandBasicModel(responseDto.brand)) ?? null,
    category: (responseDto?.category && createProductCategoryBasicModel(responseDto.category)) ?? null,
    photos: responseDto?.photos.map(dto => createPhotoCreateOrUpdateModel(dto)) ?? []
  };
}

export function createPhotoCreateModel(): ProductCreateModel {
  return createProductUpsertModel();
}

export function createPhotoUpdateModel(responseDto: ProductGetDetailResponseDto): ProductUpdateModel {
  return {
    ...createProductUpsertModel(responseDto),
    id: responseDto.id,
    isDiscontinued: responseDto.isDiscontinued
  };
}

type ProductUpsertModel = {
  name: string;
  description: string;
  unit: string;
  defaultAmountBeforeVatPerUnit: number;
  defaultVatPercentagePerUnit: number;
  isForRetail: boolean;
  brand: BrandBasicModel | null;
  category: ProductCategoryBasicModel | null;
  photos: PhotoCreateOrUpdateModel[];
};