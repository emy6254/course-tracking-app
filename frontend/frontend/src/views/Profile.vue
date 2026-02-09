<template>
  <div class="profile container mt-5">
    <div class="box">
      <h2 class="title is-3 has-text-centered">Moj Profil</h2>

      <div v-if="loading" class="has-text-centered my-5">
        <progress class="progress is-small is-primary" max="100">Učitavanje...</progress>
      </div>

      <div v-else-if="user">
        <article class="media">
          <div class="media-content">
            <div class="content">
              <p><strong>Ime:</strong> {{ user.firstName }}</p>
              <p><strong>Prezime:</strong> {{ user.lastName }}</p>
              <p><strong>Email:</strong> {{ user.email }}</p>
              <p><strong>Korisničko ime:</strong> {{ user.username }}</p>
              <p><strong>Uloga:</strong> {{ user.role }}</p>
            </div>
          </div>
        </article>

        <hr />

        <div v-if="user?.role !== 'Admin'">
       <h3 class="subtitle mt-4">Moji Kursevi</h3>
       <div v-if="enrolledCourses.length === 0" class="notification is-light">
         Niste prijavljeni ni na jedan kurs.
       </div>
      <div v-else>
        <ul>
        <li v-for="course in enrolledCourses" :key="course.id" class="mb-2">
         <strong>{{ course.title }}</strong> — {{ course.level }} nivo ({{ course.duration }}h)
         <button class="button is-danger is-small ml-2" @click="cancelEnrollment(course.id)">
            Otkaži prijavu
         </button>
        </li>
      </ul>
   </div>
  </div>


        <h3 class="subtitle mt-4">Izmena podataka</h3>
        <form @submit.prevent="updateProfile">
          <div class="field">
            <label class="label">Ime</label>
            <input v-model="form.firstName" class="input" type="text" required />
          </div>
          <div class="field">
            <label class="label">Prezime</label>
            <input v-model="form.lastName" class="input" type="text" required />
          </div>
          <div class="field">
            <label class="label">Email</label>
            <input v-model="form.email" class="input" type="email" required />
          </div>
          <div class="field is-grouped mt-4">
            <button class="button is-success">Sačuvaj izmene</button>
            <router-link to="/courses" class="button is-link">Idi na kurseve</router-link>
          </div>
        </form>
      </div>

      <div v-else class="notification is-warning mt-5">
        Nema podataka o korisniku.
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { useStore } from 'vuex'
import { api } from '@/store'

const store = useStore()
const loading = ref(true)
const user = ref(null)

const form = ref({
  firstName: '',
  lastName: '',
  email: ''
})
const enrolledCourses = ref([])

const updateProfile = async () => {
  try {
    await store.dispatch('updateProfile', form.value)
    alert('Profil uspešno ažuriran')
  } catch {
    alert('Greška pri ažuriranju profila')
  }
}

const cancelEnrollment = async (id) => {
  if (!confirm("Da li sigurno želite da otkažete prijavu?")) return;
  try {
    await api.delete(`/courses/unenroll/${id}`);
    alert("Prijava otkazana.");
    enrolledCourses.value = enrolledCourses.value.filter(c => c.id !== id);
  } catch (err) {
    alert(err.response?.data?.message || "Greška");
  }
};

onMounted(async () => {
  try {
    await store.dispatch('fetchCurrentUser');
    user.value = store.getters.currentUser;
    form.value.firstName = user.value.firstName;
    form.value.lastName = user.value.lastName;
    form.value.email = user.value.email;

    const enrolledRes = await api.get("/users/enrollments");
    enrolledCourses.value = enrolledRes.data || [];
  } catch (err) {
    console.error("Neuspešno učitavanje profila ili kurseva:", err);
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.profile {
  max-width: 700px;
  margin: 0 auto;
}
</style>
