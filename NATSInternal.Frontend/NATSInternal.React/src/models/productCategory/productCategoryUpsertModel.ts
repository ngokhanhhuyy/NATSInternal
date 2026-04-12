import { useRouteHelper } from "@/helpers";

declare global {
  type ProductCategoryUpsertModel = Readonly<{
    id: string;
    name: string;
    get detailRoutePath(): string;
    toUpdateRequestDto(): ProductCategoryUpdateRequestDto;
  }>;
}

const { getProductCategoryDetailRoutePath } = useRouteHelper();

export function createProductCategoryUpsertModel(responseDto?: ProductCategoryGetDetailResponseDto): ProductCategoryUpsertModel {
  return {
    id: responseDto?.id ?? "",
    name: responseDto?.name ?? "",
    get detailRoutePath(): string {
      return getProductCategoryDetailRoutePath(this.id);
    },
    toUpdateRequestDto(): ProductCategoryUpdateRequestDto {
      return {
        name: this.name
      };
    }
  };
}