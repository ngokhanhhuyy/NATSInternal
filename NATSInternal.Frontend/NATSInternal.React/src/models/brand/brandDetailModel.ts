import { createCountryBasicModel } from "../shared/countryBasicModel";

declare global {
  type BrandDetailModel = Readonly<{
    id: string;
    name: string;
    website: string | null;
    socialMediaUrl: string | null;
    phoneNumber: string | null;
    email: string | null;
    address: string | null;
    createdDateTime: string;
    country: CountryBasicModel | null;
  }>;
}

export function createBrandDetailModel(responseDto: BrandGetDetailResponseDto): BrandDetailModel {
  return {
    ...responseDto,
    country: responseDto.country && createCountryBasicModel(responseDto.country)
  };
}