import type { User } from "./mockDatabase";
import { PermissionNames } from "./mockPermissionNames";
import { AuthenticationError, AuthorizationError } from "../errors";

type Caller = Readonly<User & {
  hasPermission(permissionName: string): boolean;
  get powerLevel(): number;
}>;

let _caller: Caller | null = null;

export function getUserExistingAuthorization(user: User): UserExistingAuthorizationResponseDto {
  return {
    canChangePassword: canChangeUserPassword(user),
    canResetPassword: canResetUserPassword(user),
    canAddToOrRemoveFromRoles: canAddUserToOrRemoveUserFromRoles(user),
    canDelete: canDeleteUser(user)
  };
}

export function canCreateUser(): boolean {
  return getCaller().hasPermission(PermissionNames.CreateUser);
}

export function canChangeUserPassword(user: User): boolean {
  return user.id === getCaller().id;
}

export function canResetUserPassword(user: User): boolean {
  const caller = getCaller();
  return user.id !== caller.id && caller.hasPermission(PermissionNames.ResetAnotherUserPassword);
}

export function canAddUserToOrRemoveUserFromRoles(user: User): boolean {
  const caller = getCaller();
  return user.id !== caller.id && caller.hasPermission(PermissionNames.AddUserToOrRemoveUserFromRoles);
}

export function canDeleteUser(user: User): boolean {
  const caller = getCaller();
  return user.id !== caller.id && caller.hasPermission(PermissionNames.DeleteAnotherUser);
}

function getCaller(): Caller {
  if (_caller) {
    return _caller;
  }

  const userData = localStorage.getItem("currentUser");
  if (!userData) {
    throw new AuthenticationError();
  }

  try {
    _caller = {
      ...JSON.parse(userData) as Caller,
      hasPermission(permissionName: string): boolean {
        return this.roles
          .flatMap(role => role.permissions.map(permission => permission.name))
          .includes(permissionName);
      },
      get powerLevel(): number {
        return Math.max(...this.roles.map(role => role.powerLevel));
      }
    };
    return _caller;
  } catch {
    localStorage.removeItem("currentUser");
    _caller = null;
    throw new AuthorizationError();
  }
}