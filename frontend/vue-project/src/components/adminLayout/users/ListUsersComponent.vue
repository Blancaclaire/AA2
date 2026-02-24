<script setup lang="ts">
import UserCardComponent from './UserCardComponent.vue';
defineProps<{ users: any[] }>()
const emit = defineEmits(['delete-user'])
</script>

<template>
  <b-row class="g-4">
    <b-col v-for="user in users" :key="user.id" md="6" lg="4">
      <user-card-component :user="user" @delete-user="emit('delete-user', $event)">
      </user-card-component>
    </b-col>
  </b-row>

</template>