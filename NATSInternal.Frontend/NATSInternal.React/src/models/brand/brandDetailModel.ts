import { createUserBasicModel } from "../shared/userBasicModel";
import { createBrandBasicModel } from "../shared/brandBasicModel";
import { createCountryBasicModel } from "../shared/countryBasicModel";
import { useDateTimeHelper, useRouteHelper } from "@/helpers";

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
    createdUser: UserBasicModel;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicModel | null;
    country: CountryBasicModel | null;
    updateRoutePath: string;
    toBasicModel(): BrandBasicModel;
  }>;
}

const { getDisplayDateTimeString }  = useDateTimeHelper();
const { getBrandUpdateRoutePath } = useRouteHelper();

export function createBrandDetailModel(responseDto: BrandGetDetailResponseDto): BrandDetailModel {
  return {
    ...responseDto,
    website: responseDto.website?.replaceAll(/(,?)(\s+)/g, "-") ?? null,
    socialMediaUrl: responseDto.socialMediaUrl?.replaceAll(/(,?)(\s+)/g, "-")?? null,
    createdDateTime: getDisplayDateTimeString(responseDto.createdDateTime),
    createdUser: createUserBasicModel(responseDto.createdUser),
    lastUpdatedDateTime: responseDto.lastUpdatedDateTime && getDisplayDateTimeString(responseDto.lastUpdatedDateTime),
    lastUpdatedUser: responseDto.lastUpdatedUser && createUserBasicModel(responseDto.lastUpdatedUser),
    country: responseDto.country && createCountryBasicModel(responseDto.country),
    updateRoutePath: getBrandUpdateRoutePath(responseDto.id),
    toBasicModel(): BrandBasicModel {
      return createBrandBasicModel(this);
    }
  };
}