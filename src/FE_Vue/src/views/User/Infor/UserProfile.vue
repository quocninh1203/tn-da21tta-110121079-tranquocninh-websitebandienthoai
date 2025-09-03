<template>
  <div class="user-profile-container">
    <!-- Header with Avatar -->
    <div class="profile-header">
      <div class="avatar-section">
        <div class="avatar-container">
          <img 
            v-if="userInfo.avatar" 
            :src="userInfo.avatar" 
            :alt="userInfo.fullName"
            class="avatar-image"
          />
          <v-icon v-else size="80" class="default-avatar">mdi-account-circle</v-icon>
          <div class="avatar-overlay" @click="changeAvatar">
            <v-icon color="white">mdi-camera</v-icon>
          </div>
        </div>
      </div>
      <div class="user-basic-info">
        <h1>{{ userInfo.fullName || 'Chưa cập nhật tên' }}</h1>
        <p class="user-email">{{ userInfo.email }}</p>
      </div>
    </div>

    <!-- Main Content -->
    <div class="profile-content">
      <!-- Personal Information Section -->
      <div class="info-section">
        <div class="section-header">
          <h2>Thông tin cá nhân</h2>
          <v-btn 
            v-if="!isEditingInfo" 
            color="orange" 
            variant="outlined" 
            @click="startEditInfo"
          >
            <v-icon start>mdi-pencil</v-icon>
            Chỉnh sửa
          </v-btn>
        </div>

        <v-form ref="infoForm" v-model="infoFormValid" class="info-form">
          <div class="form-grid">
            <!-- Họ tên -->
            <v-text-field
              v-model="editableInfo.fullName"
              label="Họ và tên *"
              variant="outlined"
              :readonly="!isEditingInfo"
              :rules="nameRules"
              prepend-inner-icon="mdi-account"
            />

            <!-- Email (chỉ đọc) -->
            <v-text-field
              v-model="userInfo.email"
              label="Email"
              variant="outlined"
              readonly
              prepend-inner-icon="mdi-email"
              hint="Email không thể thay đổi"
              persistent-hint
            />

            <!-- Số điện thoại -->
            <v-text-field
              v-model="editableInfo.phoneNumber"
              label="Số điện thoại *"
              variant="outlined"
              :readonly="!isEditingInfo"
              :rules="phoneRules"
              prepend-inner-icon="mdi-phone"
            />

            <!-- Địa chỉ -->
            <v-textarea
              v-model="editableInfo.address"
              label="Địa chỉ"
              variant="outlined"
              :readonly="!isEditingInfo"
              rows="3"
              prepend-inner-icon="mdi-map-marker"
            />
          </div>

          <!-- Form Actions -->
          <div v-if="isEditingInfo" class="form-actions">
            <v-btn 
              color="grey" 
              variant="outlined" 
              @click="cancelEditInfo"
              :disabled="isLoadingInfo"
            >
              Hủy
            </v-btn>
            <v-btn 
              color="orange" 
              @click="saveUserInfo"
              :loading="isLoadingInfo"
              :disabled="!infoFormValid"
            >
              Lưu thay đổi
            </v-btn>
          </div>
        </v-form>
      </div>

      <!-- Password Change Section -->
      <div class="password-section">
        <div class="section-header">
          <h2>Đổi mật khẩu</h2>
          <v-btn 
            v-if="!isEditingPassword" 
            color="blue" 
            variant="outlined" 
            @click="startEditPassword"
          >
            <v-icon start>mdi-lock</v-icon>
            Đổi mật khẩu
          </v-btn>
        </div>

        <div v-if="!isEditingPassword" class="password-info">
          <v-icon color="blue" class="password-icon">mdi-shield-check</v-icon>
          <div>
            <h3>Mật khẩu của bạn được bảo mật</h3>
            <p>Thay đổi mật khẩu định kỳ để bảo vệ tài khoản</p>
          </div>
        </div>

        <v-form v-if="isEditingPassword" ref="passwordForm" v-model="passwordFormValid" class="password-form">
          <div class="password-grid">
            <!-- Mật khẩu cũ -->
            <v-text-field
              v-model="passwordData.oldPassword"
              label="Mật khẩu hiện tại *"
              type="password"
              variant="outlined"
              :rules="[v => !!v || 'Vui lòng nhập mật khẩu hiện tại']"
              prepend-inner-icon="mdi-lock"
            />

            <!-- Mật khẩu mới -->
            <v-text-field
              v-model="passwordData.newPassword"
              label="Mật khẩu mới *"
              type="password"
              variant="outlined"
              :rules="newPasswordRules"
              prepend-inner-icon="mdi-lock-plus"
            />

            <!-- Xác nhận mật khẩu mới -->
            <v-text-field
              v-model="passwordData.confirmPassword"
              label="Xác nhận mật khẩu mới *"
              type="password"
              variant="outlined"
              :rules="confirmPasswordRules"
              prepend-inner-icon="mdi-lock-check"
            />
          </div>

          <!-- Password Form Actions -->
          <div class="form-actions">
            <v-btn 
              color="grey" 
              variant="outlined" 
              @click="cancelEditPassword"
              :disabled="isLoadingPassword"
            >
              Hủy
            </v-btn>
            <v-btn 
              color="blue" 
              @click="changePassword"
              :loading="isLoadingPassword"
              :disabled="!passwordFormValid"
            >
              Đổi mật khẩu
            </v-btn>
          </div>
        </v-form>
      </div>
    </div>

    <!-- Success Snackbar -->
    <v-snackbar
      v-model="showSuccessMessage"
      color="success"
      :timeout="3000"
    >
      {{ successMessage }}
      <template v-slot:actions>
        <v-btn variant="text" @click="showSuccessMessage = false">
          Đóng
        </v-btn>
      </template>
    </v-snackbar>

    <!-- Error Snackbar -->
    <v-snackbar
      v-model="showErrorMessage"
      color="error"
      :timeout="5000"
    >
      {{ errorMessage }}
      <template v-slot:actions>
        <v-btn variant="text" @click="showErrorMessage = false">
          Đóng
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useUserStore } from '@/stores/api/userStore'
import { useNotificationStore } from '@/stores/local/notification'
import { useAuthStore } from '@/stores/api/authStore'
import { getUserId } from '@/utils/jwtHelper'
import { convertToBase64 } from '@/utils/formatters'

