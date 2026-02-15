import { useAvatarHelper, useRouteHelper } from "@/helpers";

declare global {
  type CustomerBasicModel = Readonly<{
    id: string;
    fullName: string;
    nickName: string | null;
    isDeleted: boolean;
    avatarUrl: string;
    detailRoute: string;
  }>;
}

const avatarHelper = useAvatarHelper();
const { getCustomerDetailRoutePath } = useRouteHelper();

function createFromResponseDto(responseDto: CustomerBasicResponseDto): CustomerBasicModel {
  return {
    ...responseDto,
    avatarUrl: avatarHelper.getDefaultAvatarUrlByFullName(responseDto.fullName),
    detailRoute: getCustomerDetailRoutePath(responseDto.id)
  };
}

function createFromCustomerListCustomerModel(model: CustomerListCustomerModel): CustomerBasicModel {
  const { id, fullName, nickName, avatarUrl, detailRoute } = model;
  return {
    id,
    fullName,
    nickName,
    isDeleted: false,
    avatarUrl,
    detailRoute
  };
}

export {
  createFromResponseDto as createCustomerBasicModelFromResponseDto,
  createFromCustomerListCustomerModel as createCustomerBasicModelFromCustomerListCustomerModel
};