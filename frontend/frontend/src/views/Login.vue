<template>
  <div class="login">
    <h1 class="title">Prijava</h1>
    <form @submit.prevent="submitLogin">
      <div class="field">
        <label class="label">Korisničko ime</label>
        <div class="control">
          <input class="input" type="text" v-model="username" required />
        </div>
      </div>

      <div class="field">
        <label class="label">Lozinka</label>
        <div class="control">
          <input class="input" type="password" v-model="password" required />
        </div>
      </div>

      <div v-if="error" class="notification is-danger">
        {{ error }}
      </div>

      <div class="control">
        <button class="button is-primary" type="submit" :disabled="loading">
          {{ loading ? 'Prijavljivanje...' : 'Prijavi se' }}
        </button>
      </div>
    </form>

    <!-- Poruka ispod forme -->
    <div class="has-text-centered mt-4">
      <p>
        Nemate nalog?
        <router-link to="/register">Registrujte se</router-link>
      </p>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      username: '',
      password: '',
      loading: false,
      error: null
    }
  },
  methods: {
    async submitLogin() {
      this.loading = true;
      this.error = null;
      try {
        await this.$store.dispatch('login', {
          username: this.username,
          password: this.password
        });
        this.$router.push('/');
      } catch (err) {
        this.error = this.$store.state.error || 'Došlo je do greške pri prijavi.';
      } finally {
        this.loading = false;
      }
    }
  }
}
</script>

<style scoped>
.login {
  max-width: 400px;
  margin: 50px auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 10px;
  background-color: white;
}
.mt-4 {
  margin-top: 1.5rem;
}
</style>
