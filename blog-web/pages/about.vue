<template>
    <v-btn icon="mdi-arrow-left" elevation="0" class="mt-3 ml-3" @click="router.back()"></v-btn>
    <v-container>
        <v-card elevation="3" height="auto" class="my-2 mx-auto" width="auto" max-width="1000">
            <v-sheet class="bg-surface-light">
                <v-card-title class="font-weight-black">
                    About
                </v-card-title>
                <v-card-subtitle class="pb-2">
                    By Joshua Lester
                </v-card-subtitle>

            </v-sheet>
            <v-card-text class=" pt-4">
                This is a website to practice my skills in Vue, Vuetify, Nuxt, and ASP.NET.
            </v-card-text>
        </v-card>

    </v-container>
</template>

<script setup lang="ts">
const router = useRouter();
</script>