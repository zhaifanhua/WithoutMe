<!-- 加载组件 -->

<template>
  <div id="loading" class="loading"></div>
</template>

<script setup lang="ts"></script>

<style lang="scss">
  @import '@/styles/site';

  .loading {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 5px;
    background-color: rgba(0, 0, 0, 0.5);
    @include useZindex('loading');
  }
</style>
