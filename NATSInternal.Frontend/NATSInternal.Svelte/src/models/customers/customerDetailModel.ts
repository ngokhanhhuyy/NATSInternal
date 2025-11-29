import { createCustomerBasicModel } from "../shared/customerBasicModel";
import { createUserBasicModel } from "../shared/userBasicModel";
import { useAvatarHelper, useCurrencyHelper, useDateTimeHelper } from "@/helpers";
import { useRouteHelper, usePhoneNumberHelper } from "@/helpers";

declare global {
  type CustomerDetailModel = Readonly<{
    id: string;
    firstName: string;
    middleName: string | null;
    lastName: string;
    fullName: string;
    nickName: string;
    gender: Gender;
    birthday: string | null;
    phoneNumber: string | null;
    zaloNumber: string | null;
    facebookUrl: string | null;
    email: string | null;
    address: string | null;
    note: string | null;
    createdUser: UserBasicModel;
    createdDateTime: string;
    lastUpdatedUser: UserBasicModel | null;
    lastUpdatedDateTime: string | null;
    debtRemainingAmount: number;
    introducer: CustomerBasicModel | null;
    authorization: CustomerExistingAuthorizationResponseDto;
    get avatarUrl(): string;
    get displayDebtRemainingAmountText(): string;
    get updateRoute(): string;
  }>;
}

const { getDefaultAvatarUrlByFullName } = useAvatarHelper();
const { getAmountDisplayText } = useCurrencyHelper();
const { getDisplayDateString, getDisplayDateTimeString } = useDateTimeHelper();
const { formatRawPhoneNumber } = usePhoneNumberHelper();
const { getCustomerUpdateRoutePath } = useRouteHelper();

export function createCustomerDetailModel(responseDto: CustomerGetDetailResponseDto): CustomerDetailModel {
  return {
    ...responseDto,
    birthday: responseDto.birthday && getDisplayDateString(responseDto.birthday),
    phoneNumber: responseDto.phoneNumber && formatRawPhoneNumber(responseDto.phoneNumber),
    zaloNumber: responseDto.zaloNumber && formatRawPhoneNumber(responseDto.zaloNumber),
    createdUser: createUserBasicModel(responseDto.createdUser),
    createdDateTime: getDisplayDateTimeString(responseDto.createdDateTime),
    lastUpdatedUser: responseDto.lastUpdatedUser && createUserBasicModel(responseDto.lastUpdatedUser),
    lastUpdatedDateTime: responseDto.lastUpdatedDateTime && getDisplayDateTimeString(responseDto.lastUpdatedDateTime),
    introducer: responseDto.introducer && createCustomerBasicModel(responseDto.introducer),
    get avatarUrl(): string {
      return getDefaultAvatarUrlByFullName(this.fullName);
    },
    get displayDebtRemainingAmountText(): string{
      return getAmountDisplayText(this.debtRemainingAmount);
    },
    get updateRoute(): string {
      return getCustomerUpdateRoutePath(this.id);
    }
  };
}