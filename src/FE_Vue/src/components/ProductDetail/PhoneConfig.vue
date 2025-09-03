<template>
  <v-card class="mt-10 pa-6 radius">
    <v-row>
      <!-- Mô tả sản phẩm -->
      <v-col cols="12" md="6">
        <div class="product-desc">
          <h3 class="desc-title">Mô tả sản phẩm</h3>
          <div class="desc-content">{{ phone.description }}</div>
        </div>
      </v-col>

      <!-- Thông số kỹ thuật -->
      <v-col cols="12" md="6">
        <v-simple-table class="spec-table">
          <tbody>
            <tr>
              <td><strong>Thương hiệu:</strong></td>
              <td>{{ phone.brandName }}</td>
            </tr>               
            <tr>
              <td><strong>Màn hình:</strong></td>
              <td>{{ phone.screen }}</td>
            </tr>                  
            <tr>
              <td><strong>Chip:</strong></td>
              <td>{{ phone.chip }}</td>
            </tr>
            <tr>
              <td><strong>Pin:</strong></td>
              <td>{{ phone.battery }}</td>
            </tr>
            <!-- Có thể thêm các thông số khác nếu cần -->
          </tbody>
        </v-simple-table>
      </v-col>
    </v-row>
  </v-card>
</template>

<script setup>
const props = defineProps({
  phone: Object
});
</script>

<style scoped>
.radius {
  border-radius: 20px;
  background: #eee;
}
.spec-table td {
  padding: 8px;
  vertical-align: top;
}
.product-desc {
  background: #fafafa;
  border-radius: 8px;
  padding: 16px;
  min-height: 160px;
  box-shadow: 0 2px 8px #eee;
}
.desc-title {
  font-size: 1.2rem;
  font-weight: bold;
  margin-bottom: 8px;
  color: #ff5722;
}
.desc-content {
  font-size: 1rem;
  color: #333;
  line-height: 1.6;
}
</style>
