import React, { useState, useRef, useEffect } from "react";
import { compute, joinClassName } from "@/helpers";

// Child components.
import BaseListPage, { type BaseListPageProps } from "../baseListPage";
import { FormField, TextInput } from "@/components/form";
import { MagnifyingGlassIcon } from "@heroicons/react/24/outline";

// Props.
type ListModel<TItemModel extends object> = ISearchableListModel<TItemModel> & IUpsertableListModel<TItemModel>;
export type SearchableListPageProps<TListModel extends ListModel<TItemModel>, TItemModel extends object> =
  BaseListPageProps<TListModel, TItemModel>;

// Components.
export default function SearchableListPage<TListModel extends ListModel<TItemModel>, TItemModel extends object>
  (props: SearchableListPageProps<TListModel, TItemModel>): React.ReactNode
{
  // States.
  const [searchContent, setSearchContent] = useState<string>(props.model.searchContent);
  const previousSearchContent = useRef<string>(searchContent);

  // Computed.
  const searchContentValidationMessage = compute<string | undefined>(() => {
    const trimmedSearchContent = searchContent.trim();
    if (trimmedSearchContent.length === 1) {
      return "Nội dung tìm kiếm phải chứa ít nhất 2 ký tự";
    }
  });

  // Callbacks.
  function handleSearchBoxKeyDown(event: React.KeyboardEvent): void {
    if (event.key !== "Enter" || searchContentValidationMessage) {
      return;
    }

    if (searchContent === previousSearchContent.current) {
      return;
    }

    props.onModelUpdated({ searchContent } as Partial<TListModel>);
    previousSearchContent.current = searchContent;
  }

  function handleSearchBoxBlurredOrSearchButtonClicked(): void {
    if (searchContent !== previousSearchContent.current) {
      props.onModelUpdated({ searchContent } as Partial<TListModel>);
      previousSearchContent.current = searchContent;
    }
  }

  // Effect.
  useEffect(() => {
    setSearchContent(props.model.searchContent);
  }, [props.model.searchContent]);

  // Template.
  return (
    <BaseListPage {...props} filterPanelChildren={
      <div className={joinClassName(
        "grid grid-cols-1 gap-x-2 gap-y-3",
        props.filterPanelChildren != null && "md:grid-cols-[auto_1fr] lg:grid-cols-1 xl:grid-cols-2"
      )}>
        <FormField path="searchContent" displayName="Tìm kiếm" hideLabel>
          <div className="form-input-group">
            <TextInput
              className={joinClassName("z-1 min-w-65", searchContentValidationMessage && "is-invalid")}
              placeholder="Tìm kiếm"
              autoComplete="off"
              value={searchContent}
              onValueChanged={setSearchContent}
              onKeyDown={handleSearchBoxKeyDown}
              onBlur={handleSearchBoxBlurredOrSearchButtonClicked}
            />

            <button
              type="button"
              className="btn shrink-0 border-s-transparent gap-1 aspect-square"
              onClick={handleSearchBoxBlurredOrSearchButtonClicked}
            >
              <MagnifyingGlassIcon />
            </button>
          </div>

          {searchContentValidationMessage && (
            <span className="field-validation-error">
              {searchContentValidationMessage}
            </span>
          )}
        </FormField>

        {props.filterPanelChildren}
      </div>
    }/>
  );
}
