import { set } from "lodash-es";
import type { LocaleType } from "@/types/config";

export const loadLocalePool: LocaleType[] = [];

/**
 * 设置HTML页面的语言属性
 *
 * 此函数的目的是根据传入的locale参数，设置HTML页面的lang属性
 * 这对于国际化应用尤其重要，可以确保浏览器等客户端正确地识别和处理页面的语言信息
 *
 * @param locale 页面语言的标识符
 */
export function setHtmlPageLang(locale: LocaleType) {
  document.querySelector("html")?.setAttribute("lang", locale);
}

/**
 * 设置加载语言池的回调函数
 *
 * 该函数允许传入一个回调函数，以便在需要时执行加载语言包的操作
 * 它主要用于初始化或动态加载语言环境时，提供一种机制将支持的语言环境传递给某个处理函数
 *
 * @param cb 回调函数，接收一个LocaleType数组作为参数 这个回调函数的目的是为了处理（例如存储或分发）提供的语言池
 */
export function setLoadLocalePool(cb: (loadLocalePool: LocaleType[]) => void) {
  cb(loadLocalePool);
}

/**
 * 根据语言文件生成消息配置对象
 *
 * 该函数用于处理语言文件，将它们整理成一个对象结构，以便后续在应用中方便地访问不同语言的文本
 * 它会遍历提供的语言文件记录，解析文件路径，然后将文件内容（假设是默认导出的对象）挂载到一个返回的对象上
 * 如果文件路径包含多个部分，会递归地创建对象属性来反映这种结构
 *
 * @param langs - 一个记录，键是语言文件的路径，值是语言文件的内容（默认导出的对象）
 * @param prefix - 语言文件夹的前缀，默认为'lang'，用于在文件路径中识别语言文件
 * @returns 返回一个对象，该对象的结构反映了语言文件的路径结构，值是语言文件的内容
 */
export function genMessage(langs: Record<string, Record<string, any>>, prefix = "lang") {
  const obj: Recordable = {};

  // 遍历每个语言文件的路径和内容
  Object.keys(langs).forEach(key => {
    // 获取语言文件模块的内容，假设是默认导出
    const langFileModule = langs[key].default;
    // 处理文件路径，去除前缀和文件扩展名，得到用于对象属性的键
    let fileName = key.replace(`./${prefix}/`, "").replace(/^\.\//, "");
    const lastIndex = fileName.lastIndexOf(".");
    fileName = fileName.substring(0, lastIndex);
    const keyList = fileName.split("/");
    // 移除第一个部分作为模块名，剩余部分组成键路径
    const moduleName = keyList.shift();
    const objKey = keyList.join(".");

    // 如果有有效的模块名
    if (moduleName) {
      // 如果有键路径，说明是嵌套结构
      if (objKey) {
        // 为模块创建或获取对象
        set(obj, moduleName, obj[moduleName] || {});
        // 将语言文件模块设置在对应的嵌套路径上
        set(obj[moduleName], objKey, langFileModule);
      } else {
        // 直接将语言文件模块设置在模块名下
        set(obj, moduleName, langFileModule || {});
      }
    }
  });
  // 返回构建好的消息配置对象
  return obj;
}
