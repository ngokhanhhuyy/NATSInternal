import { A } from "@solidjs/router";
import { useRouteHelper, useTsxHelper } from "@/helpers";

// Component.
export default function MainLogo(): JSX.Element {
  // Dependencies.
  const { getHomeRoutePath } = useRouteHelper();
  const { joinClassName: joinClass } = useTsxHelper();

  // Template.
  return (
    <A
      class={joinClass(
        "flex gap-1.5 md:gap-2.5 justify-start items-center",
        "hover:opacity-100 hover:no-underline",
      )}
      href={getHomeRoutePath()}
    >
      <i class={joinClass(
        "bi bi-star-fill",
        "bg-emerald-500/10 dark:bg-emerald-500/20 border border-success size-10 p-1.5",
        "rounded-[50%] fill-emerald-500 dark:fill-emerald-400 shrink-0",
      )} />

      <span class="text-emerald-500 dark:text-emerald-400 font-light text-2xl block">
        natsinternal
      </span>
    </A>
  );
}