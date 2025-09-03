<template>
    <v-sheet
      class="mx-auto rounded-lg"
      elevation="8"
      max-width="1275"
    >
      <!-- Thêm tên Group item ở đây -->
      <v-container>
        <v-row class="py-4" style="background-color: white; color: #000">
          <v-col cols="4" class="text-center">
            <span class="text-h4 font-weight-bold">
              <slot name="nameGroup"></slot>
            </span>
          </v-col>
          <v-col cols="8">
            <v-btn
              v-for="company in companies"
              :key="company.id"
              class="ml-2"
              :color="colors"
              outlined
              @click="filterByCompany(company.id)"
            >
              {{ company.name }}
            </v-btn>
          </v-col>
        </v-row>
      </v-container>
      
      <v-slide-group
        class="pa-4"
        selected-class="bg-success"
        show-arrows
      >
        <button
          v-for="item in filteredItems"
          :key="item.id"
          @click="moveToDetail(item.id)"
        >
        <v-slide-group-item>
          <v-card
            class="ma-4 pa-4"
            color="#fff"
            height="auto"
            width="250"
          >
            <v-img
                :src="`https://localhost:5000${item.imageUrl}`"
                class="align-end"
                height="200px"
            >  
            </v-img>

            <div class="pa-1">
              <v-card-title class="text-h6">{{ item.name }}</v-card-title>
              <v-card-subtitle class="text-subtitle-2">{{ item.Description }}</v-card-subtitle>
              <v-card-text>
                <div>Hỗ trợ mạng: {{ item.networkSupport }}</div>
                <div>Giá: {{ formatNumber(getPriceForProduct(item.id)[0].price) }}</div>
              </v-card-text>
            </div>

          </v-card>
        </v-slide-group-item>
        </button>
      </v-slide-group>
    </v-sheet>
</template>

<script>
import router from '@/router';

export default {
  props: {
    items: {
      type: Array,       // Chỉ định kiểu là mảng
      required: false,     // Bắt buộc phải truyền
      default: () => []
    },
    options:{
      type: Array,
      required: false,
      default: () => []
    },
    colors:{
      type: String,
      required: false,
      default: () => "primary"
    }
  },
  data() {
    return {
      selectedCompanyId: 0,
      companies: [
        { id: 0, name: "Tất cả" },
        { id: 1, name: "Apple" },
        { id: 2, name: "Oppo" },
        { id: 3, name: "SamSung" },
        { id: 4, name: "Vivo" },
      ],
    };
  },
  computed: {
    filteredItems() {
      // Lọc danh sách item theo companyId
      if (this.selectedCompanyId === 0) {
        return this.items;
      }
      return this.items.filter(item => item.companyId === this.selectedCompanyId);
    },
  },
  methods: {
    formatNumber(number){
      if (number == null) return null;
      return number.toLocaleString('en-US').replace(/,/g, '.') + "đ";
    },
    // Lấy Id của Company đang được chọn
    filterByCompany(companyId) {
      this.selectedCompanyId = companyId;
      console.log("Company đang được chọn:" + companyId);
    },
    getPriceForProduct(productId){
      const productOptions = this.options
        .filter(option => option.productId === productId);
      if (productOptions.length > 0) {
        return productOptions; // Trả về danh sách các productOption
      } else {
        return 'Đang cập nhật'; // Trả về thông báo nếu không có productOption
      }
    },
    async moveToDetail(productId){
      router.push(`/product/${productId}`);
    }

  }
};
</script>

<style scoped>
span{
  border-left: 1px solid #ccc;
  margin-left: -100px;
  padding-left: 20px;
}
</style>