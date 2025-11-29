import { useAvatarHelper, useRouteHelper } from "@/helpers";

declare global {
  type CustomerBasicModel = Readonly<{
    id: string;
    fullName: string;
    nickName: string | null;
    isDeleted: boolean;
    get avatarUrl(): string;
    get detailRoute(): string;
  }>;
}

const avatarHelper = useAvatarHelper();
const { getCustomerDetailRoutePath } = useRouteHelper();

export function createCustomerBasicModel(responseDto: CustomerBasicResponseDto): CustomerBasicModel {
  return {
    ...responseDto,
    get avatarUrl(): string {
      return avatarHelper.getDefaultAvatarUrlByFullName(this.fullName);
    },
    get detailRoute(): string {
      return getCustomerDetailRoutePath(this.id);
    }
  };
}