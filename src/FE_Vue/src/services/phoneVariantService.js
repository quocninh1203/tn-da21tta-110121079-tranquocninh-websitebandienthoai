import api from "@/services/axiosInstance";

const phoneVariantService = {
  createPhoneVariant(data) {
    return api.post("/PhoneVariantAPI/CreatePhoneVariant", data);
  },

  updatePhoneVariant(variantId, data) {
    return api.put(`/PhoneVariantAPI/UpdatePhoneVariant?variantId=${variantId}`, data);
  },

}

export default phoneVariantService;