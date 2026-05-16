import { httpClient } from "./httpClient";

export type AuthenticationApi = {
  getAccessCookieAsync(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void>;
  clearAccessCookieAsync(): Promise<void>;
  getCallerDetailAsync(): Promise<UserDetailResponseDto>;
  changePasswordAsync(requestDto: ChangePasswordRequestDto): Promise<void>;
  checkAuthenticationStatusAsync(): Promise<void>;
};

export const authenticationApi: AuthenticationApi = {
  async getAccessCookieAsync(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void> {
    return await httpClient.postAndIgnoreAsync("/authentication/get-access-cookie", requestDto);
  },

  async clearAccessCookieAsync(): Promise<void> {
    return await httpClient.postAndIgnoreAsync("/authentication/clear-access-cookie", {  });
  },
  
  async getCallerDetailAsync(): Promise<UserDetailResponseDto> {
    return await httpClient.getAsync("/authentication/caller-detail");
  },

  async changePasswordAsync(requestDto: ChangePasswordRequestDto): Promise<void> {
    return await httpClient.postAndIgnoreAsync("/authentication/change-password", requestDto);
  },

  async checkAuthenticationStatusAsync(): Promise<void> {
    await httpClient.postAndIgnoreAsync("/authentication/check-authentication-status", { });
  }
};
