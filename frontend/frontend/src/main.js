import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

import Toast from 'vue-toastification'
import 'vue-toastification/dist/index.css'

const toastOptions = {
  position: 'top-right',
  timeout: 3000,
  closeOnClick: true,
  pauseOnHover: true,
  draggable: true
}

const app = createApp(App)
app.use(store)
app.use(router)
app.use(Toast, toastOptions)
app.mount('#app')
