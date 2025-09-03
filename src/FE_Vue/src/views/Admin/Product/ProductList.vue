<template>
  <div class="admin-content">
    <h2 class="title">Danh sách sản phẩm</h2>

    <div class="filter-row">
      <div class="filter-status">
        <label for="statusFilter">Trạng thái:</label>
        <select id="statusFilter" v-model="filterStatus" class="filter-input">
          <option value="">Tất cả</option>
          <option value="active">Hiển thị</option>
          <option value="inactive">Ẩn</option>
        </select>
      </div>
      <button class="btn add-btn" @click="addProduct">+ Thêm sản phẩm</button>
    </div>

    <table class="admin-table">
      <thead>
        <tr>
          <th>#</th>
          <th>Ảnh</th>
          <th>Tên sản phẩm</th>
          <th>Hãng</th>
          <th>Giá</th>
          <th>Trạng thái</th>
          <th>Thao tác</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(product, index) in filteredProducts"
          :key="product.phoneId"
        >
          <td>{{ index + 1 }}</td>
          <td><img :src="product.imageUrl" alt="Ảnh" class="thumbnail" /></td>
          <td>{{ product.name }}</td>
          <td>{{ getBrandName(product.brandId) }}</td>
          <td>{{ formatPrice(product.priceReview) }}</td>
          <td>
            <span :class="['status', product.isActive ? 'active' : 'inactive']">
              {{ product.isActive ? 'Hiển thị' : 'Ẩn' }}
            </span>
          </td>
          <td class="actions">
            <button class="btn view" @click="viewProduct(product.phoneId, product.slug)" title="Xem chi tiết">
              <v-icon>mdi-eye</v-icon>
            </button>
            <button class="btn edit" @click="editProduct(product.phoneId)" title="Sửa">
              <v-icon>mdi-pencil</v-icon>
            </button>
            <button class="btn delete" @click="deleteProduct(product.phoneId)" title="Xoá">
              <v-icon>mdi-close</v-icon>
            </button>
            <button class="btn status-toggle" @click="toggleStatus(product)" :title="product.isActive ? 'Ẩn sản phẩm' : 'Hiển thị sản phẩm'">
              <v-icon>{{ product.isActive ? 'mdi-eye-off' : 'mdi-eye-outline' }}</v-icon>
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <ModalProduct
      v-if="showModal"
      :show="showModal"
      :isEdit="isEdit"
      :product="selectedProduct"
      @close="handleModalClose"
      @submit="handleModalSubmit"
    />
  </div>
</template>

<script setup>
import { onMounted, computed, ref } from 'vue';
import { useRouter } from 'vue-router';
import { usePhoneStore } from '@/stores/api/phoneStore';
import { useBrandStore } from '@/stores/api/brandStore';
import { useNotificationStore } from '@/stores/local/notification';
import ModalProduct from '@/components/Admins/Products/FormProduct.vue'

const phoneStore = usePhoneStore();
const brandStore = useBrandStore();
const router = useRouter();
const notificationStore = useNotificationStore();

const showModal = ref(false)
const isEdit = ref(false)
const selectedProduct = ref(null)

onMounted(async () => {
  await Promise.all([
    phoneStore.fetchAllPhones(),
    brandStore.fetchAllBrands()
  ]);
});

const filterStatus = ref('');

const reversedProducts = computed(() =>
  [...phoneStore.phoneList].reverse()
);

const filteredProducts = computed(() => {
  if (!filterStatus.value) return reversedProducts.value;
  if (filterStatus.value === 'active') {
    return reversedProducts.value.filter(p => p.isActive);
  }
  if (filterStatus.value === 'inactive') {
    return reversedProducts.value.filter(p => !p.isActive);
  }
  return reversedProducts.value;
});

const getBrandName = (brandId) => {
  const brand = brandStore.brandList.find(b => b.id === brandId);
  return brand ? brand.name : 'Không rõ';
};

const formatPrice = (value) => {
  return value?.toLocaleString('vi-VN') + '₫';
};

const viewProduct = async (id, slug) => {
  await phoneStore.fetchPhoneById(id);  
  router.push({name: 'adminProductDetail', params: { slug: slug } });
};

const editProduct = (id) => {
  const product = phoneStore.phoneList.find(p => p.phoneId === id)
  selectedProduct.value = product
  isEdit.value = true
  showModal.value = true
};

