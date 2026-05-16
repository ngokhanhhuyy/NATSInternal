import { getProductCategoryDetailRoutePath } from "@/helpers";

type DetailResponseDto = ProductCategoryDetailResponseDto;

declare global {
  type ProductCategoryUpsertModel = Readonly<{
    id: number;
    name: string;
    get detailRoutePath(): string;
    toUpdateRequestDto(): ProductCategoryUpsertRequestDto;
  }>;
}

export function createProductCategoryUpsertModel(responseDto?: DetailResponseDto): ProductCategoryUpsertModel {
  return {
    id: responseDto?.id ?? 0,
    name: responseDto?.name ?? "",
    get detailRoutePath(): string {
      return getProductCategoryDetailRoutePath(this.id);
    },
    toUpdateRequestDto(): ProductCategoryUpsertRequestDto {
      return {
        name: this.name
      };
    }
  };
}
