<script setup lang="tsx">
import type { JSX } from "vue/jsx-runtime";
export type Model = {
  userName: string;
  roleId: number;
};

const props = defineProps<{
  model: Model;
}>();

const emit = defineEmits<{
  (event: "modelChanged", newModel: Model): void;
}>();

const slots = defineSlots<{
  header(scoped: { model: Model }): JSX.Element;
  default(): void;
}>

defineRender(
  <>
    <input
      type="text"
      value={props.model.userName}
      onInput={(e) => emit("modelChanged", { ...props.model, userName: (e.target as HTMLInputElement).value })}
    />
    {slots().header({ model: props.model })}
    <slot name="header" {...{ model: props.model }}></slot>
  </>
);
</script>