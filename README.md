# Bin-Blog

> *“Code is poetry, and the screen is our canvas.”*

[![.NET Status](https://img.shields.io/badge/.NET-10.0%20(Preview)-purple?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512bd4?style=flat-square&logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![Style](https://img.shields.io/badge/Style-Minimalist-black?style=flat-square)](https://github.com/zhangbbin/bin-blog)

---

## 0x00. Manifesto (设计宣言)

**Bin-Blog** 不是一个复杂的 CMS，也不是为了堆砌功能而生。它是对现代臃肿 Web 的一次反叛。
这是一个完全基于 **C#** 和 **Blazor Server** 构建的个人数字花园，旨在探索 .NET 10 的前沿特性与极简 UI 设计的完美平衡。

* **去噪 (De-noise):** 没有广告，没有追踪器，只有内容。
* **沉浸 (Immersion):** 专注于阅读体验，通过留白和排版呼吸。
* **纯粹 (Purity):** 前后端同构，C# 贯穿始终。

## 0x01. Anatomy (解剖结构)

这个生命体由以下核心器官组成：

### 🧠 The Core (大脑)
* **Runtime:** .NET 10 (Blazor Server App)
* **ORM:** Entity Framework Core (MySQL/SQLite Ready)
* **Auth:** 自研轻量级用户/权限系统 (Admin/Author/Reader)

### 💅 The Skin (皮肤)
* **CSS Architecture:** 不依赖庞大的 CSS 框架，手写 CSS Variables 实现动态主题。
* **Dark Mode:** 基于 `localStorage` 和 JS Interop 的光暗自动切换，致敬昼夜更替。
* **Responsive:** Grid + Flexbox 混合布局，从 4K 屏幕适配到移动端。

### 🔌 The Synapses (神经突触)
* **Giscus Integration:** 利用 GitHub Discussions 作为评论系统的后端，无缝且开源。
* **Markdown Rendering:** 使用 `Markdig` 管道，支持代码高亮与复杂排版。

## 0x02. The Blueprint (目录拓扑)

```text
Bin-Blog/
├── Components/          # UI 组件库
│   ├── Layout/          # 骨架 (MainLayout, NavMenu)
│   ├── Pages/           # 页面实体 (Home, PostDetail, About)
│   └── Shared/          # 细胞 (Sidebar, ThemeToggle, CommentSection)
├── Web/                 # 业务逻辑层
│   ├── Data/            # 数据模型 (User, BlogPost)
│   └── Services/        # 神经传导 (BlogService)
└── wwwroot/             # 静态资源 (css, js, favicon)
