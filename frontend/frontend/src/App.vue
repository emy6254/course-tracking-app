<template>
  <div id="app">
    <nav class="navbar" v-if="isLoggedIn">
      <div class="navbar-menu">
        <div class="navbar-end">
          <span class="navbar-item">Zdravo, {{ currentUser?.firstName }}</span>
          <button class="button is-light navbar-item" @click="logout">Odjava</button>
        </div>
      </div>
    </nav>
    <router-view />
  </div>
</template>

<script>
export default {
  computed: {
    isLoggedIn() { return this.$store.getters.isLoggedIn },
    currentUser() { return this.$store.getters.currentUser }
  },
  methods: {
    logout() {
      this.$store.dispatch('logout')
      this.$router.push('/login')
    }
  },
  
  }

</script>


<style>
@import 'https://cdn.jsdelivr.net/npm/bulma@0.9.3/css/bulma.min.css';

.mt-4 {
  margin-top: 1.5rem;
}

.card {
  margin-bottom: 1.5rem;
}
</style>
