<template>
    <div class="header-app">
        <div class="header-left-app" @click="home">
            <h1 class="name-app">NShop</h1>
        </div>
        <div class="header-right-app">
            <!--  -->
        </div>
    </div>
    <div class="padding"></div>
</template>

<script setup>
import { useRouter } from "vue-router";

const router = useRouter();

const home = () => {
    router.push('/')
}
</script>

<style>
.header-app{
    display: flex;
    position: fixed;
    height: 50px;
    width: 100vw    ;
    z-index: 1000;
    background-color: #fff;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); 
}
.header-left-app {
    display: flex;
    align-items: center;
    width: 250px;
    height: 50px;
    background-color: #4CAF50;
    clip-path: polygon(0% 0%, 100% 0%, 80% 100%, 0% 100%);
}
.name-app{
    font-family: Arial, sans-serif;
    font-size: 24px;
    font-weight: bold;
    text-transform: uppercase;
    margin-left: 30px;
    background: linear-gradient(45deg,rgba(150, 62, 222, 0.86), white, rgba(150, 62, 222, 0.86));
    background-clip: text;
    color: transparent;
}
.header-right-app{
}
.padding{
    height: 50px;
    width: 100vw;
}
</style>