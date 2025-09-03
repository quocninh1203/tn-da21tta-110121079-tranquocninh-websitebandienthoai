import api from "@/services/axiosInstance";

const brandService = {
  createBrand(data) {
    return api.post("/BrandAPI/CreateBrand", data);
  },

  updateBrand(brandId, data) {
    return api.put(`/BrandAPI/UpdateBrand?brandId=${brandId}`, data);
  },

  getBrandById(brandId) {
    return api.get(`/BrandAPI/GetBrand?brandId=${brandId}`);
  },

  getAllBrands() {
    return api.get("/BrandAPI/GetAllBrands");
  },

  deleteBrand(brandId) {
    return api.delete(`/BrandAPI/DeleteBrand?brandId=${brandId}`);
  },
};

export default brandService;
