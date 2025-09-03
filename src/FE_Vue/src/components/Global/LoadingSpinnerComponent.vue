<template>
    <div v-if="loading" class="loading-overlay">
      <div class="loading-spinner"></div>
    </div>
  </template>
  
  <script setup>
  defineProps({
    loading: {
      type: Boolean,
      required: true
    }
  })
  </script>
  <!-- position: absolute; -->
  <style scoped>
  .loading-overlay {
    background-color: #000;
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999;
    height: 100%;
    width: 100%;
  }
  
  .loading-spinner {
    border: 5px solid rgba(0, 0, 0, 0.1);
    border-top: 5px solid #f00;
    border-radius: 50%;
    width: 50px;
    height: 50px;
    animation: spin 1s linear infinite;
  }
  
  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }
  </style>
  