import { getDefaultAvatarUrlByFullName, getCustomerDetailRoutePath } from "@/helpers";

declare global {
  type CustomerBasicModel = Readonly<{
    id: number;
    fullName: string;
    nickName: string | null;
    isDeleted: boolean;
    avatarUrl: string;
    detailRoutePath: string;
  }>;
}

export function createCustomerBasicModel(responseDto: CustomerBasicResponseDto): CustomerBasicModel {
  return {
    id: responseDto.id,
    fullName: responseDto.fullName,
    nickName: responseDto.nickName,
    isDeleted: responseDto.isDeleted,
    avatarUrl: getDefaultAvatarUrlByFullName(responseDto.fullName),
    detailRoutePath: getCustomerDetailRoutePath(responseDto.id)
  };
}
