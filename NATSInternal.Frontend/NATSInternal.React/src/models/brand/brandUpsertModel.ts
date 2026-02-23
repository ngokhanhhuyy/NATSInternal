import { createCountryBasicModel } from "../shared/countryBasicModel";

declare global {
  type BrandUpsertModel = Readonly<{
    name: string;
    website: string | null;
    socialMediaUrl: string | null;
    phoneNumber: string | null;
    email: string | null;
    address: string | null;
    country: CountryBasicModel | null;
    toRequestDto(): BrandUpsertRequestDto;
  }>;
}

export function createBrandUpsertModel(responseDto?: BrandGetDetailResponseDto): BrandUpsertModel {
  return {
    name: responseDto?.name ?? "",
    website: responseDto?.website ?? "",
    socialMediaUrl: responseDto?.socialMediaUrl ?? "",
    phoneNumber: responseDto?.phoneNumber ?? "",
    email: responseDto?.email ?? "",
    address: responseDto?.address ?? "",
    country: responseDto?.country ? createCountryBasicModel(responseDto.country) : null,
    toRequestDto(): BrandUpsertRequestDto {
      return {
        name: this.name,
        website: this.website || null,
        socialMediaUrl: this.socialMediaUrl || null,
        phoneNumber: this.phoneNumber || null,
        email: this.email || null,
        address: this.address || null,
        countryId: this.country?.id ?? null
      };
    }
  };
}