const userStore = useUserStore()
const notificationStore = useNotificationStore()
const authStore = useAuthStore()

// Reactive data
const isEditingInfo = ref(false)
const isEditingPassword = ref(false)
const isLoadingInfo = ref(false)
const isLoadingPassword = ref(false)
const infoFormValid = ref(false)
const passwordFormValid = ref(false)
const showSuccessMessage = ref(false)
const showErrorMessage = ref(false)
const successMessage = ref('')
const errorMessage = ref('')

// User information - sử dụng dữ liệu từ userStore.profile
const userInfo = ref({
  id: null,
  fullName: '',
  email: '',
  phoneNumber: '',
  address: '',
  avatar: ''
})

const editableInfo = ref({
  fullName: '',
  phoneNumber: '',
  address: ''
})

// Password change data
const passwordData = ref({
  oldPassword: '',
  newPassword: '',
  confirmPassword: ''
})

// Validation rules
const nameRules = [
  v => !!v || 'Họ tên là bắt buộc',
  v => v.length >= 2 || 'Họ tên phải có ít nhất 2 ký tự',
  v => v.length <= 50 || 'Họ tên không được quá 50 ký tự'
]

const phoneRules = [
  v => !!v || 'Số điện thoại là bắt buộc',
  v => /^[0-9]{10}$/.test(v) || 'Số điện thoại phải có 10 chữ số'
]

const newPasswordRules = [
  v => !!v || 'Mật khẩu mới là bắt buộc',
  v => v.length >= 6 || 'Mật khẩu phải có ít nhất 6 ký tự',
  v => v !== passwordData.value.oldPassword || 'Mật khẩu mới phải khác mật khẩu cũ'
]

