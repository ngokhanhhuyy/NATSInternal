export interface IJsonHelper {
  parseJson<T>(json: string): T | null;
}

const helper = {
  parseJson<T>(json: string): T | null {
    try {
      return JSON.parse(json) as T;
    } catch {
      return null;
    }
  }
};

export function useJsonHelper(): IJsonHelper {
  return helper;
}