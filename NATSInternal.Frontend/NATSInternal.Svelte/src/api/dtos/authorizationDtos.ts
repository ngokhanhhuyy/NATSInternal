declare global {
  type UserExistingAuthorizationResponseDto = {
    canChangePassword: boolean;
    canResetPassword: boolean;
    canDelete: boolean;
    canAddToOrRemoveFromRoles: boolean;
  };

  type CustomerExistingAuthorizationResponseDto = {
    canEdit: boolean;
    canDelete: boolean;
  };

  type ProductExistingAuthorizationResponseDto = {
    canEdit: boolean;
    canDelete: boolean;
  };
}

export {};
