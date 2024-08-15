<template>
    <v-app class="ma-5">
        <v-container>
            <v-card v-for="(post, index) in posts" elevation="3" height="auto" class="my-5 mx-auto" width="auto"
                max-width="1000" @click="router.push(`/blogView?id=${post.blogPostId}`)">
                <v-card-title class="font-weight-black">
                    {{ post.title }}
                </v-card-title>
                <v-card-subtitle class="mb-1 ml-3">
                    {{ `Published on ${post.createdDate.substring(0, 10)}` }}
                </v-card-subtitle>
                <v-card-text class="bg-surface-light pt-4">
                    {{ post.content }}
                </v-card-text>
            </v-card>
        </v-container>
    </v-app>
</template>

<script setup lang="ts">
import Axios from 'axios';
import type BlogPost from '~/scripts/blogPost';

const router = useRouter();
const posts = ref<Array<BlogPost>>();

onMounted(async () => {
    try {
        const url = 'blog/getBlogList';
        const response = await Axios.get(url);
        posts.value = response.data;
    } catch (error) {
        console.error('Error fetching blog list: ', error);
    }
});

</script>