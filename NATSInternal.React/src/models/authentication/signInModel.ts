import { createCloneMethod } from "../baseModels";
import type { VerifyUserNameAndPasswordRequestDto } from "@/api";

declare global {
  type SignInModel = ClonableModel<{
    userName: string;
    password: string;
    toRequestDto(): VerifyUserNameAndPasswordRequestDto;
  }>;
}

export function createSignInModel(): SignInModel {
  const model: SignInModel = {
    $clone: createCloneMethod(() => model),
    userName: "",
    password: "",
    toRequestDto(): VerifyUserNameAndPasswordRequestDto {
      return {
        userName: this.userName,
        password: this.password
      };
    }
  };

  return model;
}