import { createRouter, createWebHistory } from 'vue-router'
import { getUserRole } from '@/utils/jwtHelper';

import HomeView from '../views/HomePage.vue'
import RegisterView from '@/views/RegisterView.vue'
import LoginForm from '@/views/LoginFormView.vue'
import ProductDetail from '@/views/ProductDetailView.vue';
import AccountManager from '@/views/User/Infor/AccountManager.vue';

// import admin vue
// import AdminView from '@/views/Admin/AdminView.vue'

// import AdminDashboardView from '@/views/Admin/Dashboard/AdminDashboardView.vue';

// import AdminProductView from '@/views/Admin/Product/AdminProductList.vue'
// import AdminProductCreate from '@/views/Admin/Product/AdminProductCreate.vue';
// import AdminProductOptionList from '@/views/Admin/Product/Options/AdminProductOptionList.vue';

// import AdminCategoryCreate from '@/views/Admin/Category/AdminCategoryCreate.vue';
// import AdminCategoryView from '@/views/Admin/Category/AdminCategoryList.vue';
// import AdminCategoryUpdate from '@/views/Admin/Category/AdminCategoryUpdate.vue';

// import AdminOderView from '@/views/Admin/Order/AdminOrderList.vue';
// import AdminOrderDetail from '@/views/Admin/Order/AdminOrderDetail.vue';

// import user vue
// import ProfileView from '@/views/User/ProfileView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/about',
      component: () => import('@/views/AboutPage.vue')
    },
    {
      path: '/contact',
      component: () => import('@/views/ContactPage.vue')
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView
    },
    {
      path: '/login',
      name: 'login',
      component: LoginForm
    },
    {
      path: '/products',
      component: () => import('@/views/ProductPage.vue'),
    },
    {
      path: '/product/:slug',
      name: 'product',
      component: ProductDetail
    },
    {
      path: '/category/:id',
      name: 'category',
      // component: ProductDetail
    },
    {
      path: '/cart',
      name: 'cart',

      component: () => import('../views/User/CartView.vue')
    },
    {
      path: '/order',
      name: 'order',
      component: () => import('../views/User/Pay/OrderView.vue')
    },
    {
      path: '/order/payment',
      name: 'QRPayment',
      component: () => import('@/views/User/Pay/QRPaymentView.vue'),
      props: true,
    },
    {
      path: '/order/success',
      name: 'orderSuccess',
      component: () => import('@/views/User/Pay/OrderSuccessView.vue'),
    },
    {
      path: '/account',
      name: 'account',
      component: AccountManager,
      redirect: '/account/profile',
      children: [
        {
          path: 'profile',
          component: () => import('@/views/User/Infor/UserProfile.vue')
        },
        {
          path: 'orders',
          name: 'orders',
          component: () => import('@/views/User/Infor/OrderListView.vue')
        },
        {
          path: 'orders/:orderId',
          name: 'OrderDetail',
          component: () => import('../views/User/Infor/OrderDetailView.vue'),
          props: true,
          meta: { requiresAuth: true }
        },
      ]
    },
    {
      path: '/admin',
      component: () => import('@/views/Admin/AdminDashboard.vue'),
      redirect: '/admin/dashboard',
      children: [
        {
          path: 'dashboard',
          component: () => import('@/views/Admin/Dashboard/AdminDashboardView.vue'),
        },   
        {
          path: 'products',
          component: () => import('@/views/Admin/Product/ProductList.vue'),
        },     
        {
          path: 'product/:slug',
          name: 'adminProductDetail',
          component: () => import('@/views/Admin/Product/ProductDetail.vue'),
        }, 
        {
          path: 'orders',
          component: () => import('@/views/Admin/Order/OrderList.vue'),
        },
        {
          path: 'order/:code',
          name: 'adminOrderDetail',
          component: () => import('@/views/Admin/Order/OrderDetail.vue'),
        },
        {
          path: 'customers',
          name: 'CustomerList',
          component: () => import('@/views/Admin/Customer/CustomerList.vue'),
        },
        {
          path: 'categories',
          component: () => import('@/views/Admin/Category/CategoryList.vue'),
        },
      //   {
      //     path: 'category/create',
      //     component: AdminCategoryCreate,
      //   },
      //   {
      //     path: 'category/update/:id',
      //     component: AdminCategoryUpdate,
      //   },
      ]
    }
  ]
});

// Navigation Guard 
router.beforeEach((to, from, next) => {
  const role = getUserRole();

  console.log(role);
  
  const isAdminRoute = to.path.startsWith('/admin');
  const isUserRoute = to.path.startsWith('/cart') || to.path.startsWith('/orders') || to.path.startsWith('/profile');

  // Trường hợp chưa đăng nhập
  if (role === undefined || role === null) {
    if (isAdminRoute || isUserRoute) {
      return next({ path: '/login', query: { redirect: to.fullPath } }); 
    }
    return next(); // các trang public như /, /login, /register,...
  }

  // Nếu là user (roleId = 0)
  if (role === 0) {
    if (isAdminRoute) {
      return next('/notfound');
    }
    return next(); // Cho phép truy cập /cart, /order, /profile
  }

    // Nếu là admin (roleId = 1)
  if (role === 1) {
    return next(); // Admin có thể truy cập mọi nơi (nếu bạn muốn chặn admin truy cập user route thì có thể bổ sung)
  }

  return next(); // fallback

});

export default router
