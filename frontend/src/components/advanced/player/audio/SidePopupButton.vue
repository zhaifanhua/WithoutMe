<!-- 侧边弹出按钮组件 -->

<template>
  <div class="side-popup">
    <button @click="toggleEject" :class="{ eject: isEject }" :title="buttonTitle">
      <Icon :icon="buttonIcon" />
    </button>
  </div>
</template>

<script setup lang="ts">
  import { computed } from "vue";
  import { Icon } from "@iconify/vue";

  const props = defineProps<{
    isEject: boolean;
  }>();

  const emit = defineEmits<{
    (e: "toggleEject"): void;
  }>();

  const buttonIcon = computed(() => {
    return props.isEject ? "solar:alt-arrow-right-linear" : "solar:alt-arrow-left-linear";
  });

  const buttonTitle = computed(() => {
    return props.isEject ? "收起播放器" : "展开播放器";
  });

  function toggleEject() {
    emit("toggleEject");
  }
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";
  @import "./_variables.scss";

  .side-popup {
    position: absolute;
    top: 50%;
    right: -$side-width;
    margin-right: -1px;
    transform: translateY(-50%);

    button {
      border: none;
      @include useBackdropFilter($base-bg-filter-blur);
      background-color: rgba($bg-color-secondary, 0.5);
      color: $text-color-primary;
      width: $side-width;
      height: $side-height;
      border-radius: 0 $border-radius-side $border-radius-side 0;
      cursor: pointer;
      transition: $transition;

      &.eject {
        @include useBackdropFilter($base-bg-filter-blur);
        background-color: rgba($bg-color-primary, 0.5);
      }
    }
  }
</style>
