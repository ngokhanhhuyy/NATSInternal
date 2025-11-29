declare global {
  type CustomerCreateModel = CustomerUpsertModel;
  type CustomerUpdateModel = CustomerUpsertModel & {
    id: string;
    authorization: CustomerExistingAuthorizationResponseDto;
  };
}

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
  introducer: CustomerBasicResponseDto | null;
  toRequestDto(): CustomerUpsertRequestDto;
};

function createCustomerCreateModel(): CustomerCreateModel {
  return {
    firstName: "",
    middleName: "",
    lastName: "",
    nickName: "",

  }
}