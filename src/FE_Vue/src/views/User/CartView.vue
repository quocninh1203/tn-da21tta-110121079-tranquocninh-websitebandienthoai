<template>
  <TheMenu />

  <v-container class="main-card">
    <v-row>
      <!-- Header -->
      <v-col cols="12">
        <v-row class="py-4">
          <v-col cols="6">
            <h2>Danh sách sản phẩm</h2>
          </v-col>
        </v-row>
      </v-col>

      <!-- Header Titles -->
      <v-col cols="12" v-if="carts.length > 0">
        <v-row>
          <v-col cols="4"></v-col>
          <v-col cols="5">
            <v-row>
              <v-col cols="4">Đơn giá</v-col>
              <v-col cols="4" class="text-center">Số lượng</v-col>
              <v-col cols="4">Tổng</v-col>
            </v-row>
          </v-col>
          <v-col cols="3"></v-col>
        </v-row>
      </v-col>

      <!-- Cart Items -->
      <v-col cols="12" v-if="carts.length > 0">
        <v-row
          v-for="(item, index) in carts"
          :key="index"
          :class="['cart-item mb-4', selectedItems[index] ? 'selected-item' : '']"
        >
          <v-col cols="4" class="center">
            <v-row>
              <v-col cols="12">
                <v-row>
                  <v-col cols="1">
                    <v-checkbox v-model="selectedItems[index]"></v-checkbox>
                  </v-col>
                  <v-col cols="3">
                    <v-img :src="item.imageUrl" max-width="100" />
                  </v-col>
                  <v-col cols="8">
                    <p>{{ item.phoneName }}</p>
                    <p>{{ item.ram }}/{{ item.storage }}</p>
                    <div class="color-square" :style="{ backgroundColor: mapColor(item.color) }" :title="item.color"></div>
                  </v-col>
                </v-row>
              </v-col>
            </v-row>
          </v-col>

          <v-col cols="5" class="center">
            <v-row>
              <v-col cols="4">{{ formatVND(item.price) }}</v-col>
              <v-col cols="4" class="d-flex align-center justify-center">
                <v-btn :disabled="item.quantity === 1" icon outlined size="30" @click="decreaseQuantity(item)">
                  <v-icon>mdi-minus</v-icon>
                </v-btn>
                <div class="quantity-display mx-2">{{ item.quantity }}</div>
                <v-btn icon outlined size="30" @click="increaseQuantity(item)">
                  <v-icon>mdi-plus</v-icon>
                </v-btn>
              </v-col>
              <v-col cols="4">{{ formatVND(item.price * item.quantity) }}</v-col>
            </v-row>
          </v-col>

          <v-col cols="3" class="text-right">
            <v-row>
              <v-col cols="12">
                <v-btn color="#f00" @click="remove(item.cartId)">xóa</v-btn>
              </v-col>
            </v-row>
          </v-col>
        </v-row>
      </v-col>

      <!-- Empty Cart -->
      <v-col cols="12" v-else>
        <v-alert type="info" dismissible>
          Giỏ hàng của bạn hiện tại đang trống.
        </v-alert>
      </v-col>

      <v-col cols="12" style="height: 120px;"></v-col>
    </v-row>
  </v-container>

  <!-- Sticky Bottom Bar -->
  <div v-if="carts.length > 0" class="cart-footer-bar">
    <v-container class="main-card no-shadow no-border py-2">
      <v-row align="center" justify="space-between">
        <v-col cols="6" class="d-flex align-center">
          <v-checkbox
            v-model="selectAll"
            :label="`Chọn tất cả(${ carts.length })`"
            hide-details
            class="ma-0 pa-0"
            @change="toggleSelectAll"
          ></v-checkbox>
        </v-col>
        <v-col cols="2" class="text-right">
          <span>Sản phẩm({{ selectedItemsCount }}):</span>
        </v-col>
        <v-col cols="2" class="text-left">
          <span>{{ formatVND(totalPrice) }}</span>
        </v-col>
        <v-col cols="2" class="text-right">
          <v-btn :disabled="!hasSelected" color="#ff5722" @click="checkout">Mua hàng</v-btn>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, watch } from 'vue';
import { useRouter } from 'vue-router';
import { formatVND } from '@/utils/formatters';
import { getUserId } from '@/utils/jwtHelper';
import { useCartStore } from '@/stores/api/cartStore';
import { useOrderStore } from '@/stores/api/orderStore';
import { useNotificationStore } from '@/stores/local/notification';

import TheMenu from '@/components/Global/TheMenu.vue';

const router = useRouter();
const cartStore = useCartStore();
const orderStore = useOrderStore(); 
const notificationStore = useNotificationStore();

const carts = computed(() => cartStore.carts);
const userId = ref(0);
const selectedItems = ref([]);
const selectAll = ref(false);

watch(carts, (newCarts) => {
  selectedItems.value = newCarts.map(() => false);
}, { immediate: true });

const hasSelected = computed(() => selectedItems.value.some(v => v));
const selectedItemsCount = computed(() => selectedItems.value.filter(Boolean).length);

const toggleSelectAll = () => {
  selectedItems.value = carts.value.map(() => selectAll.value);
};

const increaseQuantity = async (item) => {
  const newQuantity = item.quantity + 1;
  const res = await cartStore.updateCartQuantity(item.cartId, newQuantity, userId.value);
  
  if (res.data.success === false) { 
    notificationStore.addNotification(res.data.message, 'error');
  }
};

const decreaseQuantity = async (item) => {
  if (item.quantity > 1) {
    const newQuantity = item.quantity - 1;
    await cartStore.updateCartQuantity(item.cartId, newQuantity, userId.value);
  }
};

const totalPrice = computed(() => {
  return carts.value.reduce((sum, item, index) => {
    return selectedItems.value[index] ? sum + item.price * item.quantity : sum;
  }, 0);
});

const checkout = async () => {
  const selectedProducts = carts.value.filter((_, idx) => selectedItems.value[idx]);
  await orderStore.saveProductsToSession(selectedProducts);
  router.push("/order")
};

const remove = (id) => {
  cartStore.deleteCart(id, userId.value);
};

const mapColor = (color) => {
  const map = {
    Red: '#f44336',
    Blue: '#2196f3',
    Black: '#212121',
    White: '#fff',
    Pink: '#f48fb1',
    Green: '#4caf50',
    Yellow: '#ffeb3b',
    Gray: '#9e9e9e',
    Purple: '#9c27b0'
  };
  return map[color] || '#ccc';
};

onMounted(() => {
  userId.value = getUserId();
  cartStore.fetchCartByUserId(userId.value);
});
</script>

<style scoped>
.main-card {
  border-radius: 20px;
  border: 1px solid #ddd;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
  padding: 20px 40px;
  margin-top: 24px;
  margin-bottom: 24px;
}
.cart-item {
  border: 1px solid #ccc;
  border-radius: 6px;
  padding: 10px;
  transition: border 0.2s, box-shadow 0.2s;
}
.selected-item {
  border: 1px solid #f00 !important;
  box-shadow: 0 0 8px rgba(255, 0, 0, 0.4);
}
.center {
  display: flex;
  align-items: center;
}
.color-square {
  width: 20px;
  height: 20px;
  border: 1px solid #999;
  border-radius: 4px;
  display: inline-block;
  margin-top: 5px;
}
.cart-footer-bar {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background: white;
  box-shadow: 0 -2px 10px rgba(0, 0, 0, 0.1);
  z-index: 100;
  border-top: 1px solid #eee;
}
.no-border {
  border: none !important;
}
.no-shadow {
  box-shadow: none !important;
}
</style>
