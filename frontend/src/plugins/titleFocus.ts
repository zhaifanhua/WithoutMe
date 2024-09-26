// 页面标题聚焦插件
import { App, reactive, ref } from "vue";

// 获取页面中的 title 标签的内容及 link 标签的图标链接
const titleElement = document.querySelector("title");
const iconLinkElement = document.querySelector("link[rel~='icon']");
// 原始标题及图标链接
const originalTitle = ref(titleElement.textContent);
const originalIconLink = ref(iconLinkElement.getAttribute("href"));

// 配置项
const setTimeoutConfig = reactive({
  setTimeout: {
    onblurTime: 1000,
    focusTime: 2000,
  },
});
const titleConfig = reactive({
  title: {
    onblurTitle: "😭 离开我了！",
    focusTitle: "😉 欢迎回来！",
  },
});
const linkConfig = reactive({
  icon: {
    onblurLink: "/images/icon/onblur.svg",
    focusLink: "/images/icon/focus.svg",
  },
});

// 页面可见性变化时的标题与图标链接调整
const handleVisibilityChange = () => {
  const { onblurTime, focusTime } = setTimeoutConfig.setTimeout;
  const { onblurTitle, focusTitle } = titleConfig.title;
  const { onblurLink, focusLink } = linkConfig.icon;

  // 失去焦点
  if (document.hidden) {
    if (onblurTime >= 0) {
      setTimeout(() => {
        titleElement.textContent = onblurTitle;
        iconLinkElement.setAttribute("href", onblurLink);
      }, onblurTime);
    }
  }
  // 恢复焦点
  else {
    if (focusTime >= 0) {
      titleElement.textContent = focusTitle;
      iconLinkElement.setAttribute("href", focusLink);
      setTimeout(() => {
        titleElement.textContent = originalTitle.value;
        iconLinkElement.setAttribute("href", originalIconLink.value);
      }, focusTime);
    } else {
      titleElement.textContent = originalTitle.value;
      iconLinkElement.setAttribute("href", originalIconLink.value);
    }
  }
};

const setupTitleFocus = () => {
  document.addEventListener("visibilitychange", handleVisibilityChange, false);
};
const cleanupTitleFocus = () => {
  document.removeEventListener("visibilitychange", handleVisibilityChange);
};

export default {
  install: (app: App) => {
    // 在应用启动时设置事件监听器
    setupTitleFocus();

    // 在应用卸载时移除事件监听器
    app.config.globalProperties.$titleFocus = {
      setup: setupTitleFocus,
      cleanup: cleanupTitleFocus,
    };

    // 提供一个全局的组合式函数
    app.provide("useTitleFocus", () => ({
      setup: setupTitleFocus,
      cleanup: cleanupTitleFocus,
    }));
  },
};
