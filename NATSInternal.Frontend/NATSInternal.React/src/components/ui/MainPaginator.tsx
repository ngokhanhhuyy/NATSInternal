import React, { useMemo } from "react";
import { usePaginationHelper, useTsxHelper, type PaginationRange } from "@/helpers";

// Child components.
import { Button } from "../ui";

// Types.
type PaginationRanges = {
  largeScreen: PaginationRange,
  smallScreen: PaginationRange,
};

type MainPaginatorProps = {
  page: number;
  pageCount: number;
  isReloading: boolean;
  onPageChanged: (page: number) => any;
} & React.ComponentPropsWithoutRef<"div">;

export default function MainPaginator(props: MainPaginatorProps): React.ReactNode {
  // Props.
  const { page: currentPage, pageCount, isReloading, onPageChanged, className, ...domProps } = props;

  // Dependencies.
  const { getPaginationRange } = usePaginationHelper();
  const { joinClassName } = useTsxHelper();

  // Computed.
  const paginationRanges: PaginationRanges = useMemo(() => ({
    largeScreen: getPaginationRange({
      currentPage,
      pageCount,
      visibleButtons: 5
    }),
    smallScreen: getPaginationRange({
      currentPage,
      pageCount,
      visibleButtons: 3
    }),
  }), [currentPage, pageCount]);

  const isPageExceedingSmallScreenRange = (page: number) => {
    return page < paginationRanges.smallScreen.startingPage || page > paginationRanges.smallScreen.endingPage;
  };

  const isPageExceedingLargeScreenRange = (page: number) => {
    return page < paginationRanges.largeScreen.startingPage || page > paginationRanges.largeScreen.endingPage;
  };

  // Callbacks.
  const handlePageButtonClick = (buttonPage: number): void => {
    window.scrollTo({ top: 0, behavior: "smooth" });
    props.onPageChanged(buttonPage);
  };

  // Template.
  const renderPageWithNumberButtons = (): React.ReactNode[] => {
    const startingPage = paginationRanges.largeScreen.startingPage;
    const endingPage = paginationRanges.largeScreen.endingPage;
    return Array.from({ length: endingPage - (startingPage - 1) }, (_, index) => index + startingPage).map(page => (
      <Button
        className={currentPage === page ? "primary" : undefined}
        onClick={() => handlePageButtonClick(page)}
        key={page}
      >
        {page}
      </Button>
    ));
  };

  return (
    <div
      className={joinClassName(
        className,
        "flex flex-row justify-center gap-2",
        props.isReloading && "opacity-50 pointer-events-none"
      )}
      {...domProps}
    >
      {isPageExceedingLargeScreenRange(1) && (
        <div className={joinClassName(
          "flex gap-2",
          !isPageExceedingSmallScreenRange(1) && "hidden"
        )}>
          <Button type="button" className="button" onClick={() => handlePageButtonClick(1)}>{1}</Button>
          <span>...</span>
        </div>
      )}
      {renderPageWithNumberButtons()}
      {isPageExceedingLargeScreenRange(pageCount) && (
        <div className={joinClassName(
          "flex gap-2",
          !isPageExceedingSmallScreenRange(pageCount) && "hidden"
        )}>
          <span>...</span>
          <Button type="button" className="button" onClick={() => handlePageButtonClick(pageCount)}>
            {pageCount}
          </Button>
        </div>
      )}
    </div>
  );
};