import api from "@/services/axiosInstance";

const phoneImageService = {
  createPhoneImage(data) {
    return api.post("/PhoneImageAPI/CreatePhoneImage", data);
  },

  updatePhoneImage(phoneImageId, data) {
    return api.put(`/PhoneImageAPI/UpdatePhoneImage?phoneImageId=${phoneImageId}`, data);
  },

  deletePhoneImage(phoneImageId) {
    return api.delete(`/PhoneImageAPI/DeletePhoneImage?phoneImageId=${phoneImageId}`);
  },
}

export default phoneImageService;
