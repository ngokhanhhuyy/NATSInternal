import React, { useRef } from "react";
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
  const previousSearchContent = useRef<string>(props.model.searchContent);

  // Computed.
  const searchContentValidationMessage = compute<string | undefined>(() => {
    const searchContent = props.model.searchContent.trim();
    if (searchContent.length === 1) {
      return "Nội dung tìm kiếm phải chứa ít nhất 2 ký tự";
    }
  });

  // Callbacks.
  function handleSearchBoxKeyDown(event: React.KeyboardEvent): void {
    if (event.key !== "Enter" || searchContentValidationMessage) {
      return;
    }

    if (props.model.searchContent === previousSearchContent.current) {
      return;
    }

    props.onReloadingRequested();
    previousSearchContent.current = props.model.searchContent;
  }

  function handleSearchBoxBlurred(): void {
    if (props.model.searchContent !== previousSearchContent.current) {
      props.onReloadingRequested();
      previousSearchContent.current = props.model.searchContent;
    }
  }

  // Template.
  return (
    <BaseListPage {...props} filterPanelChildren={
      <FormField path="searchContent" displayName="Tìm kiếm" hideLabel>
        <div className="form-input-group">
          <TextInput
            className={joinClassName("z-1 min-w-65", searchContentValidationMessage && "is-invalid")}
            placeholder="Tìm kiếm"
            autoComplete="off"
            value={props.model.searchContent}
            onValueChanged={(searchContent) => props.onModelUpdated({ searchContent } as Partial<TListModel>)}
            onKeyDown={handleSearchBoxKeyDown}
            onBlur={handleSearchBoxBlurred}
          />

          <button
            type="button"
            className="btn shrink-0 border-s-transparent gap-1"
            onClick={props.onReloadingRequested}
          >
            <MagnifyingGlassIcon />
            <span className="hidden sm:inline">Tìm kiếm</span>
          </button>
        </div>

        {searchContentValidationMessage && (
          <span className="field-validation-error">
            {searchContentValidationMessage}
          </span>
        )}
      </FormField>
    }/>
  );
}
