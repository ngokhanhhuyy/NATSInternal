import React from "react";
import { Link } from "react-router";
import { joinClassName } from "@/helpers";

// Child components.
import { NewTabLink } from "@/components/ui";
import { Field } from "@/pages/shared/detail";
import { UserIcon } from "@heroicons/react/24/outline";

// Props.
type DetailPanelProps = {
  model: OrderDetailModel;
};

// Components.
export default function DetailPanel(props: DetailPanelProps): React.ReactNode {
  // Template.
  function renderUser(user: UserBasicModel): React.ReactNode {
    if (user.isDeleted) {
      return (
        <span className="line-through">
          @{user.userName}
        </span>
      );
    }

    return (
      <Link to={user.detailRoute}>
        @{user.userName}
      </Link>
    );
  }

  return (
    <div className="panel">
      <div className="panel-header">
        <span className="panel-header-title">
          Thông tin đơn hàng
        </span>
      </div>

      <div className="panel-body flex flex-col gap-3 p-3">
        <div className="flex flex-col gap-3 flex-1">
          <div className="flex flex-col gap-3">
            <div className="panel-body-area p-3 pt-2">
              <Field name="customer">
                <div className="flex items-center gap-2">
                  <div className="img-thumbnail size-6 flex justify-center items-center text-black dark:text-white">
                    <UserIcon className="size-3 opacity-50" />
                  </div>
                  <div className="flex flex-col">
                    <Link className="text-blue-700 dark:text-blue-400" to={props.model.customer.detailRoutePath}
                    >
                      {props.model.customer.fullName}
                    </Link>
                  </div>
                </div>
              </Field>
            </div>

            <div className="panel-body-area flex flex-col gap-3 p-3 pt-2">
              {/* CreatedUser */}
              <Field name="createdUser">
                {renderUser(props.model.createdUser)}
              </Field>
      
              {/* CreatedDateTime */}
              <Field name="createdDateTime">
                {props.model.displayCreatedDateTime}
              </Field>
      
              {/* LastUpdatedUser */}
              {props.model.lastUpdatedUser && (
                <Field name="lastUpdatedUser">
                  {renderUser(props.model.lastUpdatedUser)}
                </Field>
              )}
      
              {/* LastUpdatedDateTime */}
              {props.model.displayLastUpdatedDateTime && (
                <Field name="lastUpdatedDateTime">
                  {props.model.displayLastUpdatedDateTime}
                </Field>
              )}
      
              {/* DeletedUser */}
              {props.model.deletedUser && (
                <Field name="deletedUser">
                  {renderUser(props.model.deletedUser)}
                </Field>
              )}
      
              {props.model.displayDeletedDateTime && (
                <Field name="deletedDateTime">
                  {props.model.displayDeletedDateTime}
                </Field>
              )}
            </div>
          </div>

          <div className={joinClassName(
            "panel-body-area p-3",
            !props.model.note ? "flex justify-center items-center flex-1" : "pt-2"
          )}>
            {props.model.note ? (
              <Field name="note">
                {props.model.note}
              </Field>
            ) : (
              <span className="opacity-50">Không có ghi chú</span>
            )}
          </div>
        
          <div className={joinClassName(
            "panel-body-area flex flex-wrap gap-2 p-3",
            !props.model.photos.length && "justify-center items-center flex-1"
          )}>
            {props.model.photos.length ? props.model.photos.map(photo => (
              <NewTabLink url={photo.url} key={photo.id}>
                <img className="img-thumbnail size-16" src={photo.url}/>
              </NewTabLink>
            )) : (
              <div className="flex justify-center items-center opacity-50">
                Không có hình ảnh
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
