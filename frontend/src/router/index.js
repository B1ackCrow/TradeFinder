import { createRouter, createWebHistory } from 'vue-router'

import HomeView from '@/views/HomeView.vue'
import AuthenticationView from '@/views/AuthenticationView.vue'
import LoginComponent from "@/components/authentication/LoginComponent.vue";
import RegistrationComponent from "@/components/authentication/RegistrationComponent.vue";
import AdministrationView from "@/views/AdministrationView.vue";

const router = createRouter({
    history: createWebHistory(),
    routes: [
        // {
        //     path: '/',
        //     name: 'home',
        //     component: HomeView
        // },
        {
            path: '/',
            name: 'administration',
            component: AdministrationView
        },
        {
            path: '/authentication',
            name: 'authentication',
            component: AuthenticationView,
            children: [
                {
                    path: 'login',
                    component: LoginComponent
                },
                {
                    path: 'registration',
                    component: RegistrationComponent
                }
            ]
        }
    ]
})

export default router