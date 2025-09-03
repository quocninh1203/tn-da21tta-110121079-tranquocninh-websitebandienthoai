<template>
  <div v-if="isVisible" class="modal-overlay" @click="handleOverlayClick">
    <div class="modal-container" @click.stop>
      <!-- Header -->
      <div class="modal-header">
        <div class="modal-icon" :class="iconClass">
          <svg-icon type="mdi" :path="iconPath" class="icon"></svg-icon>
        </div>
        <h3 class="modal-title">{{ title }}</h3>
      </div>

      <!-- Content -->
      <div class="modal-content">
        <p class="modal-message">{{ message }}</p>
        <div v-if="details" class="modal-details">
          {{ details }}
        </div>
      </div>

      <!-- Actions -->
      <div class="modal-actions">
        <button 
          class="btn btn-secondary" 
          @click="handleCancel"
          :disabled="isLoading"
        >
          <svg-icon type="mdi" :path="mdiClose" class="btn-icon"></svg-icon>
          {{ cancelText }}
        </button>
        <button 
          class="btn btn-primary" 
          :class="confirmButtonClass"
          @click="handleConfirm"
          :disabled="isLoading"
        >
          <div v-if="isLoading" class="loading-spinner"></div>
          <svg-icon v-else type="mdi" :path="confirmIconPath" class="btn-icon"></svg-icon>
          {{ isLoading ? 'Đang xử lý...' : confirmText }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import SvgIcon from '@jamescoyle/vue-icon';
import { 
  mdiClose, 
  mdiCheck, 
  mdiAlert, 
  mdiDelete, 
  mdiInformation,
  mdiHelpCircle
} from '@mdi/js';

const props = defineProps({
  // Visibility
  modelValue: {
    type: Boolean,
    default: false
  },
  
  // Content
  title: {
    type: String,
    default: 'Xác nhận'
  },
  message: {
    type: String,
    required: true
  },
  details: {
    type: String,
    default: ''
  },
  
  // Buttons
  confirmText: {
    type: String,
    default: 'Xác nhận'
  },
  cancelText: {
    type: String,
    default: 'Hủy'
  },
  
  // Styling
  type: {
    type: String,
    default: 'warning', // warning, danger, info, success
    validator: (value) => ['warning', 'danger', 'info', 'success'].includes(value)
  },
  
  // Behavior
  closeOnOverlay: {
    type: Boolean,
    default: true
  },
  loading: {
    type: Boolean,
    default: false
  }
});

const emit = defineEmits(['update:modelValue', 'confirm', 'cancel']);

const isLoading = ref(false);

const isVisible = computed({
  get: () => props.modelValue,
  set: (value) => emit('update:modelValue', value)
});

const iconPath = computed(() => {
  const iconMap = {
    warning: mdiAlert,
    danger: mdiDelete,
    info: mdiInformation,
    success: mdiCheck
  };
  return iconMap[props.type] || mdiHelpCircle;
});

const iconClass = computed(() => `icon-${props.type}`);

const confirmButtonClass = computed(() => `btn-${props.type}`);

const confirmIconPath = computed(() => {
  return props.type === 'danger' ? mdiDelete : mdiCheck;
});

const handleConfirm = async () => {
  isLoading.value = true;
  try {
    await emit('confirm');
  } finally {
    isLoading.value = false;
  }
};

const handleCancel = () => {
  emit('cancel');
  isVisible.value = false;
};

const handleOverlayClick = () => {
  if (props.closeOnOverlay && !isLoading.value) {
    handleCancel();
  }
};
</script>

<style scoped>
/* CSS Variables */
:root {
  --primary-color: #e67e22;
  --success-color: #27ae60;
  --warning-color: #f39c12;
  --danger-color: #e74c3c;
  --info-color: #3498db;
  --text-dark: #2c3e50;
  --text-medium: #34495e;
  --text-light: #7f8c8d;
  --background-soft: #fdf6f0;
  --border-light: #ecf0f1;
  --shadow-modal: rgba(0, 0, 0, 0.5);
}

/* Modal overlay */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: var(--shadow-modal);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  animation: fadeIn 0.3s ease;
}

.modal-container {
  background: white;
  border-radius: 16px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  max-width: 500px;
  width: 90%;
  max-height: 90vh;
  overflow: hidden;
  animation: slideIn 0.3s ease;
}

/* Modal header */
.modal-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 30px 30px 20px;
  text-align: center;
}

.modal-icon {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 20px;
}

.icon {
  width: 40px;
  height: 40px;
  color: white;
}

.icon-warning {
  background: linear-gradient(135deg, var(--warning-color), #f1c40f);
}

.icon-danger {
  background: linear-gradient(135deg, var(--danger-color), #c0392b);
}

.icon-info {
  background: linear-gradient(135deg, var(--info-color), #2980b9);
}

.icon-success {
  background: linear-gradient(135deg, var(--success-color), #2ecc71);
}

.modal-title {
  margin: 0;
  font-size: 24px;
  font-weight: 700;
  color: var(--text-dark);
}

/* Modal content */
.modal-content {
  padding: 0 30px 20px;
  text-align: center;
}

.modal-message {
  margin: 0 0 15px 0;
  font-size: 16px;
  line-height: 1.5;
  color: var(--text-medium);
}

.modal-details {
  padding: 15px;
  background: var(--background-soft);
  border-radius: 8px;
  font-size: 14px;
  color: var(--text-light);
  border-left: 4px solid var(--primary-color);
}

/* Modal actions */
.modal-actions {
  display: flex;
  gap: 12px;
  padding: 20px 30px 30px;
}

.btn {
  flex: 1;
  padding: 12px 24px;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  font-size: 16px;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  min-height: 48px;
}

.btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
  transform: none !important;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover:not(:disabled) {
  background: #5a6268;
  transform: translateY(-2px);
}

.btn-primary {
  background: linear-gradient(135deg, var(--primary-color), #d35400);
  color: white;
}

.btn-warning {
  background: linear-gradient(135deg, var(--warning-color), #f1c40f);
  color: white;
}

.btn-danger {
  background: linear-gradient(135deg, var(--danger-color), #c0392b);
  color: white;
}

.btn-info {
  background: linear-gradient(135deg, var(--info-color), #2980b9);
  color: white;
}

.btn-success {
  background: linear-gradient(135deg, var(--success-color), #2ecc71);
  color: white;
}

.btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
}

.btn-icon {
  width: 18px;
  height: 18px;
}

/* Loading spinner */
.loading-spinner {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top: 2px solid white;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

/* Animations */
@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(-50px) scale(0.9);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

/* Responsive */
@media (max-width: 768px) {
  .modal-container {
    width: 95%;
    margin: 20px;
  }
  
  .modal-header,
  .modal-content,
  .modal-actions {
    padding-left: 20px;
    padding-right: 20px;
  }
  
  .modal-actions {
    flex-direction: column;
  }
  
  .modal-title {
    font-size: 20px;
  }
  
  .modal-icon {
    width: 60px;
    height: 60px;
  }
  
  .icon {
    width: 30px;
    height: 30px;
  }
}
</style>
