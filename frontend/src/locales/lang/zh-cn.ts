import { genMessage } from "../common";

const modules = import.meta.glob("./zh-cn/**/*.ts");
export default {
  message: {
    ...genMessage(modules, "zh-cn"),
  },
  momentLocale: null,
  momentLocaleName: "zh-cn",
};
