import { useRouteHelper } from "@/helpers";

declare global {
  type ProductCategoryUpsertModel = Readonly<{
    id: string;
    name: string;
    website: string;
    socialMediaUrl: string;
    phoneNumber: string;
    email: string;
    address: string;
    country: CountryBasicModel | null;
    get detailRoutePath(): string;
    toUpdateRequestDto(): ProductCategoryUpdateRequestDto;
  }>;
}

const { getProductCategoryDetailRoutePath } = useRouteHelper();

export function createProductCategoryUpsertModel(responseDto?: ProductCategoryGetDetailResponseDto): ProductCategoryUpsertModel {
  return {
    id: responseDto?.id ?? "",
    name: responseDto?.name ?? "",
    website: responseDto?.website ?? "",
    socialMediaUrl: responseDto?.socialMediaUrl ?? "",
    phoneNumber: responseDto?.phoneNumber ?? "",
    email: responseDto?.email ?? "",
    address: responseDto?.address ?? "",
    country: responseDto?.country ? createCountryBasicModel(responseDto.country) : null,
    get detailRoutePath(): string {
      return getProductCategoryDetailRoutePath(this.id);
    },
    toCreateRequestDto(): ProductCategoryCreateRequestDto {
      return createUpsertRequestDto(this);
    },
    toUpdateRequestDto(): ProductCategoryUpdateRequestDto {
      return createUpsertRequestDto(this);
    }
  };
}

function createUpsertRequestDto(model: ProductCategoryUpsertModel): ProductCategoryUpsertRequestDto {
  return {
    name: model.name,
    website: model.website || null,
    socialMediaUrl: model.socialMediaUrl || null,
    phoneNumber: model.phoneNumber || null,
    email: model.email || null,
    address: model.address || null,
    countryId: model.country?.id ?? null
  };
}