import { createCustomerBasicModelFromResponseDto } from "@/models";
import { useGuidHelper, useRouteHelper } from "@/helpers";

declare global {
  type CustomerUpsertModel = {
    id: string;
    firstName: string;
    middleName: string;
    lastName: string;
    nickName: string;
    gender: Gender;
    birthday: string;
    phoneNumber: string;
    zaloNumber: string;
    facebookUrl: string;
    email: string;
    address: string;
    note: string;
    introducer: CustomerBasicModel | null;
    detailRoute: string;
    authorization: CustomerExistingAuthorizationResponseDto | null;
    toRequestDto(): CustomerUpsertRequestDto;
  };
}

const { getEmptyGuid } = useGuidHelper();
const { getCustomerDetailRoutePath } = useRouteHelper();

export function createCustomerUpsertModel(responseDto?: CustomerGetDetailResponseDto): CustomerUpsertModel {
  return {
    id: responseDto?.id ?? getEmptyGuid(),
    firstName: responseDto?.firstName ?? "",
    middleName: responseDto?.middleName ?? "",
    lastName: responseDto?.lastName ?? "",
    nickName: responseDto?.nickName ?? "",
    gender: responseDto?.gender ?? "Male",
    birthday: responseDto?.birthday ?? "",
    phoneNumber: responseDto?.phoneNumber ?? "",
    zaloNumber: responseDto?.zaloNumber ?? "",
    facebookUrl: responseDto?.facebookUrl ?? "",
    email: responseDto?.email ?? "",
    address: responseDto?.address ?? "",
    note: responseDto?.note ?? "",
    introducer: (responseDto?.introducer && createCustomerBasicModelFromResponseDto(responseDto.introducer)) ?? null,
    authorization: responseDto?.authorization ?? null,
    get detailRoute(): string {
      return getCustomerDetailRoutePath(this.id);
    },
    toRequestDto() {
      return {
        firstName: this.firstName,
        middleName: this.middleName || null,
        lastName: this.lastName,
        nickName: this.nickName || null,
        gender: this.gender,
        birthday: this.birthday || null,
        phoneNumber: this.phoneNumber || null,
        zaloNumber: this.zaloNumber || null,
        facebookUrl: this.facebookUrl || null,
        email: this.email || null,
        address: this.address || null,
        note: this.note || null,
        introducerId: this.introducer?.id ?? null
      };
    },
  };
}