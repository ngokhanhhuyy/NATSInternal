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

export function createCustomerBasicModel(arg: CustomerBasicResponseDto | CustomerDetailModel): CustomerBasicModel {
  return {
    id: arg.id,
    fullName: arg.fullName,
    nickName: arg.nickName,
    isDeleted: isCustomerDetailModel(arg) ? arg.deletedDateTime != null : arg.isDeleted,
    avatarUrl: getDefaultAvatarUrlByFullName(arg.fullName),
    detailRoutePath: getCustomerDetailRoutePath(arg.id)
  };
}

function isCustomerDetailModel(arg: CustomerBasicResponseDto | CustomerDetailModel): arg is CustomerDetailModel {
  const detailProperties: (keyof CustomerDetailModel)[] = ["firstName", "middleName", "lastName"];
  for (const property of detailProperties) {
    if (!Object.hasOwn(arg, property)) {
      return false;
    }
  }

  return true;
}
