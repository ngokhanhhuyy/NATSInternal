import { useDateTimeUtility, useRouteHelper } from "@/helpers";

declare global {
  type CustomerBasicModel = Readonly<{
    id: string;
    fullName: string;
    nickName: string | null;
    gender: Gender;
    birthday: string | null;
    phoneNumber: string | null;
    isDeleted: boolean;
    authorization: CustomerExistingAuthorizationResponseDto | null;
    get detailRoute(): string;
  }>;
}

const dateTimeUtility = useDateTimeUtility();
const { getCustomerDetailRoutePath } = useRouteHelper();

export function createCustomerBasicModel(responseDto: CustomerBasicResponseDto): CustomerBasicModel {
  return {
    ...responseDto,
    birthday: responseDto.birthday && dateTimeUtility.getDisplayDateString(responseDto.birthday),
    get detailRoute(): string {
      return getCustomerDetailRoutePath(this.id);
    }
  };
}