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
  </v-app>
</template>

<script setup lang="ts">
import { useDisplay } from 'vuetify';

const navDrawer = ref(false);
const router = useRouter();
const display = ref(useDisplay());

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