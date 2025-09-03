<template>
  <v-dialog v-model="isOpen" max-width="500" persistent>
    <v-card>
      <!-- Tiêu đề -->
      <v-card-title class="text-h6 font-weight-bold px-6 py-4 d-flex justify-space-between">
        Đánh giá & nhận xét
        <v-btn icon @click="close">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-title>

      <v-divider></v-divider>

      <v-card-text>
        <!-- Đánh giá chung -->
        <div class="section-title">Đánh giá chung</div>
        <div class="rating-row">
          <div
            v-for="(item, index) in overallOptions"
            :key="index"
            class="star-label"
            @click="overallRating = index + 1"
          >
            <span :class="['star', overallRating > index ? 'active' : '']">★</span>
            <div class="caption">{{ item }}</div>
          </div>
        </div>

        <!-- Nhận xét -->
        <textarea
          v-model="comment"
          placeholder="Xin mời chia sẻ một số cảm nhận về sản phẩm (nhập tối thiểu 15 kí tự)"
          class="comment-box"
        ></textarea>

        <!-- Hình ảnh -->
        <div class="upload-section d-flex align-center mt-4">
          <!-- Preview ảnh -->
          <div v-if="selectedImage" class="preview-container me-4">
            <img :src="previewUrl" alt="preview" />
            <v-icon class="remove-icon" small @click="removeImage">mdi-close-circle</v-icon>
          </div>

          <!-- Nút chọn ảnh -->
          <label for="image-upload" class="upload-label">
            <v-icon class="me-2">mdi-camera-plus</v-icon>
          </label>
          <input
            id="image-upload"
            type="file"
            accept="image/*"
            @change="handleImageUpload"
            class="hidden-input"
          />
        </div>
      </v-card-text>

      <v-card-actions class="px-6 pb-4">
        <v-btn
          class="submit-review-btn"
          block
          size="large"
          :disabled="!isValid"
          @click="submitReview"
        >
          GỬI ĐÁNH GIÁ
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup>
import { ref, watch, computed } from 'vue'
import { convertToBase64 } from '@/utils/formatters' 

const props = defineProps({ modelValue: Boolean })
const emit = defineEmits(['update:modelValue', 'submitted'])

const isOpen = ref(props.modelValue)
watch(() => props.modelValue, (val) => (isOpen.value = val))
watch(isOpen, (val) => emit('update:modelValue', val))

const overallRating = ref(0)
const comment = ref('')
const selectedImage = ref(null)
const previewUrl = ref('')

const overallOptions = ['Rất Tệ', 'Tệ', 'Bình thường', 'Tốt', 'Tuyệt vời']

const isValid = computed(() => {
  return overallRating.value > 0 && comment.value.trim().length >= 15
})

function close() {
  isOpen.value = false
}

function handleImageUpload(event) {
  const file = event.target.files[0]
  if (!file) return
  selectedImage.value = file
  previewUrl.value = URL.createObjectURL(file)
}

function removeImage() {
  selectedImage.value = null
  previewUrl.value = ''
}

async function submitReview() {
  if (!isValid.value) return

  let imageBase64 = null
  if (selectedImage.value) {
    imageBase64 = await convertToBase64(selectedImage.value) // ✅ Gọi từ utils
  }

  emit('submitted', {
    overallRating: overallRating.value,
    comment: comment.value,
    image: imageBase64 // null nếu không có ảnh
  })

  close()
}
</script>


<style scoped>
.section-title {
  font-weight: 600;
  font-size: 16px;
  margin-bottom: 12px;
}

.rating-row {
  display: flex;
  justify-content: space-around;
  margin-bottom: 24px;
}

.star-label {
  display: flex;
  flex-direction: column;
  align-items: center;
  cursor: pointer;
}

.star {
  font-size: 32px;
  color: #ccc;
  transition: color 0.2s;
}

.star.active {
  color: #fdd835;
}

.caption {
  font-size: 13px;
  margin-top: 4px;
  color: #444;
}

.comment-box {
  margin-top: 20px;
  width: 100%;
  min-height: 100px;
  padding: 12px;
  font-size: 14px;
  border: 1px solid #ccc;
  border-radius: 10px;
  resize: vertical;
}

.upload-section {
  display: flex;
  align-items: center;
}

.upload-label {
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  color: #1976d2;
  width: 100px;
  height: 80px;
  border-radius: 10px;
  border: 1px dashed #ccc;
  background-color: #e9f4ff;
}

.hidden-input {
  display: none;
}

.preview-container {
  position: relative;
}

.preview-container img {
  max-width: 80px;
  max-height: 80px;
  border-radius: 8px;
  object-fit: cover;
}

.remove-icon {
  position: absolute;
  top: -8px;
  right: -8px;
  background: #fff;
  border-radius: 50%;
  color: red;
  cursor: pointer;
  font-size: 18px;
  box-shadow: 0 0 3px rgba(0, 0, 0, 0.2);
}

.submit-review-btn {
  background-color: #d70018;
  color: #fff;
  font-weight: bolder;
  font-size: 16px;
  text-transform: none;
  border-radius: 8px;
}
</style>
