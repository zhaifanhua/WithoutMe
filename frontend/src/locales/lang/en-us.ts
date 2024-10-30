import { genMessage } from "../common";

const modules = import.meta.glob("./en-us/**/*.ts");
export default {
  message: {
    ...genMessage(modules, "en-us"),
  },
  momentLocale: null,
  momentLocaleName: "en-us",
};
