import { defineStore } from "pinia";
import brandService from "@/services/brandService";

export const useBrandStore = defineStore("brand", {
  state: () => ({
    brands: [],
    selectedBrand: null,
    loading: false,
    error: null,
  }),

  actions: {
    async fetchAllBrands() {
      this.loading = true;
      try {
        const res = await brandService.getAllBrands();
        this.brands = res.data.model || [];
      } catch (err) {
        this.error = err;
      } finally {
        this.loading = false;
      }
    },

    async fetchBrandById(brandId) {
      this.loading = true;
      try {
        const res = await brandService.getBrandById(brandId);
        this.selectedBrand = res.data.model;
      } catch (err) {
        this.error = err;
      } finally {
        this.loading = false;
      }
    },

    async createBrand(data) {
      try {
        const res = await brandService.createBrand(data);
        await this.fetchAllBrands();
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async updateBrand(brandId, data) {
      try {
        const res = await brandService.updateBrand(brandId, data);
        await this.fetchAllBrands();
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async deleteBrand(brandId) {
      try {
        const res = await brandService.deleteBrand(brandId);
        await this.fetchAllBrands();
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },
  },

  getters: {
    brandList(){
        return this.brands;
    }
  }
});
