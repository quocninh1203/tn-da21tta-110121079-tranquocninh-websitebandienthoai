<template>
    <div class="home-page">
        <HomeHeader />

        <TheMenu />

        <HomeCenter />

        <ProductOnDisplay />
    
        <Footer />
    </div>

</template>

<script setup>
import HomeHeader from '@/components/Homepages/HomeHeader.vue'
import TheMenu from '@/components/Global/TheMenu.vue';
import HomeCenter from '@/components/Homepages/HomeCenter.vue';
// import Phone3D from '@/components/Homepages/Phone3D.vue';
import ProductOnDisplay from '@/components/Homepages/ProductOnDisplay.vue';
import Footer from '@/components/Global/Footer.vue';
import { onMounted } from 'vue'
import { usePhoneStore } from '@/stores/api/phoneStore'
import { useBrandStore } from '@/stores/api/brandStore';

const phoneStore = usePhoneStore()
const brandStore = useBrandStore()

onMounted(() => {
  phoneStore.fetchActivePhones()
  brandStore.fetchAllBrands()
})
</script>

<style scoped>
.home-page{
    background-color: #eee;
}
</style>