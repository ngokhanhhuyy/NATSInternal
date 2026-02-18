<script lang="tsx">
import { defineComponent, type PropType, type SetupContext, type SlotsType } from "vue";
import type { JSX } from "vue/jsx-runtime";
export type Model = {
  userName: string;
  roleId: number;
};

type Props = { model: Model };
type Emits = {
  modelChanged(newModel: Model): void;
};
type Slots = SlotsType<{
  default?: () => any;
  header?: (model: Model) => any;
}>;

const User = defineComponent<Props, Emits, string, Slots>((props, { emit, slots }) => {
  return () => (
    <>
      <input
        type="text"
        value={props.model.userName}
        onInput={(e) => emit("modelChanged", { ...props.model, userName: (e.target as HTMLInputElement).value })}
      />
      {slots.header?.(props.model)}
      {slots.default?.()}
    </>
  );
});

export default User;

const UserList = () => {
  return () => (
    <User model={{ userName: "Huy", roleId: 1 }}>
      <span></span>
    </User>
  );
};
</script>