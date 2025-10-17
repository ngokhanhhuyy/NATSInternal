export interface IJsonUtility {
  parseJson<T>(json: string): T | null;
}

export function useJsonUtility(): IJsonUtility {
  function parseJson<T>(json: string): T | null {
    try {
      return JSON.parse(json) as T;
    } catch {
      return null;
    }
  }

  return { parseJson };
}