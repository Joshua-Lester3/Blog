<template>
  <v-app>
    <v-app-bar :elevation="4" @click="navDrawer = false">
      <template v-slot:prepend>
        <v-app-bar-nav-icon class="ml-1" @click.prevent.stop="navDrawer = true" />
      </template>

      <v-app-bar-title>My Blog</v-app-bar-title>
    </v-app-bar>
    <!-- SSR with v-nav-drawer causes hydration mismatch. 
     No current fix as Vuetify is not SSR friendly according to a github user (?)
     Only found solution is to render v-nav-drawer on client side -->
    <ClientOnly>
      <v-navigation-drawer v-model="navDrawer" :width="navigationDrawerWidth" disable-resize-watcher temporary>
        <v-list class="text-center">
          <v-list-item @click="router.push('/'); navDrawer = false;">
            <v-icon>mdi-home</v-icon> Home
          </v-list-item>
          <v-list-item @click="router.push('/about')">
            <v-icon>mdi-information-slab-box-outline</v-icon> About
          </v-list-item>
        </v-list>
      </v-navigation-drawer>
    </ClientOnly>
    <v-main>
      <NuxtPage />
    </v-main>
    <v-footer class="bg-grey-lighten-1">
    <v-row justify="center" no-gutters>
      <v-btn
        v-for="link in links"
        :key="link.text"
        class="mx-2"
        color="white"
        rounded="xl"
        variant="text"
        @click="router.push(link.url)"
      >{{link.text}}
      </v-btn>
      <v-col class="text-center mt-4" cols="12">
        {{ new Date().getFullYear() }} — <strong>My Blog</strong>
      </v-col>
    </v-row>
  </v-footer>
  </v-app>
</template>

<script setup lang="ts">
import { useDisplay } from 'vuetify';
import TokenService from '~/scripts/tokenService';

interface Link {
  text: string;
  url: string;
}

const navDrawer = ref(false);
const router = useRouter();
const display = ref(useDisplay());
const tokenService = ref(new TokenService());
provide('TOKEN', tokenService);
const links = [
  {
    text: 'Home',
    url: '/',
  },
  {
    text: 'About',
    url: '/about',
  },
  {
    text: 'Login',
    url: '/login',
  }
]

const navigationDrawerWidth = computed(() => {
  switch (display.value.name) {
    case 'sm': return 200;
    case 'md': return 225;
    case 'lg': return 250;
    case 'xl': return 275;
    case 'xxl': return 300;
  }
});

</script>