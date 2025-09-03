<template>
  <div class="modal-backdrop">
    <div class="form-image-modal">
      <h2>Thêm hình ảnh sản phẩm</h2>
      <form @submit.prevent="handleSubmit">
        <div class="form-group switch-group">
          <button
            type="button"
            :class="['switch-btn', mode === 'file' ? 'active' : '']"
            @click="setMode('file')"
          >
            Ảnh từ máy
          </button>
          <button
            type="button"
            :class="['switch-btn', mode === 'remote' ? 'active' : '']"
            @click="setMode('remote')"
          >
            Ảnh từ internet
          </button>
        </div>
        <div v-if="mode === 'file'" class="form-group">
          <label class="form-label">Chọn ảnh từ máy</label>
          <input type="file" accept="image/*" @change="onFileChange" class="form-input" />
        </div>
        <div v-if="mode === 'remote'" class="form-group">
          <label class="form-label">Nhập địa chỉ ảnh</label>
          <input type="text" v-model="remoteUrl" class="form-input" placeholder="https://example.com/image.jpg" />
        </div>
        <div v-if="previewUrl" class="form-group preview-group">
          <label class="form-label">Xem trước ảnh</label>
          <img :src="previewUrl" alt="Preview" class="preview-img" />
        </div>
        <div class="form-actions">
          <button type="submit" class="btn btn-green">Lưu</button>
          <button type="button" class="btn btn-orange" @click="emit('close')">Hủy</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from "vue";
import { convertToBase64 } from "@/utils/formatters";

const emit = defineEmits(["submit", "close"]);
const file = ref(null);
const remoteUrl = ref("");
const previewUrl = ref("");
const mode = ref("file");

function setMode(val) {
  mode.value = val;
  file.value = null;
  remoteUrl.value = "";
  previewUrl.value = "";
}

async function onFileChange(e) {
  const selected = e.target.files[0];
  file.value = selected;
  if (selected) {
    previewUrl.value = await convertToBase64(selected);
  } else {
    previewUrl.value = "";
  }
}

watch(remoteUrl, async (val) => {
  if (mode.value === "remote" && val) {
    try {
      const response = await fetch(val);
      const blob = await response.blob();
      previewUrl.value = await convertToBase64(blob);
      file.value = null;
    } catch {
      previewUrl.value = "";
    }
  } else if (mode.value === "remote") {
    previewUrl.value = "";
  }
});

function handleSubmit() {
  if (previewUrl.value) {
    emit("submit", { imageBase64: previewUrl.value });
  }
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
.form-image-modal {
  background: #fff;
  padding: 32px 32px 24px 32px;
  border-radius: 18px;
  max-width: 400px; /* tăng width */
  width: 100%;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.18);
  position: relative;
}
h2 {
  margin-bottom: 18px;
  font-size: 26px;
  color: #e65100;
  font-weight: 600;
}
.form-group {
  margin-bottom: 18px;
  display: flex;
  flex-direction: column;
}
.switch-group {
  flex-direction: row;
  gap: 12px;
  margin-bottom: 18px;
  display: flex;
  justify-content: center;
}
.switch-btn {
  background: #fff8f1;
  border: 1.5px solid #ffe0b2;
  color: #e65100;
  font-weight: 500;
  border-radius: 8px;
  padding: 8px 22px;
  font-size: 16px;
  cursor: pointer;
  transition: background 0.18s, border 0.18s, color 0.18s;
}
.switch-btn.active,
.switch-btn:hover {
  background: #ffe0b2;
  border-color: #fb8c00;
  color: #fb8c00;
}
.radio-group {
  flex-direction: row;
  gap: 24px;
  margin-bottom: 18px;
}
.radio-group label {
  font-weight: 500;
  color: #e65100;
  margin-right: 18px;
  display: flex;
  align-items: center;
  gap: 6px;
}
.form-label {
  font-weight: 500;
  color: #e65100;
  margin-bottom: 6px;
}
.form-input {
  border: 1.5px solid #ffe0b2;
  border-radius: 8px;
  padding: 10px 12px;
  font-size: 16px;
  background: #fff8f1;
  outline: none;
  transition: border 0.2s;
}
.form-input:focus {
  border-color: #fb8c00;
}
.preview-group {
  align-items: flex-start;
}
.preview-img {
  margin-top: 6px;
  max-width: 180px;
  max-height: 180px;
  border-radius: 8px;
  border: 1px solid #eee;
  background: #fafafa;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
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
