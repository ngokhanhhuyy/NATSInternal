declare global {
  type UserBasicResponseDto = {
    id: string;
    userName: string;
    isDeleted: boolean;
  };

  type RoleBasicResponseDto = {
    id: string;
    name: string;
    displayName: string;
    powerLevel: number;
  };

  type CustomerBasicResponseDto = {
    id: string;
    fullName: string;
    nickName: string | null;
    isDeleted: boolean;
  };
  
  type ProductBasicResponseDto = {
    id: string;
    name: string;
    unit: string;
    isDeleted: boolean;
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