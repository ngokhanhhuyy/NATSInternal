import React, { useState, useMemo } from "react";
import { useJSONDirtyModelChecker } from "@/hooks";
import { getMetadata } from "@/metadata";
import { createCountryBasicModel } from "@/models";

// Child components.
import { FormContainer } from "@/components/layouts";
import { FormField, TextInput, SelectInput, type SelectInputOption,  } from "@/components/form";
import { SubmitButton, DeleteButton } from "@/components/form";

// Props.
type Props<TUpsertResult extends string | void> = {
  isForCreating: boolean;
  model: BrandUpsertModel;
  onModelUpdated(updatedData: Partial<BrandUpsertModel>): any;
  upsertAction(): Promise<TUpsertResult>;
  onUpsertingSucceeded(upsertResult: TUpsertResult): any;
  deleteAction?(): Promise<void>;
  onDeletionSucceeded?(): any;
  renderButtons?(): React.ReactNode;
};

// Component.
const BrandUpsertPage = <TUpsertResult extends string | void>(props: Props<TUpsertResult>): React.ReactNode => {
  // States.
  const isModelDirty = useJSONDirtyModelChecker(props.model);
  const [countries] = useState<CountryBasicModel[]>(() => getMetadata().countries.map(createCountryBasicModel));

  // Computed.
  const countryOptions = useMemo<SelectInputOption[]>(() => {
    return [
      { value: "", displayName: "Chưa chọn quốc gia" },
      ...countries.map(country => ({ value: country.code, displayName: country.name }))
    ];
  }, []);

  // Callbacks.
  function handleCountryCodeChanged(code: string): void {
    let country: CountryBasicModel | null = null;
    if (code) {
      country = countries.filter(c => c.code === code)[0];
    }

    props.onModelUpdated({ country });
  }

  // Template.
  return (
    <FormContainer
      upsertAction={props.upsertAction}
      deleteAction={props.deleteAction}
      onUpsertingSucceeded={props.onUpsertingSucceeded}
      onDeletionSucceeded={props.onDeletionSucceeded}
      isModelDirty={isModelDirty}
    >
      <div className="panel">
        <div className="panel-header">
          <span className="panel-header-title">
            Thông tin chi tiết
          </span>
        </div>

        <div className="panel-body flex flex-col gap-3 p-3">
          <FormField path="name" displayName="Tên thương hiệu">
            <TextInput
              required
              value={props.model.name}
              onValueChanged={(name) => props.onModelUpdated({ name })}
              placeholder="Tên thương hiệu"
            />
          </FormField>

          <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-3">
            <FormField path="website">
              <TextInput
                value={props.model.website}
                onValueChanged={(website) => props.onModelUpdated({ website })}
                placeholder="https://website-thuong-hieu.com"
              />
            </FormField>

            <FormField path="socialMediaUrl">
              <TextInput
                value={props.model.socialMediaUrl}
                onValueChanged={(socialMediaUrl) => props.onModelUpdated({ socialMediaUrl })}
                placeholder="https://facebook.com/thuong-hieu"
              />
            </FormField>

            <FormField path="phoneNumber">
              <TextInput
                type="tel"
                value={props.model.phoneNumber}
                onValueChanged={(phoneNumber) => props.onModelUpdated({ phoneNumber })}
                placeholder="0123 456 7890"
              />
            </FormField>

            <FormField path="email">
              <TextInput
                type="email"
                value={props.model.email}
                onValueChanged={(email) => props.onModelUpdated({ email })}
                placeholder="email@thuong-hieu.com"
              />
            </FormField>
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
            <FormField path="address">
              <TextInput
                value={props.model.address}
                onValueChanged={(address) => props.onModelUpdated({ address })}
                placeholder="123 Nguyễn Tất Thành"
              />
            </FormField>
            
            <FormField path="country">
              <SelectInput
                options={countryOptions}
                value={props.model.country?.code ?? ""}
                onValueChanged={handleCountryCodeChanged}
              />
            </FormField>
          </div>
        </div>
      </div>

      <div className="flex justify-end gap-3">
        {props.deleteAction && <DeleteButton onClick={props.deleteAction} />}
        <SubmitButton/>
      </div>
    </FormContainer>
  );
};

export default BrandUpsertPage;