import axios from 'axios';

const api = axios.create({
  baseURL: '/api',   // ← relativo: nginx lo reenvía a backend:8080/api
  headers: {
    'Content-Type': 'application/json'
  }
});

// Recogemos token y lo añadimos en cada petición
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;