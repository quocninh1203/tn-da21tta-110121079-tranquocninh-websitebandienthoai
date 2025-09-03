<template>
  <div class="modal-backdrop">
    <div class="form-variant-modal">
      <h2>{{ isEdit ? 'Sửa biến thể' : 'Thêm biến thể' }}</h2>
      <form @submit.prevent="handleSubmit">
        <div class="form-row">
          <div class="form-group">
            <label>Màu sắc</label>
            <select v-model="form.colorId" required>
              <option value="">Chọn màu</option>
              <option v-for="color in colors" :key="color.id" :value="color.id">{{ color.name }}</option>
            </select>
          </div>
          <div class="form-group">
            <label>RAM</label>
            <select v-model="form.ramId" required>
              <option value="">Chọn RAM</option>
              <option v-for="ram in rams" :key="ram.id" :value="ram.id">{{ ram.size }}</option>
            </select>
          </div>
          <div class="form-group">
            <label>ROM</label>
            <select v-model="form.storageId" required>
              <option value="">Chọn ROM</option>
              <option v-for="storage in storages" :key="storage.id" :value="storage.id">{{ storage.size }}</option>
            </select>
          </div>
        </div>
        <div class="form-row">
          <div class="form-group">
            <label>Giá (VND)</label>
            <input type="number" v-model="form.price" min="0" required />
          </div>
          <div class="form-group">
            <label>Số lượng tồn kho</label>
            <input type="number" v-model="form.stockQuantity" min="0" required />
          </div>
        </div>
        <div class="form-actions">
          <button type="submit" class="btn btn-green">{{ isEdit ? 'Lưu' : 'Thêm' }}</button>
          <button type="button" class="btn btn-orange" @click="emit('close')">Hủy</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from "vue";

const props = defineProps({
  isEdit: Boolean,
  variant: Object,
  phoneId: Number,
  colors: {
    type: Array,
    default: () => []
  },
  rams: {
    type: Array,
    default: () => []
  },
  storages: {
    type: Array,
    default: () => []
  }
});
const emit = defineEmits(["submit", "close"]);

const form = ref({
  phoneId: props.phoneId || "",
  colorId: "",
  ramId: "",
  storageId: "",
  price: "",
  stockQuantity: ""
});

watch(
  () => [props.variant, props.phoneId, props.colors, props.rams, props.storages],
  ([variant, phoneId, colors, rams, storages]) => {
    if (props.isEdit && variant) {
      // Tìm id theo name/size
      const colorId = colors.find(c => c.name === variant.colorName)?.id || "";
      const ramId = rams.find(r => r.size === variant.ramSize)?.id || "";
      const storageId = storages.find(s => s.size === variant.storageSize)?.id || "";
      form.value = {
        phoneId: phoneId || "",
        colorId,
        ramId,
        storageId,
        price: variant.price || "",
        stockQuantity: variant.stockQuantity || ""
      };
    } else {
      form.value = {
        phoneId: phoneId || "",
        colorId: "",
        ramId: "",
        storageId: "",
        price: "",
        stockQuantity: ""
      };
    }
  },
  { immediate: true }
);

function handleSubmit() {
  emit("submit", { ...form.value });
}
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  z-index: 1000;
  inset: 0;
  background: rgba(0, 0, 0, 0.25);
  display: flex;
  align-items: center;
  justify-content: center;
}
.form-variant-modal {
  background: #fff;
  padding: 32px 32px 24px 32px;
  border-radius: 18px;
  max-width: 500px;
  width: 100%;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.18);
  position: relative;
}
h2 {
  margin-bottom: 18px;
  font-size: 24px;
  color: #5f3864;
  font-weight: 600;
}
.form-row {
  display: flex;
  gap: 16px;
  margin-bottom: 16px;
}
.form-group {
  flex: 1 1 0;
  display: flex;
  flex-direction: column;
  margin-bottom: 0;
}
.form-group label {
  font-weight: 500;
  color: #e65100;
  margin-bottom: 6px;
}
.form-group input,
.form-group select {
  border: 1.5px solid #ffe0b2;
  border-radius: 8px;
  padding: 8px 12px;
  font-size: 16px;
  background: #fff8f1;
  outline: none;
  transition: border 0.2s;
}
.form-group input:focus,
.form-group select:focus {
  border-color: #fb8c00;
}
.form-actions {
  display: flex;
  gap: 18px;
  justify-content: center;
  margin-top: 10px;
}
.btn {
  border: none;
  border-radius: 8px;
  font-weight: 600;
  font-size: 17px;
  padding: 8px 32px;
  cursor: pointer;
  transition: background 0.18s;
}
.btn-green {
  background: #43a047;
  color: #fff;
}
.btn-green:hover {
  background: #2e7d32;
}
.btn-orange {
  background: #fff;
  color: #e65100;
  border: 2px solid #e65100;
}
.btn-orange:hover {
  background: #ffe0b2;
}
</style>
