<!-- 内容容器 -->

<template>
  <div class="content-container">
    <slot></slot>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;

  .content-container {
    @include mixins.useTransition(all 0.3s ease-in-out);

    // 超小屏设备 phone
    @include mixins.useAutoScale(xs) {
      width: 100%;
    }
    // 小屏设备 pad
    @include mixins.useAutoScale(sm) {
      width: 95%;
    }
    // 中屏设备 notebook
    @include mixins.useAutoScale(md) {
      width: 90%;
    }
    // 大屏设备 desktop
    @include mixins.useAutoScale(lg) {
      width: 85%;
    }
    // 超大屏设备 tv
    @include mixins.useAutoScale(xl) {
      width: 80%;
    }
  }
</style>
