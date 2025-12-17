import { createBrandBasicModel, createProductCategoryBasicModel, createPhotoCreateOrUpdateModel } from "@/models";

declare global {
  type ProductCreateModel = ProductUpsertModel<ProductCreateRequestDto> & { toRequestDto(): ProductCreateRequestDto };
  type ProductUpdateModel = ProductUpsertModel<ProductUpdateRequestDto> & {
    id: string;
    isDiscontinued: boolean;
    toRequestDto(): ProductCreateRequestDto;
  };
}

function createProductUpsertModel<TRequestDto extends AbstractProductUpsertRequestDto>
    (responseDto?: ProductGetDetailResponseDto): ProductUpsertModel<TRequestDto> {
  return {
    name: responseDto?.name ?? "",
    description: responseDto?.description ?? "",
    unit: responseDto?.unit ?? "",
    defaultAmountBeforeVatPerUnit: responseDto?.defaultAmountBeforeVatPerUnit ?? 0,
    defaultVatPercentagePerUnit: responseDto?.defaultVatPercentagePerUnit ?? 0,
    isForRetail: responseDto?.isForRetail ?? true,
    brand: (responseDto?.brand && createBrandBasicModel(responseDto.brand)) ?? null,
    category: (responseDto?.category && createProductCategoryBasicModel(responseDto.category)) ?? null,
    photos: responseDto?.photos.map(dto => createPhotoCreateOrUpdateModel(dto)) ?? [],
    toRequestDto(): TRequestDto {
      return {
        
      }
    }
  };
}

export function createPhotoCreateModel(): ProductCreateModel {
  return createProductUpsertModel();
}

export function createPhotoUpdateModel(responseDto: ProductGetDetailResponseDto): ProductUpdateModel {
  return {
    ...createProductUpsertModel(responseDto),
    id: responseDto.id,
    isDiscontinued: responseDto.isDiscontinued,
    toRequestDto(): ProductCreateRequestDto {
      return {
        id
      }
    }
  };
}

type ProductUpsertModel<TRequestDto extends AbstractProductUpsertRequestDto> = {
  name: string;
  description: string;
  unit: string;
  defaultAmountBeforeVatPerUnit: number;
  defaultVatPercentagePerUnit: number;
  isForRetail: boolean;
  brand: BrandBasicModel | null;
  category: ProductCategoryBasicModel | null;
  photos: PhotoCreateOrUpdateModel[];
  toRequestDto(): TRequestDto;
};