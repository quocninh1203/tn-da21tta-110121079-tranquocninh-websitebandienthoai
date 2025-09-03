import axios from 'axios';
import Cookies from 'js-cookie';
import { getUserId, getAccessToken } from '@/utils/jwtHelper';
import authService from './authService';
import { useAuthStore } from '@/stores/api/authStore';

const api = axios.create({
    baseURL: 'https://localhost:5000/api',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach(({ resolve, reject }) => {
    if (error) {
      reject(error);
    } else {
      resolve(token);
    }
  });
  failedQueue = [];
};

// Request interceptor
api.interceptors.request.use(
  async (config) => {
    const token = getAccessToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor với refresh token logic
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    
    console.log('Response error:', error.response?.status); // Debug
    console.log('Original request URL:', originalRequest.url); // Debug
    
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      
      const refreshToken = Cookies.get('refreshToken');
      console.log('Refresh token exists:', !!refreshToken); // Debug
      console.log(refreshToken);
      
      
      if (!refreshToken) {
        console.log('No refresh token - logging out');
        // Logout logic
        return Promise.reject(error);
      }
      
      try {
        console.log('Attempting to refresh token...'); // Debug
        
        const refreshResponse = await authService.refreshToken();
        
        console.log('Refresh response:', refreshResponse.data); // Debug
        
        const newAccessToken = refreshResponse.data.model?.accessToken || refreshResponse.data.accessToken;
        
        if (!newAccessToken) {
          throw new Error('No access token in refresh response');
        }
        
        console.log('Got new token, retrying original request'); // Debug
        sessionStorage.setItem('token', newAccessToken);
        
        // Retry original request
        originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
        return api(originalRequest);
        
      } catch (refreshError) {
        console.error('Refresh token failed:', refreshError); // Debug
        
        // Clear tokens và logout
        sessionStorage.removeItem('token');
        Cookies.remove('refreshToken');
        
        const authStore = useAuthStore();
        authStore.logout();
        
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }
    
    return Promise.reject(error);
  }
);


export default api;
