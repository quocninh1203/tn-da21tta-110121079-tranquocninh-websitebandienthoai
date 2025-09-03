import { defineStore } from "pinia";
import reviewService from "@/services/reviewService";

export const useReviewStore = defineStore("review", {
  state: () => ({
    reviewProduct: null, // sản phẩm sẽ được đánh giá
    loading: false,
    error: null,
  }),

  actions: {
    setReviewProduct(product) {
      this.reviewProduct = product;
    },

    async createReview(data) {
      this.loading = true;
      try {
        const res = await reviewService.createReview(data);
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      } finally {
        this.loading = false;
      }
    },
  },

  // persist: true, // giữ lại reviewProduct khi reload trang
});
