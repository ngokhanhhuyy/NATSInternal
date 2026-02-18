import { useLocation, useParams, useSearchParams, useMatch, useCurrentMatches } from "@solidjs/router";

const router = {
  location: useLocation(),
  params: useParams(),
  searchParams: useSearchParams(),
  get match() { return useMatch(); }
};