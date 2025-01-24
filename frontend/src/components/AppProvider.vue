<!-- 应用程序提供器，用于提供全局组件 -->

<template>
  <div id="app-provider" class="app-provider">
    <Cursor></Cursor>
    <!-- <Loading></Loading>
    <Notification></Notification> -->
    <Background></Background>
    <APlayer></APlayer>
  </div>
  <slot></slot>
</template>

<script setup lang="ts">
  import Cursor from "@/components/generic/Cursor.vue";
  import Loading from "@/components/generic/Loading.vue";
  import Notification from "@/components/generic/Notification.vue";
  import Background from "@/components/generic/Background.vue";
  import APlayer from "@/components/advanced/player/audio/APlayer.vue";
</script>

<style scoped lang="scss"></style>
