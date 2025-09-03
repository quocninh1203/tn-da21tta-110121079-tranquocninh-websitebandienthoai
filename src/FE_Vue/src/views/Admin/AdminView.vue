<!-- <template>
  <v-app>
    <v-app-bar app class="top-bar">
      <v-container fluid>
        <v-row align="center">
          <v-col cols="10">
             <v-text class="headline">Quản lý</v-text>
          </v-col>
          <v-col cols="2" class="top-right-bar">
            <v-card rounded="xl" width="40" height="40" class="mr-3">
              <v-img
                :src="avatarDefault"
                height="40"
                alt="Avatar"              
              ></v-img>
            </v-card>
            <v-text>Quốc Ninh</v-text>
            <v-icon>mdi-menu-down</v-icon>

            <v-card offset-y class="option">
              <v-text>Xem thông tin</v-text>
              <v-text>Đăng xuất</v-text>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-app-bar>

    <v-navigation-drawer app permanent width="200" class="sidebar">
      <v-list dense>
        <v-list-item
          @click="loadContent"
          v-for="item in menuItems"
          :key="item.title"
          :to="item.route"
          :class="{ active: $route.path.startsWith(item.route) }"
        >
          <v-list-item-content>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-main class="main">
      <LoadingSpinnerComponent :loading="loadingStore.loading" />

      <v-container fluid v-if="!loadingStore.loading">
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup>
import { onMounted } from 'vue';
// import { useRoute } from 'vue-router';

// import component, image
import LoadingSpinnerComponent from '@/components/Global/LoadingSpinnerComponent.vue';
import avatarDefault from '@/assets/Images/default_avatar.jpg';

// import store
import { useLoadingStore } from '@/stores/local/loading';

const menuItems = [
  { title: "Dashboard", route: "/admin/dashboard" },
  { title: "Danh mục", route: "/admin/category" },
  { title: "Sản phẩm", route: "/admin/product" },
  { title: "Đơn hàng", route: "/admin/order" },
  // { title: "Người dùng", route: "/admin/user" },
  // { title: "Doanh thu", route: "/admin/revenue" },
];

const loadingStore = useLoadingStore();

const loadContent = () => {
  loadingStore.loadingComponent();
}

onMounted(() => {
  loadContent();
});
</script>

<style scoped>

.sidebar, .top-bar {
  background-color: #1a202c !important; 
  color: #6C7293 !important; 
}

.headline {
  font-size: 24px;
  font-weight: bold;
  color: #f00; 
}

.top-right-bar {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.option {
  position: absolute;
  top: 30px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 9;
  background-color: #6C7293;
  display: none;
}

/* item được chọn */
.active {
  position: relative;
  background-color: #2d3748; 
  color: #f00;
}
.active::before {
  content: "";
  position: absolute;
  left: 0; 
  top: 0;
  width: 2px; 
  height: 100%; 
  background-color: #f00;
}

.main {
  color: #6C7293;
  background-color: #000;
  position: relative;
}
</style> -->
