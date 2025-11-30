type RoleName = "Developer" | "Manager" | "Accountant" | "Staff";

export type RoleHelper = {
  getRoleBackgroundColorClassName(roleName: string): string | null;
  getRoleForegroundColorClassName(roleName: string): string | null;
  getRoleBorderColorClassName(roleName: string): string | null;
};

const roleForegroundColorClassNameMap: Record<RoleName, string> = {
  Developer: "text-indigo-500",
  Manager: "text-red-500",
  Accountant: "text-emerald-500",
  Staff: "text-primary/50"
};

const roleBackgroundColorClassNameMap: Record<RoleName, string> = {
  Developer: "bg-indigo-500/10",
  Manager: "bg-red-500/10",
  Accountant: "bg-emerald-500/10",
  Staff: "bg-primary/5"
};

const roleBorderColorClassNameMap: Record<RoleName, string> = {
  Developer: "border-indigo-500/25",
  Manager: "border-red-500/25",
  Accountant: "border-emerald-500/25",
  Staff: "border-primary/15"
};

const roleHelper: RoleHelper = {
  getRoleBackgroundColorClassName(roleName: string): string | null {
    return roleBackgroundColorClassNameMap[roleName as RoleName] ?? null;
  },
  getRoleForegroundColorClassName(roleName: string): string | null {
    return roleForegroundColorClassNameMap[roleName as RoleName] ?? null;
  },

  getRoleBorderColorClassName(roleName: string): string | null {
    return roleBorderColorClassNameMap[roleName as RoleName] ?? null;
  }
};

export function useRoleHelper(): RoleHelper {
  return roleHelper;
}
