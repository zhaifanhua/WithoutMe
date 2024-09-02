<template>
  <div id="background" class="background">
    <canvas id="canvas" class="canvas"></canvas>
    <div class="cover"></div>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';

  .background {
    @include useZindex('background');
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-image: url('/images/background/2.jpg');
    background-position: center;
    background-repeat: no-repeat;
    background-size: cover;
  }
</style>
m , ,,
