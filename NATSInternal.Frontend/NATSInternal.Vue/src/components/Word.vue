<script setup lang="tsx">
import { useSlots } from "vue";
import { JSX } from "vue/jsx-runtime";

let count = $ref(0);

const countDouble = () => <h2>{count ** 2}</h2>;
const props = defineProps<{
  renderFooter(): JSX.Element;
}>();

defineRender(
  <div>
    <h1>MyName: {count || "Notset"}</h1>
    <button type="button" onClick={() => count++}>Increment</button>
    {countDouble()}
    {props.renderFooter()}
  </div>
);
</script>