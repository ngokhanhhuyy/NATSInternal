import { useMockDatabase, type User } from "./mockDatabase";
import type { AuthenticationApi } from "../authenticationApi";
import { OperationError, AuthenticationError } from "../errors";
import { throttleAsync } from "./throttle";
import { validateUsingSchema } from "./mockValidation";
import * as v from "valibot";

export type Caller = Readonly<{
  id: string;
  userName: string;
  roles: { id: string; name: string; displayName: string; powerLevel: number; permissionNames: string[] }[];
  hasPermission(permissionName: string): boolean;
  get powerLevel(): number;
}>;

const mockDatabase = useMockDatabase();

const mockAuthenticationApi: AuthenticationApi = {
  async getAccessCookieAsync(requestDto: VerifyUserNameAndPasswordRequestDto): Promise<void> {
    await throttleAsync();
    console.log(123);
    validateUsingSchema(VerifyUserNameAndPasswordRequestDto, requestDto);
    const user = mockDatabase.users.find((u) => u.userName === requestDto.userName);
    if (!user) {
      throw new OperationError({ userName: "Không tồn tại" });
    }

    if (user.password !== requestDto.password) {
      throw new OperationError({ password: "Không chính xác." });
    }

    setCaller(user);
  },
  async clearAccessCookieAsync(): Promise<void> {
    await throttleAsync();
    clearCaller();
  },
  async changePasswordAsync(requestDto: ChangePasswordRequestDto): Promise<void> {
    await throttleAsync();
    validateUsingSchema(ChangePasswordRequestDto, requestDto);
    const caller = getAndEnsureCallerExists();
    const user = mockDatabase.users.find((u) => u.id === caller.id);
    if (!user) {
      await this.clearAccessCookieAsync();
      throw new AuthenticationError();
    }

    if (user?.password !== requestDto.currentPassword) {
      throw new OperationError({ currentPassword: "Mật khẩu hiện tại không chính xác." });
    }

    user.password = requestDto.newPassword;
  },
  async checkAuthenticationStatusAsync(): Promise<void> {
    await throttleAsync();
    getAndEnsureCallerExists();
  }
};

export function useMockAuthenticationApi(): AuthenticationApi {
  return mockAuthenticationApi;
}

const _caller: Caller | null = null;
const callerKey = "caller";
export function getAndEnsureCallerExists(): Caller {
  if (_caller) {
    return _caller;
  }

  const callerData = localStorage.getItem(callerKey);
  if (callerData == null) {
    localStorage.removeItem(callerKey);
    throw new AuthenticationError();
  }

  try {
    const callerAsUser = JSON.parse(callerData) as UserGetDetailResponseDto;
    return {
      ...callerAsUser,
      hasPermission(permissionName: string): boolean {
        return this.roles.flatMap((role) => role.permissionNames).includes(permissionName);
      },
      get powerLevel(): number {
        return Math.max(...this.roles.map((role) => role.powerLevel));
      }
    };
  } catch {
    localStorage.removeItem(callerKey);
    throw new AuthenticationError();
  }
}

export function setCaller(user: User): void {
  const caller: Pick<Caller, "id" | "userName" | "roles"> = {
    id: user.id,
    userName: user.userName,
    roles: user.roles.map((role) => ({
      id: role.id,
      name: role.name,
      displayName: role.displayName,
      powerLevel: role.powerLevel,
      permissionNames: role.permissions.map((permission) => permission.name)
    }))
  };

  localStorage.setItem(callerKey, JSON.stringify(caller));
}

export function clearCaller(): void {
  localStorage.removeItem(callerKey);
}

const VerifyUserNameAndPasswordRequestDto = v.object({
  userName: v.pipe(v.string(), v.minLength(1)),
  password: v.pipe(v.string(), v.minLength(1))
}) satisfies v.GenericSchema<VerifyUserNameAndPasswordRequestDto>;

const ChangePasswordRequestDto = v.pipe(
  v.object({
    currentPassword: v.pipe(v.string(), v.minLength(6)),
    newPassword: v.pipe(v.string(), v.minLength(6)),
    confirmationPassword: v.pipe(v.string())
  }),
  v.check((input) => input.confirmationPassword === input.newPassword, "Passwords mismatch.")
);
