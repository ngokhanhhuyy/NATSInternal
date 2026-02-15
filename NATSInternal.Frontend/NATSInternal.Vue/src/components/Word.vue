<script lang="ts">
import type { JSX } from "vue/jsx-runtime";

type Slots = {
  default: () => JSX.Element;
  header: (headerNumbers: number[]) => JSX.Element; 
}
</script>

<script setup lang="tsx">
let count = $ref(0);

const props = defineProps<a
const slots = defineSlots<Slots>();

const map = new Map();

defineRender(
  <div class="panel">
    <div class="header"></div>
  </div>
);
</script>

<style scoped>
.panel {
  display: flex;
  flex-direction: column;
}
</style>