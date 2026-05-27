import { createCustomerBasicModel } from "@/models";

declare global {
  type CustomerUpsertModel = {
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
    authorization: CustomerExistingAuthorizationResponseDto | null;
    toRequestDto(): CustomerUpsertRequestDto;
  };
}

export function createCustomerUpsertModel(responseDto?: CustomerDetailResponseDto): CustomerUpsertModel {
  return {
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
    introducer: (responseDto?.introducer && createCustomerBasicModel(responseDto.introducer)) ?? null,
    authorization: responseDto?.authorization ?? null,
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
