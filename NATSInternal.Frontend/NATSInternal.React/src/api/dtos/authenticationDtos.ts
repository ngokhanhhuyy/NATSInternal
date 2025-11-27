declare global {
  type VerifyUserNameAndPasswordRequestDto = {
    userName: string;
    password: string;
  };

  type ChangePasswordRequestDto = {
    currentPassword: string;
    confirmationPassword: string;
    newPassword: string;
  };
}