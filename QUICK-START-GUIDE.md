# 快速开始指南 / Quick Start Guide

> 这是 Bin-Blog 项目的快速开始指南，帮助你快速了解项目结构并开始开发。
> This is a quick start guide for the Bin-Blog project to help you understand the structure and start developing.

---

## 🚀 项目启动 / Project Setup

### 前置要求 / Prerequisites
- **.NET 10 SDK** (Preview)
- **MySQL 8.0+**
- **Redis** (可选，用于缓存 / Optional, for caching)
- **IDE**: Visual Studio 2025, VS Code, or JetBrains Rider

### 启动步骤 / Setup Steps

```bash
# 1. 克隆项目 / Clone repository
git clone https://github.com/zhangbbin/Bin-Blog.git
cd Bin-Blog

# 2. 配置数据库连接 / Configure database connection
# 编辑 appsettings.json，修改数据库连接字符串
# Edit appsettings.json, update database connection string

# 3. 运行数据库迁移 / Run database migrations
dotnet ef database update

# 4. 启动项目 / Run the project
dotnet watch

# 5. 浏览器访问 / Open browser
# https://localhost:5001
```

---

## 📁 项目结构 / Project Structure

```
Bin-Blog/
├── Components/              # Blazor 组件 / Blazor Components
│   ├── Layout/             # 布局组件（导航、主布局）/ Layout components
│   ├── Pages/              # 页面组件（首页、详情页等）/ Page components
│   └── Shared/             # 共享组件（评论、主题切换）/ Shared components
├── Web/                    # 业务逻辑层 / Business Logic Layer
│   ├── Data/               # 数据模型（BlogPost, User）/ Data models
│   ├── Models/             # DbContext / Database context
│   └── Services/           # 服务类（BlogService）/ Service classes
├── Migrations/             # EF Core 迁移文件 / EF Core migrations
├── wwwroot/                # 静态资源（CSS, JS, 图片）/ Static assets
├── Program.cs              # 应用程序入口 / Application entry point
└── appsettings.json        # 配置文件 / Configuration file
```

---

## 🎯 最快上手：5 分钟添加第一个功能 / 5-Minute Quick Win

### 示例 1：添加文章分类功能 / Example 1: Add Category Feature

**第 1 步：创建 Category 模型**
```csharp
// 文件：Web/Data/Category.cs
namespace Bin_Blog.Web.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
```

**第 2 步：更新 BlogPost 模型**
```csharp
// 在 BlogPost.cs 中添加
public int? CategoryId { get; set; }
public Category? Category { get; set; }
```

**第 3 步：更新 DbContext**
```csharp
// 在 BlogDbContext.cs 中添加
public DbSet<Category> Categories { get; set; } = null!;
```

**第 4 步：生成迁移**
```bash
dotnet ef migrations add AddCategoryFeature
dotnet ef database update
```

**第 5 步：在页面中显示分类**
```razor
@* 在 Components/Pages/Home.razor 中添加 *@
@if (post.Category != null)
{
    <span class="category-badge">@post.Category.Name</span>
}
```

✅ **完成！你刚刚添加了文章分类功能！**

---

### 示例 2：添加阅读时间显示 / Example 2: Add Reading Time Display

**使用 AI 提示词：**
```
在 Bin-Blog 项目的文章列表页（Home.razor）中显示阅读时间：
1. BlogPost 模型已经有 ReadingTime 计算属性
2. 在文章卡片的元数据区域显示阅读时间
3. 格式："{X} min read"
4. 使用现有的 .post-meta 样式类
5. 保持极简设计风格
```

**AI 会生成类似这样的代码：**
```razor
<div class="post-meta">
    <span>@post.PublishDate.ToString("yyyy-MM-dd")</span>
    <span> · </span>
    <span>@post.ReadingTime</span> ✅ 已经有了！
</div>
```

---

## 🛠️ 常用开发命令 / Common Commands

### 数据库相关 / Database

```bash
# 创建新迁移 / Create new migration
dotnet ef migrations add MigrationName

# 应用迁移 / Apply migrations
dotnet ef database update

# 回滚迁移 / Rollback migration
dotnet ef database update PreviousMigrationName

# 删除最后一个迁移 / Remove last migration
dotnet ef migrations remove

# 查看迁移列表 / List migrations
dotnet ef migrations list
```

### 开发相关 / Development

```bash
# 启动开发服务器（热重载）/ Start dev server with hot reload
dotnet watch

# 编译项目 / Build project
dotnet build

# 发布项目 / Publish project
dotnet publish -c Release

# 清理项目 / Clean project
dotnet clean
```

### Git 工作流 / Git Workflow

```bash
# 创建功能分支 / Create feature branch
git checkout -b feature/your-feature-name

# 查看状态 / Check status
git status

# 提交代码 / Commit changes
git add .
git commit -m "feat: add your feature"

# 推送到远程 / Push to remote
git push origin feature/your-feature-name
```

---

## 📊 核心数据模型关系 / Core Data Model Relationships

```
User (用户)
  ↓ 1:N
BlogPost (文章)
  ↓ 1:N
Comment (评论，未实现)

BlogPost 字段说明 / BlogPost Fields:
- Title: 标题 / Title
- Summary: 摘要 / Summary
- Content: Markdown 正文 / Markdown content
- Tags: 标签列表 / Tags list
- AuthorId: 作者 ID / Author ID
- IsPublished: 是否发布 / Is published
- IsDeleted: 是否删除 / Is deleted
- IsPinned: 是否置顶 / Is pinned
- ViewCount: 浏览量 / View count
- LikeCount: 点赞数 / Like count
```

