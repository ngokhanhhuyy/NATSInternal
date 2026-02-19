<script lang="ts">
  // 1. Reactivity with $state
  let email = $state("");
  let password = $state("");
  let status = $state("idle"); // 'idle' | 'loading' | 'success' | 'error'
  let showPassword = $state(false);

  // 2. Derived state (Computed)
  let isFormValid = $derived(email.includes("@") && password.length >= 8);

  // 3. Props & Snippets (Replacement for slots in Svelte 5)
  let { header, footer, providers = ["Google", "GitHub"] } = $props();

  // 4. Async logic
  async function handleSignIn(e: SubmitEvent) {
    e.preventDefault();
    status = "loading";
    
    // Simulate API call
    const promise = new Promise((resolve, reject) => {
      setTimeout(() => {
        Math.random() > 0.3 ? resolve({ user: "Gemini" }) : reject("Invalid credentials");
      }, 2000);
    });

    try {
      await promise;
      status = "success";
    } catch (err) {
      status = "error";
    }
  }
</script>

<div class="auth-card">
  <header>
    {@render header?.()}
  </header>

  <form onsubmit={handleSignIn}>
    <div class="input-group">
      <label for="email">Email</label>
      <input 
        id="email" 
        type="email" 
        bind:value={email} 
        placeholder="you@example.com"
      />
    </div>

    <div class="input-group">
      <label for="password">Password</label>
      <div class="password-wrapper">
        <input 
          id="password" 
          type={showPassword ? "text" : "password"} 
          bind:value={password}
        />
        <button type="button" onclick={() => showPassword = !showPassword}>
          {showPassword ? "🙈" : "👁️"}
        </button>
      </div>
    </div>

    <button 
      type="submit" 
      disabled={!isFormValid || status === "loading"}
      class:loading={status === "loading"}
      class:btn-success={status === "success"}
    >
      {#if status === "loading"}
        Signing in...
      {:else if status === "success"}
        Welcome Back!
      {:else}
        Sign In
      {/if}
    </button>
  </form>

  <hr />

  <div class="providers">
    <p>Or continue with:</p>
    {#each providers as provider, i}
      <button class="provider-btn" onclick={() => console.log(provider)}>
        {i + 1}. {provider}
      </button>
    {/each}
  </div>

  {#if status === "error"}
    <p class="error-msg">❌ Something went wrong. Please try again.</p>
  {/if}

  <footer>
    {@render footer?.({ email })}
  </footer>
</div>

<style>
  .auth-card {
    max-width: 400px;
    margin: 2rem auto;
    padding: 2rem;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0,0,0,0.1);
    font-family: sans-serif;
  }

  .input-group { margin-bottom: 1rem; }
  
  /* Class binding styles */
  button {
    width: 100%;
    padding: 0.8rem;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    transition: all 0.2s;
  }

  .loading { opacity: 0.7; cursor: wait; background: #666; }
  .btn-success { background: #2ecc71; color: white; }
  
  .error-msg { color: #e74c3c; font-size: 0.9rem; margin-top: 1rem; }
  
  .providers { display: flex; flex-direction: column; gap: 0.5rem; margin-top: 1rem; }
</style>