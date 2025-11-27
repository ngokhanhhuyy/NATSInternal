import { useParams, useNavigate } from "@solidjs/router";
import { type ComponentDependencies, componentDependencies } from "@/components";

export type PageDependencies = ComponentDependencies & {
  useRouter: () => {
    params: ReturnType<typeof useParams>;
    navigate: ReturnType<typeof useNavigate>;
  }
};

export type Page<TProps extends object> = (dependencies: PageDependencies, props: TProps) => JSX.Element;

export function createPage<TProps extends object>(page: Page<TProps>): (props: TProps) => JSX.Element {
  const pageDependencies: PageDependencies = {
    ...componentDependencies,
    useRouter: () => ({
      params: useParams(),
      navigate: useNavigate()
    })
  };

  return (props: TProps) => page(pageDependencies, props);
}