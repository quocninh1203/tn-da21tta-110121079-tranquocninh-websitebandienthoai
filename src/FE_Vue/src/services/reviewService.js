// src/services/reviewService.js
import api from "@/services/axiosInstance";

const reviewService = {
  createReview(data) {
    return api.post("/ReviewAPI/CreateReview", data);
  },
};

export default reviewService;
