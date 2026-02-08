<script lang="ts">
  import "./layout.css";
  import "../../static/style.css";
  import favicon from "$lib/assets/favicon.svg";

  let { children } = $props();

  export const ssr = false;
</script>

<svelte:head>
  <link rel="icon" href={favicon} />
</svelte:head>

{@render children()}
