import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useLoadingStore = defineStore('loading', () => {
  const loading = ref(true);

  const loadingPage = () => {
    loading.value = true;
    
    setTimeout(() => {
      loading.value = false
    }, 500);
  };
  const loadingComponent = () => {
    loading.value = true;

    setTimeout(() => {
      loading.value = false
    }, 200);
  };

  return {
    loading,
    loadingPage,
    loadingComponent
  };
});
