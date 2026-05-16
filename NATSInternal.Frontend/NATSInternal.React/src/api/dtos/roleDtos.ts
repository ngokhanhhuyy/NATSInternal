declare global {
  type RoleDetailResponseDto = {
    id: number;
    name: string;
    displayName: string;
    powerLevel: number;
    permissionNames: string[];
  };
}