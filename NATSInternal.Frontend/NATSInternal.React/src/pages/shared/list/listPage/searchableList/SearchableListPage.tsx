import React from "react";

// Child components.
import BaseListPage, { type BaseListPageProps } from "../baseListPage";
import { FormField, TextInput } from "@/components/form";

// Props.
type ListModel<TItemModel extends object> = ISearchableListModel<TItemModel> & IUpsertableListModel<TItemModel>;
export type SearchableListPageProps<TListModel extends ListModel<TItemModel>, TItemModel extends object> =
  BaseListPageProps<TListModel, TItemModel>;

// Components.
export default function SearchableListPage<TListModel extends ListModel<TItemModel>, TItemModel extends object>
  (props: SearchableListPageProps<TListModel, TItemModel>): React.ReactNode
{
  // Template.
  return (
    <BaseListPage {...props} filterPanelChildren={
      <FormField path="searchContent" displayName="Tìm kiếm">
        <TextInput
          placeholder="Tìm kiếm"
          autoComplete="off"
          value={props.model.searchContent}
          onValueChanged={(searchContent) => props.onModelUpdated({ searchContent } as Partial<TListModel>)}
        />
      </FormField>
    }/>
  );
}