const confirmPasswordRules = [
  v => !!v || 'Vui lòng xác nhận mật khẩu',
  v => v === passwordData.value.newPassword || 'Mật khẩu xác nhận không khớp'
]

// Methods
// const showSuccess = (message) => {
//   successMessage.value = message
//   showSuccessMessage.value = true
// }

const showError = (message) => {
  errorMessage.value = message
  showErrorMessage.value = true
}

const startEditInfo = () => {
  isEditingInfo.value = true
  editableInfo.value = {
    fullName: userInfo.value.fullName,
    phoneNumber: userInfo.value.phoneNumber,
    address: userInfo.value.address
  }
}

const cancelEditInfo = () => {
  isEditingInfo.value = false
  editableInfo.value = {
    fullName: userInfo.value.fullName,
    phoneNumber: userInfo.value.phoneNumber,
    address: userInfo.value.address
  }
}

const saveUserInfo = async () => {
  if (!infoFormValid.value) return

  try {
    isLoadingInfo.value = true

    const updateData = {
      fullName: editableInfo.value.fullName,
      phoneNumber: editableInfo.value.phoneNumber,
      address: editableInfo.value.address
    }

    const userId = getUserId()
    const res = await userStore.updateProfile(userId, updateData)

    if (res && res.success === true) {
      // Cập nhật dữ liệu local từ store
      userInfo.value.fullName = userStore.profile.fullName
      userInfo.value.phoneNumber = userStore.profile.phoneNumber
      userInfo.value.address = userStore.profile.address
      isEditingInfo.value = false
      notificationStore.addNotification(res.message, "success")
    } else {
      notificationStore.addNotification("Có lỗi xảy ra khi cập nhật thông tin. Vui lòng thử lại!", "error")
    }
  } catch (error) {
    console.error('Lỗi khi cập nhật thông tin:', error)
    notificationStore.addNotification('Có lỗi xảy ra khi cập nhật thông tin. Vui lòng thử lại!', "error")
  } finally {
    isLoadingInfo.value = false
  }
}

const changeAvatar = () => {
  const input = document.createElement('input')
  input.type = 'file'
  input.accept = 'image/*'
  input.onchange = async (event) => {
    const file = event.target.files[0]
    if (file) {
      // Kiểm tra kích thước file (max 5MB)
      if (file.size > 5 * 1024 * 1024) {
        notificationStore.addNotification("Kích thước ảnh không được vượt quá 5MB", "error")
        return
      }
      try {
        const base64 = await convertToBase64(file)
        const userId = getUserId()
        const res = await userStore.updateProfile(userId, { imageBase64: base64 })
        userInfo.value.avatar = base64
        if (res && res.success === true) {
          notificationStore.addNotification("Cập nhật ảnh đại diện thành công!", "success")
        } else {
          notificationStore.addNotification("Cập nhật ảnh đại diện thất bại!", "error")
        }
      } catch (error) {
        console.error('Lỗi khi upload ảnh:', error)
        notificationStore.addNotification('Có lỗi xảy ra khi tải ảnh lên. Vui lòng thử lại!', "error");
      }
    }
  }
  input.click()
}

const startEditPassword = () => {
  isEditingPassword.value = true
  passwordData.value = {
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  }
}

const cancelEditPassword = () => {
  isEditingPassword.value = false
  passwordData.value = {
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  }
}

const changePassword = async () => {
  if (!passwordFormValid.value) return

  try {
    isLoadingPassword.value = true

    const userId = getUserId()
    const res = await authStore.changePassword(userId, {
      passWord: passwordData.value.oldPassword,
      newPassWord: passwordData.value.newPassword
    })

    isEditingPassword.value = false
    passwordData.value = {
      oldPassword: '',
      newPassword: '',
      confirmPassword: ''
    }

    if (res && res.success === true) {
      notificationStore.addNotification(res.message, 'success')
    } else {
      notificationStore.addNotification(res.message, 'error')
    }
  } catch (error) {
    console.error('Lỗi khi đổi mật khẩu:', error)
    notificationStore.addNotification('Có lỗi xảy ra khi đổi mật khẩu. Vui lòng thử lại!', 'error')
  } finally {
    isLoadingPassword.value = false
  }
}

