// Composables
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    component: () => import('@/layouts/default/LayoutDefault.vue'),
    children: [
      {
        path: '/index.html',
        name: 'Home',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "home" */ '@/views/HomePage.vue'),
      },
      {
        path: '/auth',
        name: 'Auth',
        component: () => import('@/views/AuthPage.vue'),
      },
      {
        path: '/main',
        name: 'Main',
        component: () => import('@/views/MainPage.vue'),
      },
      {
        path: '/admin',
        name: 'Admin',
        component: () => import('@/views/AdminPage.vue'),
      },
      {
        path: '/search',
        name: 'Search',
        component: () => import('@/views/SearchPage.vue'),
      },
      {
        path: '/myprofile',
        name: 'My Profile',
        component: () => import('@/views/MyProfilePage.vue'),
      },
      {
        path: '/profile/:id',
        name: 'Profile',
        component: () => import('@/views/ProfilePage.vue'),
      },
      {
        path: '/groupProfile/:id',
        name: 'Group Profile',
        component: () => import('@/views/GroupProfilePage.vue'),
      },
      {
        path: '/test',
        name: 'Test',
        component: () => import('@/views/TestPage.vue'),
      }
    ],
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
})

export default router
