<template>
  <div class="category-list-page">
    <h1 class="page-title">Quản lý danh mục (Hãng)</h1>
    <div class="toolbar">
      <input v-model="search" class="search-input" placeholder="Tìm kiếm theo tên hãng..." />
      <button class="btn-refresh" @click="fetchBrands">Làm mới</button>
      <button class="btn-add" @click="showAddModal = true">Thêm hãng</button>
    </div>
    <div class="table-wrapper">
      <table class="category-table">
        <thead>
          <tr>
            <th>STT</th>
            <th>Tên hãng</th>
            <th>Logo</th>
            <th>Thao tác</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(brand, idx) in filteredBrands" :key="brand.id">
            <td>{{ idx + 1 }}</td>
            <td>{{ brand.name }}</td>
            <td>
              <img :src="brand.imageUrl" alt="Logo" class="brand-logo" v-if="brand.imageUrl" />
            </td>
            <td>
              <button class="btn-action" @click="editBrand(brand)">Sửa</button>
              <button class="btn-action btn-danger" @click="deleteBrand(brand)">Xoá</button>
            </td>
          </tr>
          <tr v-if="filteredBrands.length === 0">
            <td colspan="4" class="no-data">Không tìm thấy hãng phù hợp.</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal thêm/sửa hãng -->
    <div v-if="showAddModal || showEditModal" class="modal-overlay">
      <div class="modal-content">
        <h2>{{ showEditModal ? 'Sửa hãng' : 'Thêm hãng' }}</h2>
        <form @submit.prevent="submitBrand">
          <div class="form-group">
            <label>Tên hãng</label>
            <input v-model="brandForm.name" required />
          </div>
          <div class="form-group">
            <label>Logo</label>
            <input type="file" accept="image/*" @change="onImageChange" />
            <div v-if="showEditModal && brandForm.imageUrl" class="old-logo-preview">
              <span>Ảnh hiện tại:</span>
              <img :src="brandForm.imageUrl" alt="Logo cũ" class="brand-logo" />
            </div>
            <div v-if="imagePreview" class="new-logo-preview">
              <span>Ảnh mới:</span>
              <img :src="imagePreview" alt="Logo mới" class="brand-logo" />
            </div>
          </div>
          <div class="modal-actions">
            <button type="submit" class="btn-save">Lưu</button>
            <button type="button" class="btn-cancel" @click="closeModal">Huỷ</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useBrandStore } from '@/stores/api/brandStore'
import { useNotificationStore } from '@/stores/local/notification'
const brandStore = useBrandStore()
const notificationStore = useNotificationStore()

const search = ref('')
const showAddModal = ref(false)
const showEditModal = ref(false)
const brandForm = ref({
  id: null,
  name: '',
  imageUrl: ''
})
const imageFile = ref(null)
const imagePreview = ref('')

const fetchBrands = async () => {
  await brandStore.fetchAllBrands()
  search.value = ''
}

const brands = computed(() => brandStore.brands || [])

const filteredBrands = computed(() => {
  if (!search.value) return brands.value
  const s = search.value.toLowerCase()
  return brands.value.filter(b =>
    b.name.toLowerCase().includes(s)
  )
})

const editBrand = (brand) => {
  showEditModal.value = true
  showAddModal.value = false
  brandForm.value = { ...brand }
  imageFile.value = null
  imagePreview.value = ''
}

const deleteBrand = (brand) => {
  if (confirm(`Bạn có chắc chắn muốn xoá hãng ${brand.name}?`)) {
    // Gọi API xoá nếu có
    // brandStore.deleteBrand(brand.id)
  }
}

const closeModal = () => {
  showAddModal.value = false
  showEditModal.value = false
  brandForm.value = { id: null, name: '', imageUrl: '' }
  imageFile.value = null
  imagePreview.value = ''
}

const onImageChange = (e) => {
  const file = e.target.files[0]
  if (file) {
    imageFile.value = file
    const reader = new FileReader()
    reader.onload = (ev) => {
      imagePreview.value = ev.target.result
    }
    reader.readAsDataURL(file)
  } else {
    imageFile.value = null
    imagePreview.value = ''
  }
}

