<template>
  <nav :class="['main-menu', { fixed: isFixed }]">
    <a href="/">
      <img src="/src/assets/images/logo/nhh.png" alt="Logo" class="menu-logo" style="cursor:pointer;" />
    </a>
    
    <div class="menu-search">
      <input
        type="text"
        v-model="searchText"
        placeholder="Tìm kiếm sản phẩm..."
        class="search-input"
        @input="onSearchInput"
      />
    </div>

    <ul>
      <li><a class="item" href="/">Trang chủ</a></li>
      <li><a class="item" @click.prevent="router.push('/products')">Sản phẩm</a></li>
      <li><a class="item" @click.prevent="router.push('/about')">Giới thiệu</a></li>
      <li><a class="item" @click.prevent="router.push('/contact')">Liên hệ</a></li>
      <li v-if="!isLoggedIn"><a class="item" @click.prevent="router.push('/login')">Đăng nhập</a></li>
      <li v-else><a class="item" @click.prevent="router.push('/cart')">Giỏ Hàng</a></li>
      <li v-if="!isLoggedIn"><a class="item" @click.prevent="router.push('/register')">Đăng ký</a></li>
      <li v-else><a class="item" @click.prevent="router.push('/account')">Tài khoản</a></li>
    </ul>
  </nav>

  <!-- Component con hiển thị kết quả tìm kiếm -->
  <SearchResultBox :keyword="searchText" />
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import { useRouter } from 'vue-router'
import { getUserId } from '@/utils/jwtHelper'
import { usePhoneStore } from '@/stores/api/phoneStore'
import SearchResultBox from '../Homepages/SearchResultBox.vue'

const router = useRouter()
const isFixed = ref(false)
const searchText = ref('')

const isLoggedIn = computed(() => getUserId())
const phoneStore = usePhoneStore()

const handleScroll = () => {
  isFixed.value = window.scrollY > 0
}

const onSearchInput = () => {
  // Dữ liệu lọc được xử lý ở component con
}

onMounted(async () => {
  window.addEventListener('scroll', handleScroll)
  if (phoneStore.activePhoneList.length === 0) {
    await phoneStore.fetchActivePhones()
  }
})

onBeforeUnmount(() => {
  window.removeEventListener('scroll', handleScroll)
})
</script>

<style scoped>
@import url('https://cdn.jsdelivr.net/npm/@mdi/font@7.4.47/css/materialdesignicons.min.css');

.main-menu {
  width: 100%;
  background: linear-gradient(90deg, #ff5722 60%, #ff9800 100%);
  box-shadow: 0 2px 8px 0 #ffccbc;
  display: flex;
  justify-content: flex-start;
  align-items: center;
  padding: 0 24px;
  position: relative;
  z-index: 100;
  transition: box-shadow 0.2s, position 0.2s, top 0.2s;
}

.main-menu.fixed {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  box-shadow: 0 4px 16px 0 #ffccbc;
  animation: slideDown 0.3s;
}

@keyframes slideDown {
  from {
    top: -80px;
    opacity: 0;
  }
  to {
    top: 0;
    opacity: 1;
  }
}

.menu-logo {
  height: 80px;
  width: auto;
  margin-right: 32px;
  margin-left: 20px;
  object-fit: contain;
  display: block;
}

.menu-search {
  display: flex;
  align-items: center;
  margin-right: 40px;
}

.search-input {
  padding: 8px 12px;
  border-radius: 20px;
  border: 1.5px solid #ffccbc;
  font-size: 16px;
  outline: none;
  width: 220px;
  background: #fff8f0;
  color: #e65100;
  transition: border 0.2s;
}

.search-input:focus {
  border-color: #ff9800;
}

.main-menu ul {
  list-style: none;
  display: flex;
  gap: 32px;
  margin: 0;
  padding: 0;
}

.main-menu li {
  display: flex;
  align-items: center;
}
.main-menu li:hover {
  cursor: pointer;
}
.main-menu a.item {
  color: #fff;
  font-weight: 600;
  font-size: 24px;
  text-decoration: none;
  padding: 16px 8px;
  border-radius: 6px;
  transition: background 0.2s, color 0.2s;
}

.main-menu a.item:hover {
  background: #fff3e0;
  color: #ff5722;
  cursor: pointer;
}
</style>
