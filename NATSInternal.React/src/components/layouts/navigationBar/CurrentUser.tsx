import React, { useState, useMemo, useCallback, useEffect } from "react";
import { Link } from "react-router";
import { useApi } from "@/api";
import { useNavigationBarStore } from "@/stores";
import { useRouteHelper, useRoleHelper, useTsxHelper } from "@/helpers";

// Child component.
import { UserIcon } from "@heroicons/react/24/solid";

// Type.
type CurrentUser = UserGetDetailResponseDto & { avatarUrl: string };

// Props.
export default function CurrentUser(): React.ReactNode {
  // Dependencies.
  const isNavigationBarExpanded = useNavigationBarStore(store => store.isExpanded);
  const api = useApi();
  const { getUserProfileRoutePath } = useRouteHelper();
  const { joinClassName } = useTsxHelper();

  // State.
  const [currentUser, setCurrentUser] = useState<CurrentUser | null>(null);
  const [isAvatarLoadingFailed, setIsAvatarLoadingFailed] = useState<boolean>(false);

  // Computed.
  const maxPowerLevelRole = useMemo<UserGetDetailRoleResponseDto | null>(() => {
    if (!currentUser) {
      return null;
    }

    const maxPowerLevel = Math.max(...currentUser.roles.map(role => role.powerLevel) ?? [0]);
    return currentUser.roles.find(role => role.powerLevel === maxPowerLevel) ?? null;
  }, [currentUser]);

  // Callback.
  const handleAvatarLoadingFailed = useCallback(() => {
    setIsAvatarLoadingFailed(true);
  }, []);

  // Effect.
  useEffect(() => {
    const loadCurrentUser = async () => {
      const responseDto = await api.authentication.getCallerDetailAsync();
      const nameArgument = responseDto.userName.toUpperCase().split("").join("+");
      const avatarUrl = `https://ui-avatars.com/api/?name=${nameArgument}&size=128&format=svg`;
      setCurrentUser({ ...responseDto, avatarUrl }); 
    };

    loadCurrentUser().then(() => { });
  }, []);

  // Template.
  if (!currentUser) {
    return null;
  }

  return (
    <Link
      id="current-user-link"
      className={joinClassName(
        "flex justify-center items-end gap-2 ms-1 mt-auto relative w-fit overflow-hidden",
        "hover:no-underline hover:opacity-50 hover:cursor-pointer"
      )}
      to={getUserProfileRoutePath(currentUser.id)}
    >
      <div className="w-10 h-10 rounded-[50%] overflow-hidden shrink-0">
        {!isAvatarLoadingFailed ? (
          <img
            className="object-cover rounded-full"
            src={currentUser.avatarUrl}
            alt={currentUser.userName}
            onError={handleAvatarLoadingFailed}
          />
        ) : (
          <UserIcon className="bg-primary/10 size-6" /> 
        )}
      </div>

      <div className={joinClassName(
        "flex flex-col flex-1 justify-end items-start shrink-0",
        !isNavigationBarExpanded && "hidden"
      )}>
        <span>{currentUser.userName}</span>
        <div className={"flex flex-wrap gap-1"}>
          {maxPowerLevelRole && (
            <>
              <Role role={maxPowerLevelRole} />
              {currentUser.roles.length >= 2 && (
                <Role role={maxPowerLevelRole} remainingCount={currentUser.roles.length - 1} />
              )}
            </>
          )}
        </div>
      </div>
    </Link>
  );
}

function Role(props: { role: UserGetDetailRoleResponseDto; remainingCount?: number }): React.ReactNode {
  // Dependencies.
  const {
    getRoleBackgroundColorClassName,
    getRoleForegroundColorClassName,
    getRoleBorderColorClassName } = useRoleHelper();
  const { joinClassName } = useTsxHelper();

  // Template.
  return (
    <span
      className={joinClassName(
        "text-xs border rounded-md px-1 py-0.5",
        getRoleBackgroundColorClassName(props.role.name),
        getRoleForegroundColorClassName(props.role.name),
        getRoleBorderColorClassName(props.role.name)
      )}
    >
      {props.remainingCount ? `+${props.remainingCount}` : props.role.name}
    </span>
  );
}