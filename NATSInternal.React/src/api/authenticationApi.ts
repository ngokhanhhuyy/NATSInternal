import { useHttpClient } from "./httpClient";
import type { VerifyUserNameAndPasswordRequestDto, ChangePasswordRequestDto } from "./dtos/authenticationDtos";
import { AuthorizationError } from "./errors";

const httpClient = useHttpClient();

export type AuthenticationApi = {
  signInAsync(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void>;
  clearAccessToken(): Promise<void>;
  changePasswordAsync(requestDto: ChangePasswordRequestDto): Promise<void>;
  checkAuthenticationStatusAsync(): Promise<boolean>;
};

const api: AuthenticationApi = {
  async signInAsync(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void> {
    return await httpClient.postAndIgnoreAsync("/authentication/getAccessCookie", requestDto);
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

export function useAuthenticationApi(): AuthenticationApi {
  return api;
}