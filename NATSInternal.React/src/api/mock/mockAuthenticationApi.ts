import type { AuthenticationApi } from "../authenticationApi";
import { AuthorizationError, ValidationError, OperationError, type ApiErrorDetails } from "../errors";
import type { VerifyUserNameAndPasswordRequestDto, ChangePasswordRequestDto } from "../dtos";
import { useMockDatabase, type User } from "./mockDatabase";

const mockDatabase = useMockDatabase();

const mockAuthenticationApi: AuthenticationApi = {
  async signInAsync(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void> {
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

    const user = mockDatabase.users.find(u => u.userName === requestDto.userName);
    if (!user) {
      throw new OperationError({ userName: "Username doesn't exist." });
    }

    if (user.password !== requestDto.password) {
      throw new OperationError({ password: "Password is incorrect." });
    }

    localStorage.setItem("currentUser", );
  },

  async clearAccessToken(): Promise<void> {
    return await httpClient.postAndIgnoreAsync("/authentication/clearAccessCookie", {  });
  },

  async changePasswordAsync(requestDto: ChangePasswordRequestDto): Promise<void> {
    return await httpClient.postAndIgnoreAsync("/authentication/changePassword", requestDto);
  },

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