<template>
  <!-- <div class="star-rating">
    <div
      v-for="index in 5"
      :key="index"
      class="star-wrapper"
    >
      <span class="star-background">★</span>
      <span
        class="star-foreground"
        :style="{ width: getStarFillPercent(index) + '%' }"
      >
        ★
      </span>
    </div>
  </div> -->

  <v-rating
  half-increments
  hover
  readonly
  :length="5"
  :size="28"
  :model-value="rating"
  color="warning"
  active-color="warning"
 />
</template>

<script setup>
const props = defineProps({
  rating: {
    type: Number,
    required: true,
  },
});

// const getStarFillPercent = (index) => {
//   const full = Math.floor(props.rating);
//   const remainder = props.rating - full;

//   if (index <= full) return 100;
//   if (index === full + 1) return Math.round(remainder * 100);
//   return 0;
// };
</script>

<!-- <style scoped>
.star-rating {
  display: flex;
  gap: 4px;
}

.star-wrapper {
  position: relative;
  font-size: 24px;
  width: 24px;
  height: 24px;
}

.star-background,
.star-foreground {
  position: absolute;
  top: 0;
  left: 0;
  font-size: 24px;
  line-height: 1;
}

.star-background {
  color: #ccc; /* Màu xám nền */
  z-index: 1;
}

.star-foreground {
  color: gold; /* Màu vàng tô */
  z-index: 2;
  overflow: hidden;
  white-space: nowrap;
}
</style> -->
