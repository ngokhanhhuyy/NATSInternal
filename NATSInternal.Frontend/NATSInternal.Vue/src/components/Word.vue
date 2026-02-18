<script setup lang="tsx">
import { ref, useSlots } from "vue";
import UserTSX, { type Model } from "./testing/UserTSX.vue";

const a = 1 as number | null;
const models: Model[] = [];
const template = (

  <UserTSX for={(user, index) in models} models={user}>
    {(scoped) => <></>}
  </UserTSX>
);
</script>

<style scoped>
.panel {
  display: flex;
  flex-direction: column;
}
</style>