const addProduct = () => {
  selectedProduct.value = null
  isEdit.value = false
  showModal.value = true
};

const handleModalSubmit = async (data) => {
  if (isEdit.value && selectedProduct.value) {
    // Gọi API cập nhật sản phẩm
    const res = await phoneStore.updatePhone(selectedProduct.value.phoneId, data)
    if (res && res.success) {
      notificationStore.addNotification('Cập nhật sản phẩm thành công!', 'success')
    } else {
      notificationStore.addNotification(res?.message || 'Cập nhật sản phẩm thất bại!', 'error')
    }
  } else {  
    // Gọi API thêm sản phẩm
    const res = await phoneStore.createPhone(data)
    if (res && res.success) {
      notificationStore.addNotification('Thêm sản phẩm thành công!', 'success')
    } else {
      notificationStore.addNotification(res?.message || 'Thêm sản phẩm thất bại!', 'error')
    }
  }
  showModal.value = false
  await phoneStore.fetchAllPhones()
};

const handleModalClose = () => {
  showModal.value = false
};

const toggleStatus = async (product) => {
  // Gọi API cập nhật trạng thái sản phẩm
  const res = await phoneStore.toggleActive(product.phoneId)
  if (res && res.success) {
    notificationStore.addNotification('Cập nhật trạng thái thành công!', 'success')
  } else {
    notificationStore.addNotification(res?.message || 'Cập nhật trạng thái thất bại!', 'error')
  }
}
</script>

<style scoped>
.admin-content {
  padding: 20px;
  background: #fdfdfd;
}

.title {
  position: relative;
  padding-left: 16px; /* để tạo chỗ cho đường trước */
  font-size: 26px;
  color: #e65100;
  margin-bottom: 20px;
  display: flex;
  align-items: center;
}

.title::before {
  content: "";
  position: absolute;
  left: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 5px;
  height: 40px;
  background-color: #e65100;
}

.filter-row {
  display: flex;
  align-items: center;
  gap: 18px;
  margin-bottom: 18px;
}

.filter-status {
  display: flex;
  align-items: center;
  gap: 8px;
}

.add-btn {
  margin-left: auto;
  background: #388e3c;
  color: #fff;
  font-weight: bold;
  border: none;
  border-radius: 5px;
  padding: 8px 18px;
  font-size: 15px;
  cursor: pointer;
  transition: background 0.2s;
}

.add-btn:hover {
  background: #2e7d32;
}

.filter-input {
  padding: 6px 10px;
  border-radius: 5px;
  border: 1px solid #eee;
  font-size: 15px;
  margin-left: 6px;
}

.admin-table {
  width: 100%;
  border-collapse: collapse;
  background: #fff;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.06);
  border-radius: 6px;
  overflow: hidden;
}

.admin-table th,
.admin-table td {
  padding: 14px 16px;
  text-align: left;
  border-bottom: 1px solid #f2f2f2;
  vertical-align: middle;
}

.admin-table th {
  background-color: #ff9800;
  color: white;
  font-weight: bold;
}

.thumbnail {
  width: 60px;
  height: 60px;
  object-fit: cover;
  border-radius: 4px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.status {
  padding: 4px 10px;
  border-radius: 5px;
  font-weight: bold;
  font-size: 14px;
  display: inline-block;
}

.status.active {
  background-color: #c8e6c9;
  color: #2e7d32;
}

.status.inactive {
  background-color: #ffcdd2;
  color: #c62828;
}

.actions {
  display: flex;
  gap: 6px;
  flex-wrap: wrap;
}

.btn {
  border: none;
  border-radius: 4px;
  color: white;
  font-weight: bold;
  cursor: pointer;
  font-size: 14px;
  padding: 6px 10px;
}

.btn.view {
  background-color: #039be5;
}

.btn.variants {
  background-color: #00897b;
}

.btn.add-variant {
  background-color: #388e3c;
}

.btn.edit {
  background-color: #ff9800;
}

.btn.delete {
  background-color: #d32f2f;
}

.btn.status-toggle {
  background-color: #bdbdbd;
  color: #333;
}
.btn.status-toggle:hover {
  background-color: #757575;
  color: #fff;
}
</style>
