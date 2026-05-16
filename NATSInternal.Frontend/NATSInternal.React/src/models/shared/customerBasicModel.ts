import { avatarHelper, routeHelper } from "@/helpers";

declare global {
  type CustomerBasicModel = Readonly<{
    id: number;
    fullName: string;
    nickName: string | null;
    isDeleted: boolean;
    avatarUrl: string;
    detailRoute: string;
  }>;
}

const { getDefaultAvatarUrlByFullName } = avatarHelper;
const { getCustomerDetailRoutePath } = routeHelper;

export function createCustomerBasicModel(responseDto: CustomerBasicResponseDto): CustomerBasicModel {
  return {
    id: responseDto.id,
    fullName: responseDto.fullName,
    nickName: responseDto.nickName,
    isDeleted: responseDto.isDeleted,
    avatarUrl: getDefaultAvatarUrlByFullName(responseDto.fullName),
    detailRoute: getCustomerDetailRoutePath(responseDto.id)
  };
}
