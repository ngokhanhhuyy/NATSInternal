import { createCountryBasicModel } from "../shared/countryBasicModel";
import { useRouteHelper } from "@/helpers";

declare global {
  type BrandUpsertModel = Readonly<{
    id: string;
    name: string;
    website: string;
    socialMediaUrl: string;
    phoneNumber: string;
    email: string;
    address: string;
    country: CountryBasicModel | null;
    get detailRoutePath(): string;
    toCreateRequestDto(): BrandCreateRequestDto;
    toUpdateRequestDto(): BrandUpdateRequestDto;
  }>;
}

const { getBrandDetailRoutePath } = useRouteHelper();

export function createBrandUpsertModel(responseDto?: BrandGetDetailResponseDto): BrandUpsertModel {
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
      return getBrandDetailRoutePath(this.id);
    },
    toCreateRequestDto(): BrandCreateRequestDto {
      return createUpsertRequestDto(this);
    },
    toUpdateRequestDto(): BrandUpdateRequestDto {
      return createUpsertRequestDto(this);
    }
  };
}

function createUpsertRequestDto(model: BrandUpsertModel): BrandUpsertRequestDto {
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