import { createMemo } from "solid-js";
import { useLocation, useParams, useSearchParams, useMatch, useCurrentMatches } from "@solidjs/router";

type Computers = Record<string, () => any>;
type Memos<TComputers extends Computers> = { [K in keyof TComputers]: Readonly<ReturnType<TComputers[K]>> };
export function createMemos<TComputers extends Computers>(computers: TComputers): Memos<TComputers> {
  const memos = { } as Memos<TComputers>;
  for (const [key, computer] of Object.entries(computers)) {
    const memo = createMemo(computer);
    Object.defineProperty(memos, key, {
      get: memo,
      enumerable: true
    });
  }

  return memos;
}

type Derivers<TComputers extends Computers> = { [K in keyof TComputers]: Readonly<ReturnType<TComputers[K]>> };
export function createDerivedSignals<TComputers extends Computers>(computers: TComputers): Derivers<TComputers> {
  const derivers = { } as Derivers<TComputers>;
  for (const [key, computer] of Object.entries(computers)) {
    Object.defineProperty(derivers, key, {
      get: computer,
      enumerable: true
    });
  }

  return derivers;
}

export function useRoute() {
  return {
    
  };
}

export * from "solid-js";
export { createStore, createMutable } from "solid-js/store";
export * from "@solidjs/router";