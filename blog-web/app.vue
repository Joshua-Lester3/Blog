<template>
  <v-app>
    <v-app-bar :elevation="4">
      <template v-slot:prepend>
        <v-app-bar-nav-icon class="ml-1" />
      </template>

      <v-app-bar-title>My Blog</v-app-bar-title>
    </v-app-bar>
    <v-main>
      <NuxtPage />
    </v-main>
  </v-app>
</template>
