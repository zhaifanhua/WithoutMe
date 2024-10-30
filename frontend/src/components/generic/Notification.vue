<!-- 消息组件 -->

<template>
  <div id="notification" class="notification"></div>
</template>

<script setup lang="ts"></script>

<style lang="scss">
  @use "@/styles/site";

  .notification {
    @include mixins.useZindex("notification");
  }
</style>