---

## 🎨 UI 组件复用 / Reusable UI Components

### 文章卡片组件示例 / Post Card Component Example

```razor
@* 可以创建 Components/Shared/PostCard.razor *@
<article class="card">
    <div class="post-meta">
        <span>@Post.PublishDate.ToString("yyyy-MM-dd")</span>
        <span> · </span>
        <span>@Post.ReadingTime</span>
    </div>
    
    <h2 class="post-title">
        <a href="/post/@Post.Id">@Post.Title</a>
    </h2>
    
    <p class="post-summary">@Post.Summary</p>
    
    <a href="/post/@Post.Id" class="read-more">
        阅读全文 &rarr;
    </a>
</article>

@code {
    [Parameter]
    public BlogPost Post { get; set; } = null!;
}
```

---

## 🔍 常见问题解决 / Troubleshooting

### 问题 1：数据库连接失败 / Issue 1: Database Connection Failed

**错误信息：**
```
Unable to connect to any of the specified MySQL hosts.
```

**解决方案：**
1. 检查 MySQL 是否运行：`systemctl status mysql`
2. 检查 `appsettings.json` 中的连接字符串
3. 确认数据库用户名和密码正确
4. 创建数据库：`CREATE DATABASE bin-blog;`

---

### 问题 2：Blazor 重连失败 / Issue 2: Blazor Reconnection Failed

**现象：** 页面显示"正在尝试重新连接..."

**解决方案：**
1. 检查浏览器控制台是否有 SignalR 错误
2. 清除浏览器缓存，重新加载页面
3. 检查防火墙是否阻止 WebSocket 连接
4. 使用 `dotnet watch` 重启开发服务器

---

### 问题 3：主题切换不生效 / Issue 3: Theme Toggle Not Working

**解决方案：**
1. 检查浏览器控制台是否有 JavaScript 错误
2. 确认 `wwwroot/themeManager.js` 已正确加载
3. 清除 localStorage：`localStorage.clear()`
4. 刷新页面

---

## 💡 开发技巧 / Development Tips

### Tip 1: 使用 Blazor DevTools
```bash
# 安装 Blazor DevTools 浏览器扩展
# Chrome: https://chrome.google.com/webstore
# Edge: https://microsoftedge.microsoft.com/addons
```

### Tip 2: 启用详细日志
```json
// appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### Tip 3: 使用 EF Core 查询日志
```csharp
// 在 Program.cs 中添加
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           .EnableSensitiveDataLogging()  // 显示参数值
           .EnableDetailedErrors());      // 显示详细错误
```

### Tip 4: Blazor 组件调试
```razor
@* 在组件中添加断点 *@
@code {
    protected override void OnInitialized()
    {
        // 在这里设置断点（F9）
        var posts = BlogService.GetPosts();
        System.Diagnostics.Debugger.Break(); // 或者代码断点
    }
}
```

---

## 🎓 学习资源 / Learning Resources

### 官方文档 / Official Documentation
- [Blazor 教程](https://learn.microsoft.com/zh-cn/aspnet/core/blazor/)
- [EF Core 指南](https://learn.microsoft.com/zh-cn/ef/core/)
- [C# 编程指南](https://learn.microsoft.com/zh-cn/dotnet/csharp/)

### 推荐视频 / Recommended Videos
- [Blazor 入门系列](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oUJCA3DCzKT79Oe3kdKEceX)
- [EF Core 深入浅出](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oX7uTkjYwvCJDG2qhcSzwZ6)

### 社区资源 / Community Resources
- [Blazor University](https://blazor-university.com/)
- [Awesome Blazor](https://github.com/AdrienTorris/awesome-blazor)

---

## 📝 下一步建议 / Next Steps

根据你的需求，建议按以下顺序开发：

### 如果你想快速看到效果 / For Quick Results
1. ✅ 添加文章搜索功能（2-3 小时）
2. ✅ 优化代码高亮显示（1 小时）
3. ✅ 添加阅读进度条（30 分钟）

### 如果你想构建完整系统 / For Complete System
1. 🔐 实现用户认证（1-2 天）
2. ⚙️ 创建后台管理面板（2-3 天）
3. 📊 添加统计分析功能（1 天）

### 使用 AI-DEVELOPMENT-PROMPTS.md
打开 `AI-DEVELOPMENT-PROMPTS.md` 文件，复制对应功能的提示词，提供给 AI 助手（GitHub Copilot / ChatGPT），让 AI 帮你生成代码！

---

## 🤝 贡献指南 / Contributing

欢迎贡献！请遵循以下步骤：

1. Fork 项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'feat: Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

### 提交信息规范 / Commit Message Convention
```
feat: 新功能 / New feature
fix: 修复 Bug / Bug fix
docs: 文档更新 / Documentation
style: 代码格式 / Code style
refactor: 重构 / Refactoring
test: 测试 / Testing
chore: 构建/工具 / Build/Tools
```

---

## 📞 联系方式 / Contact

- **GitHub**: [zhangbbin/Bin-Blog](https://github.com/zhangbbin/Bin-Blog)
- **Issues**: [提交问题](https://github.com/zhangbbin/Bin-Blog/issues)
- **Discussions**: [参与讨论](https://github.com/zhangbbin/Bin-Blog/discussions)

---

**祝你开发愉快！🚀 / Happy Coding! 🚀**

---

**Last Updated**: 2026-02-08  
**Version**: 1.0
