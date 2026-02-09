<template>
  <div class="register box">
    <h1 class="title has-text-centered mb-4">Registracija</h1>
    <form @submit.prevent="register">
      <div class="field">
        <label class="label">Korisničko ime</label>
        <div class="control">
          <input class="input" type="text" v-model="form.username" required />
        </div>
      </div>

      <div class="field">
        <label class="label">Ime</label>
        <div class="control">
          <input class="input" type="text" v-model="form.firstName" required />
        </div>
      </div>

      <div class="field">
        <label class="label">Prezime</label>
        <div class="control">
          <input class="input" type="text" v-model="form.lastName" required />
        </div>
      </div>

      <div class="field">
        <label class="label">Email</label>
        <div class="control">
          <input class="input" type="email" v-model="form.email" required />
        </div>
      </div>

      <div class="field">
        <label class="label">Lozinka</label>
        <div class="control">
          <input class="input" type="password" v-model="form.password" required />
        </div>
      </div>

      <div class="field">
        <label class="label">Potvrdi lozinku</label>
        <div class="control">
          <input class="input" type="password" v-model="form.confirmPassword" required />
        </div>
        <div class="field">
  <label class="label">Uloga</label>
  <div class="control">
    <div class="select">
      <select v-model="form.role" required>
        <option value="Student">Student</option>
        <option value="Admin">Admin</option>
      </select>
    </div>
  </div>
</div>

      </div>

      <div v-if="error" class="notification is-danger is-light">{{ error }}</div>

      <div class="field is-grouped mt-4">
        <div class="control">
          <button class="button is-primary" :disabled="loading">
            <span v-if="loading" class="spinner-border spinner-border-sm"></span>
            Registruj se
          </button>
        </div>
        <div class="control">
          <router-link to="/login" class="button is-light">Imate nalog?</router-link>
        </div>
      </div>
    </form>
  </div>
</template>

<script>
export default {
  name: 'Register',
  data() {
    return {
      form: {
        username: '',
        email: '',
        firstName: '',
        lastName: '',
        password: '',
        confirmPassword: '',
        role: 'Student' // Default role
      },
      error: null,
      loading: false
    };
  },
  methods: {
    async register() {
      this.error = null;

      if (this.form.password !== this.form.confirmPassword) {
        this.error = 'Lozinke se ne poklapaju.';
        return;
      }

      this.loading = true;

      try {
        await this.$store.dispatch('register', {
          username: this.form.username,
          email: this.form.email,
          firstName: this.form.firstName,
          lastName: this.form.lastName,
          password: this.form.password,
          role: this.form.role
        });

        this.$router.push('/login');
      } catch (err) {
        this.error = this.$store.getters.error || 'Došlo je do greške. Pokušajte ponovo.';
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style scoped>
.register {
  max-width: 500px;
  margin: 60px auto;
  padding: 30px;
}
</style>
