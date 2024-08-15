<template>
    <v-dialog v-model="modelValue">
        <v-card min-height="200" class="mx-auto" min-width="300">
            <v-sheet>
                <v-card-title>Deletion</v-card-title>
            </v-sheet>
            <v-card-text>
                Would you like to delete this document?
            </v-card-text>
            <v-card-actions>
                <v-btn variant="text" @click="modelValue = false">Cancel</v-btn>
                <v-btn color="error" variant="tonal" @click="emits('accept'); modelValue = false;">Delete</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
const modelValue = defineModel<boolean>();
const emits = defineEmits(['accept']);
</script>