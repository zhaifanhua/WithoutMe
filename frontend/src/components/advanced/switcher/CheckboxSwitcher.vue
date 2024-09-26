<template>
  <div id="switcher" class="switcher" role="radio-group">
    <button type="button" aria-label="Switch theme" class="switch-btn">
      <i></i>
    </button>
  </div>
</template>

<script setup lang="ts">
  import { onMounted } from "vue";
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";
  @import "@/styles/base/themes.scss";

  .switcher {
    display: inline-flex;
    height: 100px;
    width: 100px;
    transform: translateY(-50%);
    background-color: #000000;
  }
  .switch-btn {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    background-color: #fff;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
  }
</style>
