module.exports = {
  // 超过最大值换行
  printWidth: 120,
  // 缩进字节数
  tabWidth: 2,
  // 缩进不使用tab，使用空格
  useTabs: false,
  // 句尾添加分号
  semi: true,
  // 在Vue文件中缩进脚本和样式
  vueIndentScriptAndStyle: true,
  // 使用单引号代替双引号
  singleQuote: true,
  // 在对象中使用引号
  quoteProps: 'as-needed',
  // 在对象，数组括号与文字之间加空格 "{ foo: bar }"
  bracketSpacing: true,
  // 在对象或数组最后一个元素后面是否加逗号
  trailingComma: 'es5',
  // 在JSX中使用单引号代替双引号
  jsxSingleQuote: false,
  //  (x) => {} 箭头函数参数只有一个时是否要有小括号。avoid：省略括号
  arrowParens: 'avoid',
  // 在文件的顶部插入一个@format标记，指定文件格式
  insertPragma: false,
  // 在文件的顶部插入一个@format标记，指定文件格式
  requirePragma: false,
  // 在HTML中空格敏感度
  htmlWhitespaceSensitivity: 'strict',
  // 结尾
  endOfLine: 'lf',
  // 起始行
  rangeStart: 0,
  // 在HTML中将开始标签的右尖括号放在同一行
  bracketSameLine: true,
  overrides: [
    {
      files: '*.vue',
      options: {
        // 单属性换行
        singleAttributePerLine: true,
        // 在Vue文件中缩进脚本和样式
        vueIndentScriptAndStyle: true,
      },
    },
  ],
};
