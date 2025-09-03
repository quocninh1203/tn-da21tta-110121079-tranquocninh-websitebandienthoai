<template>
    <!-- <v-row class="bg-red-accent-4 text-center"> -->
    <div class="note-container">
      <div class="note-item">
        <h4>Về chúng tôi</h4>
        <p>
          Chúng tôi là cửa hàng uy tín, chuyên cung cấp các sản phẩm điện thoại chính hãng với giá cả cạnh tranh và dịch vụ hỗ trợ khách hàng hàng đầu.
        </p>
      </div>
      <div class="note-item">
        <h4>Hỗ trợ khác hàng</h4>
        <ul>
          <li><a href="#">Chính sách bảo hành</a></li>
          <li><a href="#">Chính sách đổi trả</a></li>
          <li><a href="#">Hướng dẫn mua hàng</a></li>
          <li><a href="#">Chính sách bảo mật</a></li>
        </ul>
      </div>
      <div class="note-item">
        <h4>Liên hệ</h4>
        <ul>
          <li><strong>Địa chỉ: </strong> 123 Đường ABC, Phường 5, TP.TV</li>
          <li><strong>Email: </strong>ninhshop@gmail.com</li>
          <li><strong>Hotline: </strong>1800 1234</li>
          <li><strong>Thời gian: </strong> 7:00 - 19:00 (Cả tuần)</li>
        </ul>
      </div>
      <div class="note-item">
        <h4>Kết nối với chúng tôi</h4>
        <div class="social">
          <a href="https://facebook.com"><v-icon>mdi-facebook</v-icon></a>
          <a href="https://instagram.com"><v-icon>mdi-instagram</v-icon></a>
          <a href="https://youtube.com"><v-icon>mdi-youtube</v-icon></a>
          <a href="https://reddit.com"><v-icon>mdi-reddit</v-icon></a>
        </div>
      </div>
    </div>
    
    <v-footer class="bg-brown-darken-4 text-center d-flex flex-column"> 
      <div>
        Copyright
        <v-icon>
            mdi-copyright
        </v-icon>
        {{ new Date().getFullYear() }} — by <strong>Ninh Shop</strong>
      </div>
    </v-footer>
</template>

  <script>
  export default {
    name: 'FooterView',
    data: () => ({

    }),
  }
</script>

<style scoped>
.note-container{
  display: grid;
  grid-template-columns: repeat(4,1fr);
  background: linear-gradient(90deg, #ff5722 60%, #ff9800 100%);
  color: #fff;
}
.note-item{
  padding: 50px;
  font-size: 16px;
}
.note-item h4{
  font-size: 24px !important;
  font-weight: 600;
  margin-bottom: 20px;
}
.note-item li{
  list-style: none;
  color: #fff;
  padding-bottom: 10px;
}
.note-item a{
  text-decoration: none;
  color: #fff;
}
.note-item .social{
  font-size: 30px;
  display: grid;
  grid-template-columns: repeat(4,1fr);
  cursor: pointer;
}
</style>