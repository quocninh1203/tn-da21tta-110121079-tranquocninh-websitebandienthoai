<template>
  <v-card class="rating-summary radius">
    <h2 class="ml-8 h2">Đánh giá {{ productName }}</h2>

    <!-- Tổng quan -->
    <v-card class="pa-8 ma-8 radius">
      <v-row>
        <!-- Cột trái -->
        <v-col class="left" cols="12" md="4">
          <div class="average">
            <span class="score">{{ rating.toFixed(1) }}</span><span class="out-of">/5</span>
          </div>
          <StarRating :rating="rating" />
          <div class="total-reviews">{{ totalReviews }} lượt đánh giá</div>

          <!-- Nút viết đánh giá -->
          <v-btn
            color="red darken-2"
            class="mt-2 write-review-btn"
            @click="showReviewForm = true"
            :disabled="!reviewInfor"
            height="40"
            width="140"
            rounded
          >
            Viết đánh giá
          </v-btn>
        </v-col>

        <!-- Cột phải -->
        <v-col class="middle" cols="12" md="8">
          <div v-for="n in 5" :key="n" class="bar-row">
            <span class="star-label">{{ 6 - n }}<span class="yellow-star">★</span></span>
            <div class="bar">
              <div class="fill" :style="{ width: getBarPercent(6 - n) + '%' }"></div>
            </div>
            <span class="count">{{ reviewCounts[6 - n] || 0 }} đánh giá</span>
          </div>
        </v-col>
      </v-row>
    </v-card>

    <!-- Bộ lọc + danh sách đánh giá -->
    <v-card class="pa-8 ma-8 radius">
      <!-- Bộ lọc -->
      <v-row class="mb-4">
        <v-col>
          <div class="filter-label">Lọc đánh giá theo</div>
          <div class="filter-buttons">
            <v-btn
              v-for="btn in filters"
              :key="btn.label"
              :variant="selectedFilter === btn.label ? 'outlined' : 'text'"
              color="primary"
              size="small"
              class="me-2"
              rounded
              @click="selectedFilter = btn.label"
            >
              {{ btn.label }}
            </v-btn>
          </div>
        </v-col>
      </v-row>

      <!-- Danh sách đánh giá -->
      <v-row
        v-for="(review, index) in [...filteredReviews].reverse()"
        :key="index"
        class="review-row py-3"
      >
        <v-col cols="12">
          <div class="d-flex align-start mb-2">
            <v-avatar size="40" class="me-3">
              <v-img :src="review.userImageUrl || defaultAvatar" alt="avatar" />
            </v-avatar>
            <div>
              <div class="font-weight-medium">{{ review.fullName }}</div>
              <div class="d-flex align-center">
                <StarRating :rating="review.rating" />
              </div>
              <div class="text-caption text-grey-darken-1">
                Phiên bản: {{ review.color }} - {{ review.ram }} / {{ review.storage }}
              </div>
            </div>
          </div>

          <div class="text-body-2 mb-2">{{ review.text }}</div>

          <div v-if="review.reviewImageUrl" class="mb-2">
            <v-img :src="review.reviewImageUrl" width="150" height="150" class="rounded" cover />
          </div>
          <!-- ⭐ Thời gian đánh giá -->
          <div class="text-caption text-grey-darken-1 d-flex align-center mt-1">
            <v-icon size="16" class="me-1">mdi-clock-time-four-outline</v-icon>
            {{ formatTimeAgo(review.reviewDate) }}
          </div>          
        </v-col>
      </v-row>

    </v-card>
  </v-card>

  <ReviewFormDialog v-model="showReviewForm" @submitted="handleReviewSubmit" />
</template>

<script setup>
import StarRating from '@/components/Global/StarRating.vue'
import ReviewFormDialog from './ReviewFormDialog.vue'
import { useReviewStore } from '@/stores/api/reviewStore'
import { usePhoneStore } from '@/stores/api/phoneStore'
import { ref, computed } from 'vue'
import { formatTimeAgo } from '@/utils/formatters'

const reviewStore = useReviewStore()
const reviewInfor = computed(() => reviewStore.reviewProduct)
const phoneStore = usePhoneStore() 

const props = defineProps({
  productName: String,
  rating: Number,
  totalReviews: Number,
  reviewCounts: Object,
  reviews: Array,
})

const showReviewForm = ref(false)

async function handleReviewSubmit(data) {
  const newReview = {
    userId: reviewInfor.value.userId,
    phoneId: reviewInfor.value.phoneId,
    variantId: reviewInfor.value.variantId,
    orderDetailId: reviewInfor.value.orderDetailId,
    rating: data.overallRating,
    text: data.comment,
    imageBase64: data.image 
  }
  await reviewStore.createReview(newReview)
  await phoneStore.fetchPhoneById(newReview.phoneId)
  reviewStore.setReviewProduct(null)
}

const defaultAvatar = '/src/assets/images/default_avatar.jpg'

const filters = [
  { label: 'Tất cả' },
  { label: '5 sao' },
  { label: '4 sao' },
  { label: '3 sao' },
  { label: '2 sao' },
  { label: '1 sao' },
]

const selectedFilter = ref('Tất cả')

const getBarPercent = (star) => {
  const total = Object.values(props.reviewCounts).reduce((a, b) => a + b, 0)
  return total ? Math.round(((props.reviewCounts[star] || 0) / total) * 100) : 0
}

const filteredReviews = computed(() => {
  if (selectedFilter.value === 'Tất cả') return props.reviews
  const star = parseInt(selectedFilter.value)
  if (!isNaN(star)) return props.reviews.filter(r => r.rating === star)
  return props.reviews
})
</script>


<style scoped>
.rating-summary {
  background: #eee;
  padding: 12px;
}
.radius {
  border-radius: 20px;
}
.h2 {
  font-weight: 600;
  line-height: 1.125;
  color: #363636;
}
.left {
  display: flex;
  flex-direction: column;
  align-items: center;
}
.average {
  font-size: 48px;
  font-weight: bold;
  display: flex;
  align-items: baseline;
  color: #000;
}
.out-of {
  font-size: 28px;
  color: #9e9e9e;
  margin-left: 4px;
}
.total-reviews {
  margin: 8px 0;
  font-size: 16px;
  color: #555;
}
.write-review-btn {
  background-color: #d70018;
  color: #fff;
  font-weight: bolder;
  font-size: 16px;
  text-transform: none;
  border-radius: 8px;
}
.write-review-btn:hover {
  background-color: #a80012;
}
.middle {
  display: flex;
  flex-direction: column;
  justify-content: center;
}
.bar-row {
  display: flex;
  align-items: center;
  margin: 6px 0;
  font-size: 16px;
  gap: 10px;
}
.star-label {
  width: 36px;
  font-weight: 500;
  color: #333;
  display: flex;
  align-items: center;
  justify-content: flex-end;
}
.yellow-star {
  color: #f5a623;
  margin-left: 2px;
}
.bar {
  flex: 1;
  height: 10px;
  background: #eee;
  border-radius: 6px;
  overflow: hidden;
}
.fill {
  height: 100%;
  background: #d70018;
  transition: width 0.3s ease-in-out;
}
.count {
  width: 90px;
  text-align: left;
  font-size: 14px;
  color: #666;
}
.filter-label {
  font-weight: 600;
  font-size: 16px;
  margin-bottom: 10px;
}
.review-row {
  border-top: 1px solid #eee;
}
</style>
