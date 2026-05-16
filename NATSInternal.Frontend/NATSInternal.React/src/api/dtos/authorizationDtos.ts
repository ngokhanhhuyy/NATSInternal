declare global {
  type UserExistingAuthorizationResponseDto = {
    canChangePassword: boolean;
    canResetPassword: boolean;
    canUpdate: boolean;
    canDelete: boolean;
  };

  type CustomerExistingAuthorizationResponseDto = {
    canEdit: boolean;
    canDelete: boolean;
  };
  
  type ProductExistingAuthorizationResponseDto = {
    canEdit: boolean;
    canDelete: boolean;
  };
  
  type ProductCategoryExistingAuthorizationResponseDto = {
    canEdit: boolean;
    canDelete: boolean;
  }
}

export { };