const submitBrand = async () => {
  let imageUrl = brandForm.value.imageUrl
  let base64Img = imagePreview.value
  if (imageFile.value) {
    imageUrl = base64Img
  } else if (showEditModal.value) {
    imageUrl = null
  }
  if (showEditModal.value) {
    const res = await brandStore.updateBrand(brandForm.value.id, {
      name: brandForm.value.name,
      imageBase64: imageUrl
    })
    if (res && res.success) {
      notificationStore.addNotification('Cập nhật hãng thành công!', 'success')
    } else {
      notificationStore.addNotification(res?.message || 'Cập nhật hãng thất bại!', 'error')
    }
  } else {
    const res = await brandStore.createBrand({
      name: brandForm.value.name,
      imageBase64: imageUrl
    })
    if (res && res.success) {
      notificationStore.addNotification('Thêm hãng thành công!', 'success')
    } else {
      notificationStore.addNotification(res?.message || 'Thêm hãng thất bại!', 'error')
    }
  }
  closeModal()
}

onMounted(() => {
  fetchBrands()
})
</script>

<style scoped>
.category-list-page {
  max-width: 900px;
  margin: 0 auto;
  padding: 32px 18px;
  background: #fff;
  border-radius: 18px;
  box-shadow: 0 4px 24px #e0e0e0;
  font-family: 'Inter', 'Segoe UI', sans-serif;
}
.page-title {
  font-size: 2rem;
  font-weight: 700;
  color: #ff5722;
  margin-bottom: 24px;
  text-align: center;
}
.toolbar {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  margin-bottom: 18px;
  gap: 12px;
}
.search-input {
  padding: 8px 14px;
  border-radius: 8px;
  border: 1.5px solid #ffccbc;
  font-size: 1rem;
  width: 260px;
  background: #fff8f0;
  color: #e65100;
}
.btn-refresh, .btn-add {
  background: linear-gradient(90deg, #ff9800 60%, #ff5722 100%);
  color: #fff;
  font-weight: bold;
  border: none;
  border-radius: 8px;
  padding: 8px 22px;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.2s;
}
.btn-refresh:hover, .btn-add:hover {
  background: linear-gradient(90deg, #e65100 60%, #ff9800 100%);
}
.table-wrapper {
  overflow-x: auto;
}
.category-table {
  width: 100%;
  border-collapse: collapse;
  background: #fff;
}
.category-table th, .category-table td {
  padding: 12px 10px;
  border-bottom: 1px solid #eee;
  text-align: left;
}
.category-table th {
  background: #fff3e0;
  color: #e65100;
  font-weight: 600;
}
.brand-logo {
  width: 60px;
  height: 32px;
  object-fit: contain;
  background: #f5f5f5;
  border-radius: 6px;
  border: 1px solid #eee;
}
.btn-action {
  background: #fff;
  color: #1976d2;
  border: 1.5px solid #1976d2;
  border-radius: 8px;
  padding: 6px 14px;
  font-size: 0.98rem;
  font-weight: 500;
  cursor: pointer;
  margin-right: 6px;
  transition: background 0.2s, color 0.2s, border 0.2s;
}
.btn-action:hover {
  background: #e3f0ff;
  color: #0d47a1;
  border-color: #0d47a1;
}
.btn-danger {
  color: #e53935;
  border-color: #e53935;
}
.btn-danger:hover {
  background: #ffebee;
  color: #b71c1c;
  border-color: #b71c1c;
}
.no-data {
  text-align: center;
  color: #888;
  font-style: italic;
}
.modal-overlay {
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
}
.form-group {
  margin-bottom: 18px;
  display: flex;
  flex-direction: column;
  gap: 6px;
}
.old-logo-preview,
.new-logo-preview {
  margin-top: 8px;
  display: flex;
  align-items: center;
  gap: 10px;
}
.old-logo-preview span,
.new-logo-preview span {
  color: #888;
  font-size: 0.98rem;
}
.brand-logo {
  width: 60px;
  height: 32px;
  object-fit: contain;
  background: #f5f5f5;
  border-radius: 6px;
  border: 1px solid #eee;
}
.form-group label {
  font-weight: 500;
  color: #e65100;
}
.form-group input {
  padding: 8px 12px;
  border-radius: 6px;
  border: 1.5px solid #ffccbc;
  font-size: 1rem;
  background: #fff8f0;
  color: #000;
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
</style>
