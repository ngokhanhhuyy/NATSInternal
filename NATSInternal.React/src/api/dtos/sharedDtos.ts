declare global {
  type UserBasicResponseDto = {
    id: string;
    userName: string;
    roles: RoleBasicResponseDto[];
    authorization: UserExistingAuthorizationResponseDto;
  };

  type RoleBasicResponseDto = {
    id: string;
    name: string;
    displayName: string;
    powerLevel: number;
  };
  
  type ProductBasicResponseDto = {
    id: string;
    name: string;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    isResupplyNeeded: boolean;
    thumbnailUrl: string | null;
    authorization: ProductExistingAuthorizationResponseDto;
  };
  
  type BrandBasicResponseDto = {
    id: string;
    name: string;
  };
  
  type ProductCategoryBasicResponseDto = {
    id: string;
    name: string;
  };
  
  type PhotoBasicResponseDto = {
    id: string;
    url: string;
    isThumbnail: boolean;
  };
  
  type PhotoCreateOrUpdateRequestDto = {
    id: string | null;
    file: string;
    isThumbnail: boolean;
    isChanged: boolean;
    isDeleted: boolean;
  };
}