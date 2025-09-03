<template>
  <div @click="navigate" class="item">
      <div id="item_icon">
          <slot name="icon"></slot>
      </div>
      <div id="detail">
          <slot name="detail"></slot>
      </div>
  </div>
</template>

<script>
export default {
  props: {
    toRoute: {
      type: String,
      required: false,
    },
    typeItem: {
      type: String,
      required: false,
      default: ""
    },
  },
  methods: {
    navigate() {
      if (this.toRoute) {
        this.$router.push(this.toRoute); // Sử dụng Vue Router để điều hướng
      }
    },
  },
};
</script>

<style scoped>
.item {
    display: flex;
    align-items: center;
    justify-content: center;
    color: white; /* Màu trắng cho chữ và icon */
    padding: 10px 10px;
    border-radius: 10px;
    cursor: pointer;
    transition: background-color 0.3s ease;
    box-sizing: border-box;
    margin: 0 10px;
}

.item:hover {
    background-color: rgba(255, 255, 255, 0.9);
    color: #f00;
}

#item-icon {
    align-items: center;
    justify-content: center;
    font-size: 24px;
}

#item-detail {
    font-size: 16px;
    font-weight: 500;
}
.user{

}
</style>