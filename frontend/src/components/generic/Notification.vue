<!-- 消息组件 -->

<template>
  <div id="notification" class="notification"></div>
</template>

<script setup lang="ts"></script>

<style lang="scss">
  @import "@/styles/site";

  .notification {
    @include useZindex("notification");
  }
</style>
