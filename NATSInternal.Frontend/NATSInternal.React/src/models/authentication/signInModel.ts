declare global {
  type SignInModel = {
    userName: string;
    password: string;
    toRequestDto(): VerifyUserNameAndPasswordRequestDto;
  };
}

export function createSignInModel(): SignInModel {
  return {
    userName: "",
    password: "",
    toRequestDto(): VerifyUserNameAndPasswordRequestDto {
      return {
        userName: this.userName,
        password: this.password
      };
    }
  };
}