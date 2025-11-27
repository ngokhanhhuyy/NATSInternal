<script lang="ts">
  let count = $state(0);
  const array: number[] = $state([]);
</script>

<h1>Welcome to SvelteKit</h1>
<p>Visit <a href="https://svelte.dev/docs/kit">svelte.dev/docs/kit</a> to read the documentation</p>
<span>{count}</span>
<button onclick={() => count++} type="button">
  Increment
</button>
<br/>
<span>List: {array.join(", ")}</span>
<button type="button" onclick={() => array.push(count)}>
  Add
</button>

<style>
  @reference "tailwindcss";
  button {
    @apply bg-white border border-black/10 px-2 py-0.5 rounded-lg shadow-xs cursor-pointer
      hover:bg-black/2 hover:border-black/15;
  }
</style>
