## 西瓜
CQuark（西瓜） 是一个简单的C#语法的脚本解析器。可以用于Unity热更新。支持Windows,iOS,Android平台。

* 本项目是在Unity3D项目环境下运行。如果在非Unity3D环境下运行，直接删除Assets/_Unity文件夹和Demo文件夹即可。

* 西瓜的前身是疯光无线前辈写的C#LightEvil和自己曾经写的一个脚本语言。

项目最新地址：    	https://github.com/flow119/CQuark

疯光无线前辈的项目地址：https://github.com/lightszero/cslightcore

以及疯光无线的Unity案例：https://github.com/lightszero/CSLightStudio



## 西瓜的优势

* 可以热更新。
* 纯C#语法，你不用去学lua了。
* 编辑器下无需生成代码，如果你乐意，你直接拿.cs文件后缀改成.txt就能用。
* 据说执行效率高于lua，但还未在iOS设备上测试过。




## 版本更新记录

2017-10-10 v0.7.6
    
    重新制作了范例
    类里的协程也能直接被调用
    完善了ScriptMono类，允许只编译单个脚本（string或者TextAsset都可以），详细见Demo07
    
2017-10-09 v0.7.5
    
    处理windows下用记事本写脚本会在开头出现一个不可解析的字符
    修正了带协程的循环中break逻辑错乱的问题。
    完善注册类型（增加Queue和Stack支持） 
     
2017-09-22 v0.7.4
    
    简化RegFunction函数

2017-09-17 v0.7.3
    
    新增switch case语法，支持switch case嵌套，但还不支持goto

2017-09-15 v0.7.2
    
    if后接else if无法解析的问题修复

2017-09-14 v0.7.1
    
    支持协程(Coroutine)
    增加ScriptMono（类似于MonoBehaviour）以及对应的Demo3
    ScriptMono增加Inspector（选择加载类型，重载文本按钮等）
    文件从StreamingAssets或Persistent目录加载（更接近热更方案）

2017-09-13 v0.7.0
    
    把cslightcore迁移过来，这个版本和cslight的0.64.1Beta完全一样
    Unity的Demo1(执行函数块)
    Unity的Demo2(从外部加载类并执行类里的函数)


## TODO
下个版本
* 优化编译速度，减少gc alloc以及重复的GetCodeKey
* 执行效率测试，主要看反射在iOS上的速度

下下个版本
* env改为全局唯一（出于效率考虑）
* 类似XLua和Bridge，把项目里的cs文件转换成可以动态替换为西瓜的脚本


## 联系我
QQ:181664367
