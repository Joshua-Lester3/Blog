<template>
    <v-app>
        <v-container>

            <v-btn icon="mdi-arrow-left" elevation="0" @click="router.back()"></v-btn>
            <v-menu>
                <template v-slot:activator="{ props }">
                    <v-btn icon="mdi-dots-vertical" v-bind="props" elevation="0" class="ml-3"></v-btn>
                </template>

                <v-list>
                    <v-list-item density="compact" @click="">
                        <v-list-item-title class="cursor-pointer">Edit</v-list-item-title>
                    </v-list-item>
                    <v-list-item density="compact" @click="showDeleteDialog = true">
                        <v-list-item-title>Delete</v-list-item-title>
                    </v-list-item>
                </v-list>
            </v-menu>
        </v-container>
        <v-container>
            <v-card elevation="3" height="auto" class="mx-auto" width="auto" max-width="1000">
                <v-sheet class="bg-surface-light">
                    <v-card-title class="font-weight-black">
                        {{ blogPost?.title }}
                    </v-card-title>
                    <v-card-subtitle class="pb-2 ml-1">
                        {{ `Published on ${blogPost?.createdDate.substring(0, 10)}` }}
                    </v-card-subtitle>
                </v-sheet>
                <v-card-text class="pt-4">
                    {{ blogPost?.content }}
                </v-card-text>
            </v-card>
        </v-container>
        <delete-blog-dialog v-model="showDeleteDialog" @accept="deleteDocument" />
    </v-app>
</template>

<script setup lang="ts">
import Axios from 'axios';
import type BlogPost from '~/scripts/blogPost';

let blogPostId: number;
const route = useRoute();
const router = useRouter();
const blogPost = ref<BlogPost | null>(null);
const showDeleteDialog = ref<boolean>(false);

onMounted(async () => {
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
})

async function deleteDocument() {
    try {
        const url = `blog/deleteBlogPost/${blogPost.value?.blogPostId}`;
        await Axios.post(url, null);
        router.back();
    } catch (error) {
        console.error('Error deleting document: ', error);
    }
}
</script>