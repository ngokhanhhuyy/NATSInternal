import { defineComponent, type PropType, type SetupContext, type SlotsType, type DefineComponent } from "vue";
import type { JSX } from "vue/jsx-runtime";
export type Model = {
  userName: string;
  roleId: number;
};

type Props = { models: Model };
type Emits = {
  modelChanged(newModel: Model): void;
};
type Slots = SlotsType<{
  header?: (model: Model) => any;
}>;

const User = defineComponent<Props, Emits, string, Slots>((props, { emit, slots }) => {
  return () => (
    <>
      <input
        type="text"
        value={props.models.userName}
        onInput={(e) => emit("modelChanged", { ...props.models, userName: (e.target as HTMLInputElement).value })}
      />
      {slots.header?.(props.models)}
    </>
  );
});

type UserFakeComponent = DefineComponent<{}, {}, {
  default?: (model: Model) => any;
}>;

const UserFake: UserFakeComponent = null as unknown as UserFakeComponent;

export default User;

const UserList = defineComponent(() => {
  return () => (
    <UserFake v-slots={{
            default: (model) => <></>
        }}/>
  );
});