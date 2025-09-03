<template>
  <TheMenu />
  <v-container class="mt-5" v-if="phone">
    <PhoneInfor />
    <v-row>
      <v-col cols="12" md="12">
        <PhoneConfig :phone="phone" />
      </v-col>
    </v-row>
    
    <v-row>
      <v-col cols="12" md="12">
        <PhoneEvaluate 
          :productName="phone.phoneName"
          :rating="phone.rating"
          :total-reviews="totalReviews"
          :review-counts="reviewCounts"
          :reviews="phone.reviews"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { computed } from 'vue'
import { usePhoneStore } from '@/stores/api/phoneStore'
import TheMenu from '@/components/Global/TheMenu.vue'
import PhoneConfig from '@/components/ProductDetail/PhoneConfig.vue'
import PhoneInfor from '@/components/ProductDetail/PhoneInfor.vue'
import PhoneEvaluate from '@/components/ProductDetail/PhoneEvaluate.vue'

const phoneStore = usePhoneStore()
const phone = computed(() => phoneStore.getSelectedPhone)

const getReviewCounts = (reviews) => {
  const counts = { 5: 0, 4: 0, 3: 0, 2: 0, 1: 0 };
  reviews.forEach(r => {
    const rate = parseInt(r.rating);
    if (rate >= 1 && rate <= 5) counts[rate]++;
  });
  return counts;
};

const reviewCounts = computed(() => getReviewCounts(phone.value.reviews || []));
const totalReviews = computed(() => phone.value.reviews?.length || 0);
</script>

<style scoped>
.main-card {
  padding: 20px;
  border-radius: 10px;
  background-color: #ffffff;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.image-carousel-wrapper {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.main-image {
  max-width: 100%;
  object-fit: contain;
  border-radius: 10px;
}

.nav-arrow {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  z-index: 2;
}

.nav-arrow.left {
  left: -20px;
}

.nav-arrow.right {
  right: -20px;
}

.sub-card {
  height: 100%;
}

.custom-title {
  font-size: 32px;
  font-weight: bold;
}

.option-container {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-bottom: 10px;
}

.option-item {
  position: relative;
  border: 1px solid #ccc;
  padding: 10px 15px;
  border-radius: 10px;
  background-color: white;
  cursor: pointer;
}

.color-container {
  display: flex;
  gap: 10px;
  margin-bottom: 10px;
  align-items: center;
}

.color-circle {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: 2px solid #999;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  position: relative;
}

.color-circle.IsCheck {
  border: 2px solid red;
}

.check-icon-white {
  color: white;
  font-size: 18px;
}

.check-container {
  position: absolute;
  top: 0;
  right: 0;
  display: none;
  align-items: center;
  justify-content: center;
  color: #fff;
  background-color: red;
  border-top-right-radius: 10px;
  border-bottom-left-radius: 15px;
}

.option-item.IsCheck {
  border: red solid 2px;
}

.check-icon {
  font-size: 15px;
  padding: 10px;
}

.IsCheck {
  display: flex !important;
  border: red solid 1px !important;
}
</style>