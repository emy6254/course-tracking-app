<template>
  <section class="section">
    <div class="container">
      <h1 class="title is-3 has-text-centered mb-5">Kursevi</h1>

      <div class="has-text-right mb-4">
        <router-link to="/profile" class="button is-light">
          🏠 Moj profil
        </router-link>
      </div>

      <!-- Pretraga i filter -->
      <div class="columns is-variable is-2 mb-4">
        <div class="column is-half">
          <input
            class="input"
            type="text"
            v-model="searchQuery"
            placeholder="Pretraži kurseve po nazivu..."
          />
        </div>
        <div class="column is-one-quarter">
          <div class="select is-fullwidth">
            <select v-model="selectedLevel">
              <option value="">Svi nivoi</option>
              <option value="Beginner">Početni</option>
              <option value="Intermediate">Srednji</option>
              <option value="Advanced">Napredni</option>
            </select>
          </div>
        </div>
      </div>

      <div v-if="loading" class="has-text-centered">
        <div class="spinner"></div>
        <p>Učitavanje...</p>
      </div>

      <div v-if="error" class="notification is-danger">
        {{ error }}
        <button class="button is-small is-danger is-outlined ml-2" @click="loadCourses">
          Pokušaj ponovo
        </button>
      </div>

      <!-- Admin dugmad -->
      <div v-if="isAdmin" class="has-text-right mb-4">
        <button class="button is-primary" @click="openCreateModal">
          ➕ Dodaj kurs
        </button>
      </div>

      <!-- Statistika -->
      <div v-if="isAdmin" class="box mb-5">
        <h3 class="title is-5">📊 Statistika kurseva</h3>
        <p><strong>Ukupno kurseva:</strong> {{ stats.total }}</p>
        <p><strong>Ukupan broj prijava:</strong> {{ stats.totalEnrollments }}</p>
        <p><strong>Prosečna cena:</strong> {{ stats.avgPrice }} RSD</p>
        <p><strong>Prosečno trajanje:</strong> {{ stats.avgDuration }} časova</p>
      </div>

      <!-- Lista kurseva -->
      <div v-if="!loading && !error && filteredCourses.length > 0">
        <div class="columns is-multiline">
          <div class="column is-4" v-for="course in filteredCourses" :key="course.id">
            <div class="card">
              <div class="card-content">
                <p class="title is-5">{{ course.title }}</p>
                <p class="subtitle is-6 has-text-grey">
                  {{ course.level }} | {{ course.duration }} časova
                </p>
                <p>{{ course.description }}</p>
                <hr />
                <p><strong>Instruktor:</strong> {{ course.instructor || "N/A" }}</p>
                <p><strong>Cena:</strong> {{ course.price }} RSD</p>
                <p><strong>Kategorija:</strong> {{ course.category || "N/A" }}</p>
                <p><strong>Prijavljenih:</strong> {{ course.enrolledCount }}</p>
              </div>
              <footer class="card-footer">
                <template v-if="isLoggedIn && !isAdmin">
                  <button
                    v-if="!isEnrolled(course.id)"
                    class="card-footer-item button is-success is-light"
                    @click="enroll(course.id)"
                  >
                    Prijavi se
                  </button>
                  <button
                    v-else
                    class="card-footer-item button is-static is-success is-light"
                    disabled
                  >
                    ✅ Prijavljeno
                  </button>
                </template>
                <template v-else-if="!isLoggedIn">
                  <span class="card-footer-item has-text-grey">Uloguj se za prijavu</span>
                </template>

                <template v-if="isAdmin">
                  <button class="card-footer-item button is-warning is-light" @click="editCourse(course)">
                    ✏️ Uredi
                  </button>
                  <button class="card-footer-item button is-danger is-light" @click="deleteCourse(course.id)">
                    🗑️ Obriši
                  </button>
                </template>
              </footer>
            </div>
          </div>
        </div>
      </div>

      <div v-else-if="!loading && !error" class="notification is-warning has-text-centered">
        Nema dostupnih kurseva.
      </div>

      <!-- Modal forma -->
      <div class="modal" :class="{ 'is-active': showModal }">
        <div class="modal-background" @click="closeModal"></div>
        <div class="modal-card">
          <header class="modal-card-head">
            <p class="modal-card-title">Dodaj/Uredi kurs</p>
            <button class="delete" @click="closeModal"></button>
          </header>
          <section class="modal-card-body">
            <div v-for="(value, key) in form" :key="key" class="field">
              <label class="label">{{ key }}</label>
              <input
                v-model="form[key]"
                class="input"
                :type="['price', 'duration'].includes(key) ? 'number' : 'text'"
              />
            </div>
          </section>
          <footer class="modal-card-foot">
            <button class="button is-success" @click="submitCourse">Sačuvaj</button>
            <button class="button" @click="closeModal">Otkaži</button>
          </footer>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import { useStore } from "vuex";
