import { useHttpClient } from "./httpClient";

const httpClient = useHttpClient();

export type AuthenticationApi = {
  getAccessCookieAsync(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void>;
  clearAccessCookieAsync(): Promise<void>;
  changePasswordAsync(requestDto: ChangePasswordRequestDto): Promise<void>;
  checkAuthenticationStatusAsync(): Promise<void>;
};

const api: AuthenticationApi = {
  async getAccessCookieAsync(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void> {
    return await httpClient.postAndIgnoreAsync("/authentication/getAccessCookie", requestDto);
  },

  async clearAccessCookieAsync(): Promise<void> {
    return await httpClient.postAndIgnoreAsync("/authentication/clearAccessCookie", {  });
  },

  async changePasswordAsync(requestDto: ChangePasswordRequestDto): Promise<void> {
    return await httpClient.postAndIgnoreAsync("/authentication/changePassword", requestDto);
  },

  async checkAuthenticationStatusAsync(): Promise<void> {
    await httpClient.getAsync("/authentication/checkAuthenticationStatus", { });
  }
};

export function useAuthenticationApi(): AuthenticationApi {
  return api;
}