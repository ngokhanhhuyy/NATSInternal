<script setup lang="ts">
import { ref, type Ref } from "vue";
import User, { type Model } from "./User.vue";
import UserSFC from "./UserSFC.vue";

const userList = ref<Model[]>([]);
</script>

<template>
  <ul>
    <li v-for="user in userList" v-bind:key="user.userName">
      <User v-bind:model="user">
        <template #header="model">
          <div>{{ model.userName }}</div>
        </template>
        <template #default>
          <div class="">{{ user.userName }}</div>
        </template>
      </User>

      <UserSFC v-model="user" v-bind:myName="123">
        <template #header="{ model }">
          <div>{{model.userName}}</div>
        </template>
        <template>
          <div class="">{{ user.userName }}</div>
        </template>
      </UserSFC>
    </li>
  </ul>
</template>

<style>
</style>