import { api } from "@/store";

const store = useStore();
const courses = ref([]);
const loading = ref(false);
const error = ref(null);
const showModal = ref(false);

const form = ref({
  title: "",
  description: "",
  instructor: "",
  duration: 4,
  level: "Beginner",
  category: "",
  price: 0,
  imageUrl: "",
});

const isAdmin = computed(() => store.getters.currentUser?.role === "Admin");
const isLoggedIn = computed(() => store.getters.isLoggedIn);

const searchQuery = ref("");
const selectedLevel = ref("");
const enrolledCourses = ref([]);

const isEnrolled = (courseId) => {
  return enrolledCourses.value.includes(courseId);
};

const filteredCourses = computed(() => {
  return courses.value.filter(course =>
    course.title.toLowerCase().includes(searchQuery.value.toLowerCase()) &&
    (selectedLevel.value === "" || course.level === selectedLevel.value)
  );
});

const stats = computed(() => {
  if (courses.value.length === 0) return {
    total: 0, totalEnrollments: 0, avgPrice: 0, avgDuration: 0
  };
  const total = courses.value.length;
  const totalEnrollments = courses.value.reduce((sum, c) => sum + (c.enrolledCount || 0), 0);
  const avgPrice = Math.round(courses.value.reduce((sum, c) => sum + c.price, 0) / total);
  const avgDuration = Math.round(courses.value.reduce((sum, c) => sum + c.duration, 0) / total);
  return { total, totalEnrollments, avgPrice, avgDuration };
});

const loadCourses = async () => {
  loading.value = true;
  error.value = null;
  try {
    const res = await api.get("/courses");
    courses.value = res.data?.courses || [];

    if (isLoggedIn.value && !isAdmin.value) {
      const profile = await api.get("/users");
      enrolledCourses.value = profile.data?.enrolledCourses?.map(c => c.id) || [];
    }
  } catch (err) {
    console.error("Greška sa backend-a:", err);
    if (err.response?.status === 401) {
      error.value = "Nemate dozvolu. Ulogujte se ponovo.";
      store.dispatch("logout");
    } else {
      error.value = `Greška ${err.response?.status || ""}: ${
        err.response?.data?.message || "Nije moguće učitati kurseve"
      }`;
    }
  } finally {
    loading.value = false;
  }
};

const enroll = async (courseId) => {
  if (!isLoggedIn.value) return alert("Uloguj se.");
  try {
    await api.post(`/courses/enroll/${courseId}`);
    alert("Uspešna prijava.");
    await loadCourses();
  } catch (err) {
    alert(err.response?.data?.message || "Greška");
  }
};

const openCreateModal = () => (showModal.value = true);
const closeModal = () => {
  showModal.value = false;
  Object.assign(form.value, {
    title: "",
    description: "",
    instructor: "",
    duration: 4,
    level: "Beginner",
    category: "",
    price: 0,
    imageUrl: "",
  });
};

const submitCourse = async () => {
  try {
    await api.post("/courses", form.value);
    alert("Dodat kurs.");
    closeModal();
    await loadCourses();
  } catch (err) {
    alert(err.response?.data?.message || "Greška");
  }
};

const editCourse = (course) => {
  Object.assign(form.value, course);
  showModal.value = true;
};

const deleteCourse = async (id) => {
  if (!confirm("Da li ste sigurni da želite da obrišete kurs?")) return;
  try {
    await api.delete(`/courses/${id}`);
    alert("Kurs obrisan.");
    await loadCourses();
  } catch (err) {
    alert(err.response?.data?.message || "Greška prilikom brisanja");
  }
};

onMounted(loadCourses);
</script>

<style scoped>
.card {
  transition: box-shadow 0.3s ease;
}
.card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}
.card-footer-item {
  border: none !important;
}
.spinner {
  border: 4px solid #f3f3f3;
  border-top: 4px solid #3498db;
  border-radius: 50%;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
  margin: 0 auto;
}
@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
</style>
