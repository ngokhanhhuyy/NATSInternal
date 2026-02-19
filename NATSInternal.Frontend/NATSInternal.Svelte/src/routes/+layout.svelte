<script lang="ts">
  import "./layout.css";
  import "@/assets/css/style.css";

  let { children } = $props();
</script>

<!-- <svelte:head>
  <link rel="icon" href={favicon} />
</svelte:head> -->

<div id="root-layout">
  {@render children()}
</div>
