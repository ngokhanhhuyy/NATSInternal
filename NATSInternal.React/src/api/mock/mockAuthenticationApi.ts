import { useMockDatabase } from "./mockDatabase";
import type { AuthenticationApi } from "../authenticationApi";
import { createUserGetDetailResponseDto } from "./mockUserApi";
import { AuthorizationError, ValidationError, OperationError, type ApiErrorDetails } from "../errors";
import { toAsyncMicrotask } from "./mockApiHelpers";

const mockDatabase = useMockDatabase();

const mockAuthenticationApi: AuthenticationApi = {
  signInAsync: toAsyncMicrotask((requestDto: VerifyUserNameAndPasswordRequestDto): void => {
    validateVerifyUserNameAndPasswordRequestDto(requestDto);
    const user = mockDatabase.users.find(u => u.userName === requestDto.userName);
    if (!user) {
      throw new OperationError({ userName: "Username doesn't exist." });
    }

    if (user.password !== requestDto.password) {
      throw new OperationError({ password: "Password is incorrect." });
    }

    localStorage.setItem("currentUser", JSON.stringify(createUserGetDetailResponseDto(user)));
  }),

  clearAccessToken: toAsyncMicrotask((): void => {
    localStorage.removeItem("currentUser");
  }),

  changePasswordAsync: toAsyncMicrotask((requestDto: ChangePasswordRequestDto): void => {
    
  }),

  async checkAuthenticationStatusAsync(): Promise<boolean> {
    try {
      await httpClient.getAsync("/authentication/checkAuthenticationStatus", { });
      return true;
    } catch (error) {
      if (error instanceof AuthorizationError) {
        return false;
      }

      throw error;
    }
  }
};

export function useMockingAuthenticationApi(): AuthenticationApi {
  return api;
}

function validateVerifyUserNameAndPasswordRequestDto(requestDto: VerifyUserNameAndPasswordRequestDto): void {
  const validationErrors: ApiErrorDetails = { };
  if (!requestDto.userName) {
    validationErrors["userName"] = "Username is required.";
  }

  if (!requestDto.password) {
    validationErrors["password"] = "Password is required.";
  }

  if (Object.entries(validationErrors).length) {
    throw new ValidationError(validationErrors);
  }
}

function validateChangePasswordRequestDto(requestDto: ChangePasswordRequestDto): void {
  const validationErrors: MockApiErrorDetails<ChangePasswordRequestDto> = { };
  if (!requestDto.currentPassword) {
    validationErrors.currentPassword = "Current password is required.";
  }
  
  if (!requestDto.currentPassword) {
    validationErrors.currentPassword = "Current password is required.";
  }

  if (!requestDto.confirmationPassword) {
    validationErrors.confirmationPassword = "Confirmation password is required.";
  } else if (requestDto.confirmationPassword !== requestDto.newPassword) {
    validationErrors.confirmationPassword = "Confirmation password doesn't match new password.";
  }
}