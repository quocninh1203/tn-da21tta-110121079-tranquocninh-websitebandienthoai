<template>
  <div class="modal-product">
    <div class="modal-content short-width">
      <h2>{{ isEdit ? 'Sửa sản phẩm' : 'Thêm sản phẩm' }}</h2>
      <form @submit.prevent="submitForm">
        <div class="form-row">
          <div class="form-group half">
            <label>Tên sản phẩm</label>
            <input v-model="form.name" required />
          </div>
          <div class="form-group half">
            <label>Hãng</label>
            <select v-model="form.brandId" required>
              <option value="">Chọn hãng</option>
              <option v-for="brand in brands" :key="brand.id" :value="brand.id">
                {{ brand.name }}
              </option>
            </select>
          </div>
        </div>
        <div class="form-row">
          <div class="form-group half">
            <label>Ảnh</label>
            <div class="switch-group">
              <button
                type="button"
                :class="['switch-btn', imageMode === 'file' ? 'active' : '']"
                @click="setImageMode('file')"
              >
                Ảnh từ máy
              </button>
              <button
                type="button"
                :class="['switch-btn', imageMode === 'remote' ? 'active' : '']"
                @click="setImageMode('remote')"
              >
                Ảnh từ internet
              </button>
            </div>
            <input
              v-if="imageMode === 'file'"
              type="file"
              accept="image/*"
              @change="onImageChange"
            />
            <input
              v-if="imageMode === 'remote'"
              type="text"
              v-model="remoteImageUrl"
              placeholder="https://example.com/image.jpg"
              class="form-input"
            />
            <div v-if="imagePreview" class="img-preview">
              <img :src="imagePreview" alt="preview" />
            </div>
          </div>
          <div class="form-group half">
            <label>Màn hình</label>
            <input v-model="form.screen" required />
          </div>
        </div>
        <div class="form-row">
          <div class="form-group half">
            <label>Chipset</label>
            <input v-model="form.chip" required />
          </div>
          <div class="form-group">
            <label>Pin</label>
            <input v-model="form.battery" required />
          </div>
        </div>
        <div class="form-group">
          <label>Mô tả</label>
          <textarea v-model="form.description" rows="4" required></textarea>
        </div>
        <div class="modal-actions">
          <button type="submit" class="btn-save">{{ isEdit ? 'Lưu' : 'Thêm' }}</button>
          <button type="button" class="btn-cancel" @click="$emit('close')">Huỷ</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch, computed } from 'vue'
import { useBrandStore } from '@/stores/api/brandStore'
import { convertToBase64 } from '@/utils/formatters'

const props = defineProps({
  show: Boolean,
  isEdit: Boolean,
  product: Object
})
const emit = defineEmits(['close', 'submit'])

const brandStore = useBrandStore()
const brands = computed(() => brandStore.brandList || [])

const form = ref({
  name: '',
  brandId: '',
  imageBase64: '',
  screen: '',
  chip: '',
  battery: '',
  description: ''
})
const imageFile = ref(null)
const imagePreview = ref('')
const imageMode = ref('file')
const remoteImageUrl = ref('')

const resetForm = () => {
  form.value = {
    name: '',
    brandId: '',
    imageBase64: '',
    screen: '',
    chip: '',
    battery: '',
    description: ''
  }
  imageFile.value = null
  imagePreview.value = ''
  imageMode.value = 'file'
  remoteImageUrl.value = ''
}

watch(() => props.product, (val) => {
  if (props.isEdit && val) {
    form.value = {
      name: val.name || '',
      brandId: val.brandId || '',
      imageBase64: val.imageUrl || '',
      screen: val.screen || '',
      chip: val.chip || '',
      battery: val.battery || '',
      description: val.description || ''
    }
    imagePreview.value = val.imageUrl || ''
    imageFile.value = null
    imageMode.value = 'file'
    remoteImageUrl.value = ''
  } else {
    resetForm()
  }
}, { immediate: true })

function setImageMode(mode) {
  imageMode.value = mode
  imageFile.value = null
  remoteImageUrl.value = ''
  imagePreview.value = ''
}

const onImageChange = async (e) => {
  const file = e.target.files[0]
  if (file) {
    imageFile.value = file
    imagePreview.value = await convertToBase64(file)
  }
}

watch(remoteImageUrl, async (val) => {
  if (imageMode.value === 'remote' && val) {
    // convert remote url to base64
    try {
      const response = await fetch(val)
      const blob = await response.blob()
      imagePreview.value = await convertToBase64(blob)
      imageFile.value = null
    } catch {
      imagePreview.value = ''
    }
  } else if (imageMode.value === 'remote') {
    imagePreview.value = ''
  }
})

const submitForm = () => {
  let data = { ...form.value }
  if ((imageMode.value === 'file' && imageFile.value) || (imageMode.value === 'remote' && remoteImageUrl.value)) {
    data.imageBase64 = imagePreview.value // always base64
  } else if (props.isEdit) {
    data.imageBase64 = null
  }
  emit('submit', data)
}
</script>

<style scoped>
.modal-product {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0,0,0,0.18);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}
.modal-content {
  background: #fff;
  border-radius: 14px;
  padding: 32px 28px;
  min-width: 340px;
  box-shadow: 0 8px 32px #e0e0e0;
  width:650px;
}
.form-row {
  display: flex;
  gap: 12px;
}
.form-group {
  margin-bottom: 18px;
  display: flex;
  flex-direction: column;
  gap: 6px;
}
.form-group.half {
  flex: 1 1 0;
}
.form-group label {
  font-weight: 500;
  color: #e65100;
}
.form-group input,
.form-group select,
.form-group textarea {
  padding: 8px 12px;
  border-radius: 6px;
  border: 1.5px solid #ffccbc;
  font-size: 1rem;
  background: #fff8f0;
  color: #000;
}
.img-preview img {
  width: 80px;
  height: 80px;
  object-fit: cover;
  border-radius: 6px;
  margin-top: 6px;
}
.modal-actions {
  display: flex;
  gap: 12px;
  justify-content: flex-end;
  margin-top: 10px;
}
.btn-save {
  background: #43a047;
  color: #fff;
  border: none;
  border-radius: 8px;
  padding: 8px 22px;
  font-size: 1rem;
  font-weight: bold;
  cursor: pointer;
}
.btn-cancel {
  background: #fff;
  color: #e65100;
  border: 1.5px solid #e65100;
  border-radius: 8px;
  padding: 8px 22px;
  font-size: 1rem;
  font-weight: bold;
  cursor: pointer;
}
.btn-cancel:hover {
  background: #fff3e0;
}
.switch-group {
  flex-direction: row;
  gap: 12px;
  margin-bottom: 8px;
  display: flex;
  justify-content: flex-start;
}
.switch-btn {
  background: #fff8f1;
  border: 1.5px solid #ffe0b2;
  color: #e65100;
  font-weight: 500;
  border-radius: 8px;
  padding: 6px 18px;
  font-size: 15px;
  cursor: pointer;
  transition: background 0.18s, border 0.18s, color 0.18s;
  margin-right: 8px;
}
.switch-btn.active,
.switch-btn:hover {
  background: #ffe0b2;
  border-color: #fb8c00;
  color: #fb8c00;
}
</style>
