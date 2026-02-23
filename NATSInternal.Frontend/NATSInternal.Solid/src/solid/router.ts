import { useLocation, useParams, useSearchParams, useCurrentMatches } from "@solidjs/router";

const router = {
  location: useLocation(),
  params: useParams(),
  searchParams: useSearchParams(),
  get currentMatches() {
    return useCurrentMatches()();
  }
};

export function useRouter(): typeof router {
  return router;
}