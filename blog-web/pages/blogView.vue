<template>
    <v-app>
        <v-alert v-model="hasError" tile icon="$warning" color="warning" title="Invalid input" :text="errorMessage"
            closable />
        <v-container>
            <v-btn icon="mdi-arrow-left" elevation="0" @click="router.back()"></v-btn>
            <v-btn v-if="editing" variant="flat" color="success" class="ml-3"
                @click.once="submitBlogPost">Submit</v-btn>
            <v-menu v-else>
                <template v-slot:activator="{ props }">
                    <v-btn icon="mdi-dots-vertical" v-bind="props" elevation="0" class="ml-3"></v-btn>
                </template>

                <v-list>
                    <v-list-item density="compact" @click="enableEditing">
                        <v-list-item-title class="cursor-pointer">Edit</v-list-item-title>
                    </v-list-item>
                    <v-list-item density="compact" @click="showDeleteDialog = true">
                        <v-list-item-title>Delete</v-list-item-title>
                    </v-list-item>
                </v-list>
            </v-menu>
        </v-container>
        <v-container v-if="editing">
            <v-card elevation="3" height="auto" class="mx-auto" width="auto" max-width="1000">
                <v-sheet class="bg-surface-light pt-2">
                    <v-card-title class="font-weight-black">
                        <v-text-field label="Title" v-model="title"></v-text-field>
                    </v-card-title>
                </v-sheet>
                <v-card-text>
                    <v-textarea v-model="content" label="Start your blog post's content here :)"></v-textarea>
                </v-card-text>
                <v-card-actions>
                    <v-spacer />
                    <v-btn variant="flat" color="success" class="mr-3 mb-2" @click.once="submitBlogPost">Submit</v-btn>
                </v-card-actions>
            </v-card>
        </v-container>
        <v-container v-else>
            <v-card elevation="3" height="auto" class="mx-auto" width="auto" max-width="1000">
                <v-sheet class="bg-surface-light">
                    <v-card-title class="font-weight-black">
                        {{ blogPost.title }}
                    </v-card-title>
                    <v-card-subtitle class="pb-2 ml-1">
                        {{ `Published on ${blogPost.createdDate.substring(0, 10)}` }}
                    </v-card-subtitle>
                </v-sheet>
                <v-card-text class="pt-4">
                    {{ blogPost.content }}
                </v-card-text>
            </v-card>
        </v-container>
        <delete-blog-dialog v-model="showDeleteDialog" @accept="deleteBlogPost" />
    </v-app>
</template>

<script setup lang="ts">
import Axios from 'axios';
import type BlogPost from '~/scripts/blogPost';

let blogPostId: number;
const route = useRoute();
const router = useRouter();
const blogPost = ref<BlogPost>({
    blogPostId: -1,
    title: 'Placeholder',
    content: 'Placeholder',
    createdDate: '2024-08-14',
});
const showDeleteDialog = ref<boolean>(false);

const editing = ref<boolean>(false);
const title = ref<string>('');
const content = ref<string>('');
const errorMessage = ref<string>('');
const hasError = ref<boolean>(false);

onMounted(async () => {
    try {
        let stringId = route.query.id as string;
        blogPostId = parseInt(stringId);
        if (blogPostId === -1) {
            editing.value = true;
            title.value = '';
            content.value = '';
        } else {
            console.log(blogPostId);
            const url = `blog/getBlogPost?id=${blogPostId}`;
            const response = await Axios.get(url);
            blogPost.value = response.data;
        }
    } catch (error) {
        console.error('Error fetching selected blog post:', error);
    }
});

async function submitBlogPost() {
    if (title.value.length === 0 || content.value.length === 0) {
        hasError.value = true;
        errorMessage.value = 'Title and/or content must not be empty.'
    }
    else {
        const url = 'blog/postBlogPost';
        await Axios.post(url, {
            blogPostId: blogPostId,
            title: title.value,
            content: content.value,
        })
            .then(response => {
                errorMessage.value = '';
                hasError.value = false;
                router.back();
            })
            .catch(error => {
                hasError.value = true;
                errorMessage.value = error.response.data;
            });
    }
}

async function deleteBlogPost() {
    try {
        const url = `blog/deleteBlogPost/${blogPost.value.blogPostId}`;
        await Axios.post(url, null);
        router.back();
    } catch (error) {
        console.error('Error deleting post: ', error);
    }
}

function enableEditing() {
    editing.value = true;
    title.value = blogPost.value.title;
    content.value = blogPost.value.content;
}
</script>