// Tải thông tin người dùng từ userStore
const loadUserProfile = async () => {
  try {
    const userId = getUserId()
    
    if (!userId) {
      showError('Không thể xác định người dùng')
      return
    }
    
    // Gọi fetchProfile từ userStore
    await userStore.fetchProfile(userId)
    
    if (userStore.profile) {
      // Cập nhật dữ liệu local từ store
      userInfo.value = {
        id: userStore.profile.id || userId,
        fullName: userStore.profile.fullName || '',
        email: userStore.profile.email || '',
        phoneNumber: userStore.profile.phoneNumber || '',
        address: userStore.profile.address || '',
        avatar: userStore.profile.imageUrl || ''
      }
      
      editableInfo.value = {
        fullName: userStore.profile.fullName || '',
        phoneNumber: userStore.profile.phoneNumber || '',
        address: userStore.profile.address || ''
      }
    }
    
  } catch (error) {
    notificationStore.addNotification('Không thể tải thông tin người dùng', 'error')
  }
}
// Lifecycle
onMounted(() => {
  loadUserProfile()
})
</script>

<style scoped>
.user-profile-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.profile-header {
  display: flex;
  align-items: center;
  gap: 24px;
  background: linear-gradient(135deg, #ff6b35, #ff8c42);
  border-radius: 16px;
  padding: 32px;
  margin-bottom: 32px;
  color: white;
}

.avatar-section {
  flex-shrink: 0;
}

.avatar-container {
  position: relative;
  width: 100px;
  height: 100px;
}

.avatar-image {
  width: 100%;
  height: 100%;
  border-radius: 50%;
  object-fit: cover;
  border: 3px solid rgba(255, 255, 255, 0.3);
}

.default-avatar {
  color: rgba(255, 255, 255, 0.8);
  width: 100px;
  height: 100px;
}

.avatar-overlay {
  position: absolute;
  bottom: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.7);
  border-radius: 50%;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: background 0.3s;
}

.avatar-overlay:hover {
  background: rgba(0, 0, 0, 0.9);
}

.user-basic-info h1 {
  margin: 0 0 8px 0;
  font-size: 1.8rem;
  font-weight: 600;
}

.user-email {
  margin: 0;
  opacity: 0.9;
  font-size: 1.1rem;
}

.profile-content {
  display: flex;
  flex-direction: column;
  gap: 32px;
}

.info-section,
.password-section {
  background: white;
  border-radius: 16px;
  padding: 32px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
  padding-bottom: 16px;
  border-bottom: 2px solid #f5f5f5;
}

.section-header h2 {
  margin: 0;
  color: #333;
  font-size: 1.4rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 20px;
  margin-bottom: 24px;
}

.password-grid {
  display: flex;
  flex-direction: column;
  gap: 20px;
  max-width: 400px;
  margin-bottom: 24px;
}

.form-actions {
  display: flex;
  gap: 16px;
  justify-content: flex-end;
}

.password-info {
  display: flex;
  gap: 16px;
  align-items: center;
  padding: 20px;
  background: #f8f9ff;
  border-radius: 12px;
  border-left: 4px solid #2196f3;
  flex-direction: column;
  text-align: center;
}

.password-icon {
  font-size: 32px;
}

.password-info h3 {
  margin: 0 0 4px 0;
  color: #333;
  font-size: 1.1rem;
}

.password-info p {
  margin: 0;
  color: #666;
  font-size: 0.9rem;
}

@media (max-width: 768px) {
  .user-profile-container {
    padding: 16px;
  }
  
  .profile-header {
    flex-direction: column;
    text-align: center;
    padding: 24px;
  }
  
  .info-section,
  .password-section {
    padding: 24px 16px;
  }
  
  .form-grid {
    grid-template-columns: 1fr;
  }
  
  .section-header {
    flex-direction: column;
    gap: 16px;
    align-items: stretch;
  }
  
  .form-actions {
    flex-direction: column;
  }
  
  .password-info {
    flex-direction: column;
    text-align: center;
  }
} 
</style>
