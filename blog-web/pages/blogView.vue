<template>
    <v-btn icon="mdi-arrow-left" elevation="0" class="mt-3 ml-3" @click="router.back()"></v-btn>
    <v-container>
        <v-card elevation="3" height="auto" class="my-2 mx-auto" width="auto" max-width="1000">
            <v-sheet class="bg-surface-light">
                <v-card-title class="font-weight-black">
                    {{ blogPost?.title }}
                </v-card-title>
                <v-card-subtitle class="pb-2 ml-1">
                    {{ `Published on ${blogPost?.createdDate.substring(0, 10)}` }}
                </v-card-subtitle>

            </v-sheet>
            <v-card-text class=" pt-4">
                {{ blogPost?.content }}
            </v-card-text>
        </v-card>

    </v-container>
</template>

<script setup lang="ts">
import Axios from 'axios';
import type BlogPost from '~/scripts/blogPost';

let blogPostId: number;
const route = useRoute();
const router = useRouter();
const blogPost = ref<BlogPost | null>(null);

try {
    let stringId = route.query.id as string;
    blogPostId = parseInt(stringId);
    console.log(blogPostId);
    const url = `blog/getBlogPost?id=${blogPostId}`;
    const response = await Axios.get(url);
    blogPost.value = response.data;
} catch (error) {
    console.error('Error fetching selected blog post:', error);
}
</script>