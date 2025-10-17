import { useJsonUtility, type IJsonUtility } from "./jsonUtility.ts";

export interface IUtilities {
  json: IJsonUtility
}

const utilities: IUtilities = {
  json: useJsonUtility()
}

export function useUtilities(): IUtilities {
  return utilities;
}