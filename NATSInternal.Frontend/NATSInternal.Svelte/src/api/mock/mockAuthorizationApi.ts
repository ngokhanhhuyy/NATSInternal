import type { User } from "./mockDatabase";
import { getAndEnsureCallerExists } from "./mockAuthenticationApi";
import { PermissionNames } from "./mockPermissionNames";

export function getUserExistingAuthorization(user: User): UserExistingAuthorizationResponseDto {
  return {
    canChangePassword: canChangeUserPassword(user),
    canResetPassword: canResetUserPassword(user),
    canAddToOrRemoveFromRoles: canAddUserToOrRemoveUserFromRoles(user),
    canDelete: canDeleteUser(user)
  };
}

export function canCreateUser(): boolean {
  return getAndEnsureCallerExists().hasPermission(PermissionNames.CreateUser);
}

export function canChangeUserPassword(user: User): boolean {
  return user.id === getAndEnsureCallerExists().id;
}

export function canResetUserPassword(user: User): boolean {
  const caller = getAndEnsureCallerExists();
  return user.id !== caller.id && caller.hasPermission(PermissionNames.ResetAnotherUserPassword);
}

export function canAddUserToOrRemoveUserFromRoles(user: User): boolean {
  const caller = getAndEnsureCallerExists();
  return user.id !== caller.id && caller.hasPermission(PermissionNames.AddUserToOrRemoveUserFromRoles);
}

export function canDeleteUser(user: User): boolean {
  const caller = getAndEnsureCallerExists();
  return user.id !== caller.id && caller.hasPermission(PermissionNames.DeleteAnotherUser);
}
