<!-- 内容容器 -->

<template>
  <div class="content-container">
    <slot></slot>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";

  .content-container {
    @include useTransition(all 0.3s ease-in-out);

    // 超小屏设备 phone
    @include useAutoScale(xs) {
      width: 100%;
    }
    // 小屏设备 pad
    @include useAutoScale(sm) {
      width: 95%;
    }
    // 中屏设备 notebook
    @include useAutoScale(md) {
      width: 90%;
    }
    // 大屏设备 desktop
    @include useAutoScale(lg) {
      width: 85%;
    }
    // 超大屏设备 tv
    @include useAutoScale(xl) {
      width: 80%;
    }
  }
</style>
