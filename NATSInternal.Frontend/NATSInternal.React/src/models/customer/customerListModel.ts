import { getMetadata } from "@/metadata";
import { useAvatarHelper, useCurrencyHelper, useDateTimeHelper } from "@/helpers";
import { usePhoneNumberHelper, useRouteHelper } from "@/helpers";

declare global {
  type CustomerListModel = Implements<IPageableListModel<CustomerListCustomerModel>, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number | null;
    searchContent: string;
    items: CustomerListCustomerModel[];
    pageCount: number;
    get sortByFieldNameOptions(): string[];
    get createRoute(): string;
    mapFromResponseDto(responseDto: CustomerGetListResponseDto): CustomerListModel;
    toRequestDto(): CustomerGetListRequestDto;
  }>;

  type CustomerListCustomerModel = Readonly<{
    id: string;
    fullName: string;
    nickName: string | null;
    gender: Gender;
    birthday: string | null;
    phoneNumber: string | null;
    debtRemainingAmount: number;
    authorization: CustomerExistingAuthorizationResponseDto;
    get avatarUrl(): string;
    get formattedBirthday(): string | null;
    get formattedPhoneNumber(): string | null;
    get formattedDebtRemainingAmount(): string;
    get detailRoute(): string;
  }>;
}

const { getDefaultAvatarUrlByFullName } = useAvatarHelper();
const { getAmountDisplayText: getDisplayCurrencyText } = useCurrencyHelper();
const { getDisplayDateString } = useDateTimeHelper();
const { formatRawPhoneNumber } = usePhoneNumberHelper();
const { getCustomerCreateRoutePath, getCustomerDetailRoutePath } = useRouteHelper();
const customerListOptions = getMetadata().listOptionsList.customer;

export function createCustomerListModel(responseDto?: CustomerGetListResponseDto): CustomerListModel {
  const model: CustomerListModel = {
    sortByAscending: customerListOptions.defaultSortByAscending ?? true,
    sortByFieldName: customerListOptions.defaultSortByFieldName ?? "",
    page: 1,
    resultsPerPage:  customerListOptions.defaultResultsPerPage,
    searchContent: "",
    items: [],
    pageCount: 0,
    get sortByFieldNameOptions(): string[] {
      return customerListOptions.sortByFieldNameOptions;
    },
    get createRoute(): string {
      return getCustomerCreateRoutePath();
    },
    mapFromResponseDto(responseDto: CustomerGetListResponseDto): CustomerListModel {
      return {
        ...this,
        items: responseDto.items.map(createCustomerListCustomerModel),
        pageCount: responseDto.pageCount
      };
    },
    toRequestDto(): CustomerGetListRequestDto {
      const requestDto: CustomerGetListRequestDto = {
        sortByAscending: this.sortByAscending,
        sortByFieldName: this.sortByFieldName,
        page: this.page,
      };

      if (this.resultsPerPage) {
        requestDto.resultsPerPage = this.resultsPerPage;
      }

      if (this.searchContent) {
        requestDto.searchContent = this.searchContent;
      }

      return requestDto;
    }
  };

  if (responseDto) {
    return model.mapFromResponseDto(responseDto);
  }

  return model;
}

function createCustomerListCustomerModel(responseDto: CustomerGetListCustomerResponseDto): CustomerListCustomerModel {
  return {
    ...responseDto,
    get avatarUrl(): string {
      return getDefaultAvatarUrlByFullName(this.fullName);
    },
    get formattedPhoneNumber(): string | null {
      return this.phoneNumber && formatRawPhoneNumber(this.phoneNumber);
    },
    get formattedBirthday(): string | null {
      return this.birthday && getDisplayDateString(this.birthday);
    },
    get formattedDebtRemainingAmount(): string {
      return getDisplayCurrencyText(this.debtRemainingAmount);
    },
    get detailRoute(): string {
      return getCustomerDetailRoutePath(this.id);
    },
  }; 
}