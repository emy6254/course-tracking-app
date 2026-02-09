import { createStore } from 'vuex'
import axios from 'axios'
import router from '@/router'

export const api = axios.create({
  baseURL: 'http://localhost:5129/api',
  headers: { 'Content-Type': 'application/json' }
})

// Interceptor za dodavanje tokena
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
}, error => Promise.reject(error))

// Interceptor za 401
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      if (router.currentRoute.value.path !== '/login') {
        router.push('/login')
      }
    }
    return Promise.reject(error)
  }
)

const store = createStore({
  state: {
    token: localStorage.getItem('token') || '',
    user: JSON.parse(localStorage.getItem('user')) || null,
    loading: false,
    error: null
  },
  getters: {
    isLoggedIn: state => !!state.token,
    currentUser: state => state.user,
    isAdmin: state => state.user?.role === 'Admin'
  },
  mutations: {
    SET_LOADING(state, value) { state.loading = value },
    SET_USER(state, user) {
      state.user = user
      localStorage.setItem('user', JSON.stringify(user))
    },
    SET_TOKEN(state, token) {
      state.token = token
      localStorage.setItem('token', token)
    },
    SET_ERROR(state, error) { state.error = error },
    LOGOUT(state) {
      state.user = null
      state.token = ''
      localStorage.removeItem('token')
      localStorage.removeItem('user')
    }
  },
  actions: {
    async login({ commit }, credentials) {
      commit('SET_LOADING', true)
      try {
        const res = await api.post('/auth/login', credentials)
        commit('SET_TOKEN', res.data.token)
        commit('SET_USER', res.data.user)
        router.push('/courses')
      } catch (err) {
        commit('SET_ERROR', err.response?.data?.message || 'Login failed')
        throw err
      } finally {
        commit('SET_LOADING', false)
      }
    },
    async register({ commit }, userData) {
      commit('SET_LOADING', true)
      try {
        await api.post('/auth/register', userData)
        router.push('/login')
      } catch (err) {
        commit('SET_ERROR', err.response?.data?.message || 'Registration failed')
        throw err
      } finally {
        commit('SET_LOADING', false)
      }
    },
    logout({ commit }) {
      commit('LOGOUT')
      router.push('/login')
    }
  }
})

export default store