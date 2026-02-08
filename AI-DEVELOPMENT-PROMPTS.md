# AI 开发提示词 / AI Development Prompts

> 本文档为 Bin-Blog 个人博客项目后续开发提供 AI 助手提示词，帮助快速完成功能迭代。
> This document provides AI assistant prompts for future development of the Bin-Blog personal blog project.

---

## 📋 当前项目状态 / Current Project Status

### 技术栈 / Tech Stack
- **Framework**: .NET 10 + Blazor Server
- **Database**: MySQL + Entity Framework Core
- **Cache**: Redis (StackExchange.Redis)
- **Markdown**: Markdig
- **Comments**: Giscus (GitHub Discussions)
- **Authentication**: JWT Bearer (已配置但未实现 / Configured but not implemented)

### 已实现功能 / Implemented Features
✅ 博客文章列表展示 / Blog post listing  
✅ 文章详情页（Markdown 渲染）/ Post detail page with Markdown rendering  
✅ 标签筛选功能 / Tag filtering  
✅ 昼夜主题切换 / Light/Dark theme toggle  
✅ Giscus 评论系统集成 / Giscus comment system integration  
✅ 数据库迁移（User + BlogPost 表）/ Database migrations (User + BlogPost tables)  
✅ 极简 UI 设计 / Minimalist UI design  

### 缺失功能 / Missing Features
❌ 用户认证与登录 / User authentication and login  
❌ 后台管理面板 / Admin panel  
❌ 文章创建/编辑/删除界面 / Post CRUD UI  
❌ 全站搜索 / Site-wide search  
❌ RSS 订阅 / RSS feed  
❌ SEO 优化（Meta 标签、Sitemap）/ SEO optimization  
❌ 图片上传与管理 / Image upload and management  
❌ 文章草稿系统 / Draft system  
❌ 访问统计（ViewCount 自动增加）/ View count tracking  
❌ 代码高亮优化 / Code syntax highlighting  

---

## 🎯 推荐开发优先级 / Recommended Development Priority

### Phase 1: 核心功能完善 / Core Features (High Priority)

#### 1.1 用户认证与授权系统
**AI Prompt (中文):**
```
在 Bin-Blog 项目中实现 JWT 认证系统：
1. 创建 AuthService 服务类，实现用户注册、登录、密码哈希（使用 BCrypt 或 PBKDF2）
2. 在 Program.cs 中配置 JWT Bearer 认证（已有依赖包，需配置 Issuer、Audience、SecretKey）
3. 创建 /login 和 /register 页面（Blazor Razor 组件）
4. 实现基于角色的授权（Admin、Author、Reader），使用 [Authorize] 特性
5. 添加登录状态持久化（使用 Blazor Server 的 AuthenticationStateProvider）
6. 在 NavMenu 中添加登录/登出按钮，显示当前用户名
7. 确保密码安全存储，遵循 OWASP 最佳实践

要求：
- 使用现有的 User 模型和 BlogDbContext
- 密码必须加盐哈希存储，不能明文
- JWT Token 有效期设置为 7 天，支持刷新
- 添加 CSRF 防护（Blazor Server 内置 AntiForgery）
```

**AI Prompt (English):**
```
Implement JWT authentication system in the Bin-Blog project:
1. Create AuthService class with user registration, login, and password hashing (using BCrypt or PBKDF2)
2. Configure JWT Bearer authentication in Program.cs (package exists, need Issuer, Audience, SecretKey)
3. Create /login and /register pages (Blazor Razor components)
4. Implement role-based authorization (Admin, Author, Reader) using [Authorize] attribute
5. Add login state persistence (using Blazor Server's AuthenticationStateProvider)
6. Add login/logout buttons to NavMenu, display current username
7. Ensure secure password storage following OWASP best practices

Requirements:
- Use existing User model and BlogDbContext
- Passwords must be salted and hashed, no plaintext
- JWT Token validity: 7 days with refresh support
- Add CSRF protection (Blazor Server built-in AntiForgery)
```

---

#### 1.2 后台管理面板
**AI Prompt (中文):**
```
为 Bin-Blog 创建后台管理系统：
1. 创建 /admin 路由，使用 [Authorize(Roles = "Admin,Author")] 保护
2. 实现以下管理功能：
   - 文章列表（包含草稿、已发布、已删除状态筛选）
   - 新建文章（Markdown 编辑器，可使用 Blazor 组件或集成第三方如 SimpleMDE）
   - 编辑文章（支持标签管理、置顶、允许评论等选项）
   - 删除文章（软删除，设置 IsDeleted = true）
   - 文章预览功能
3. 创建 AdminBlogService 扩展 BlogService，添加以下方法：
   - CreatePostAsync, UpdatePostAsync, DeletePostAsync
   - GetDraftPostsAsync, GetDeletedPostsAsync
4. UI 设计保持极简风格，与前台一致
5. 添加表单验证（标题、内容不能为空，标题最大 200 字符）
6. 自动保存草稿功能（每 30 秒自动保存一次到 localStorage）

技术要求：
- 使用 Blazor Server 的 @rendermode InteractiveServer
- Markdown 编辑器支持实时预览
- 上传的文章自动关联到当前登录用户（AuthorId）
```

**AI Prompt (English):**
```
Create admin panel for Bin-Blog:
1. Create /admin route, protect with [Authorize(Roles = "Admin,Author")]
2. Implement following admin features:
   - Post list (filter by draft, published, deleted status)
   - Create new post (Markdown editor, use Blazor component or integrate 3rd party like SimpleMDE)
   - Edit post (support tag management, pinning, comment control)
   - Delete post (soft delete, set IsDeleted = true)
   - Post preview functionality
3. Create AdminBlogService extending BlogService with methods:
   - CreatePostAsync, UpdatePostAsync, DeletePostAsync
   - GetDraftPostsAsync, GetDeletedPostsAsync
4. UI design should maintain minimalist style consistent with frontend
5. Add form validation (title, content required, title max 200 chars)
6. Auto-save draft feature (save to localStorage every 30 seconds)

Technical requirements:
- Use Blazor Server's @rendermode InteractiveServer
- Markdown editor with real-time preview
- Auto-associate posts with current logged-in user (AuthorId)
```

---

### Phase 2: 用户体验提升 / UX Enhancements (Medium Priority)

#### 2.1 全站搜索功能
**AI Prompt (中文):**
```
为 Bin-Blog 实现全站搜索功能：
1. 在 BlogService 中添加 SearchPostsAsync 方法，支持：
   - 标题模糊搜索（使用 LIKE %keyword%）
   - 内容全文搜索（考虑 MySQL 全文索引）
   - 标签精确匹配
2. 创建 /search 页面，包含：
   - 搜索输入框（带搜索建议/自动完成）
   - 搜索结果列表（高亮关键词）
   - 空状态提示（无结果时显示）
3. 在导航栏添加搜索图标，点击展开搜索框
4. 使用 Redis 缓存热门搜索词（过期时间 1 小时）
5. 搜索性能优化：
   - 添加数据库索引（Title, Tags）
   - 限制搜索结果最多 50 条
   - 防抖处理（用户输入停止 500ms 后才发起请求）

技术细节：
- 使用 EF Core 的 .Where() 和 .Contains() 实现模糊搜索
- Redis 使用 StackExchange.Redis 库（已安装）
- 搜索结果按相关性排序（优先匹配标题，其次内容）
```

**AI Prompt (English):**
```
Implement site-wide search for Bin-Blog:
1. Add SearchPostsAsync method to BlogService supporting:
   - Title fuzzy search (using LIKE %keyword%)
   - Content full-text search (consider MySQL full-text index)
   - Tag exact match
2. Create /search page including:
   - Search input box (with suggestions/autocomplete)
   - Search results list (highlight keywords)
   - Empty state message (when no results)
3. Add search icon to navbar, expand search box on click
4. Use Redis to cache popular search terms (1 hour expiration)
5. Search performance optimization:
   - Add database indexes (Title, Tags)
   - Limit results to max 50 items
   - Debounce handling (trigger request 500ms after user stops typing)

Technical details:
- Use EF Core's .Where() and .Contains() for fuzzy search
- Redis using StackExchange.Redis library (already installed)
- Sort results by relevance (prioritize title match, then content)
```

---

#### 2.2 SEO 优化与 RSS 订阅
**AI Prompt (中文):**
```
为 Bin-Blog 添加 SEO 优化和 RSS 订阅功能：

**Part 1: SEO 优化**
1. 在每个页面添加动态 Meta 标签（使用 Blazor 的 HeadContent）：
   - 文章详情页：title, description, og:image, og:url
   - 首页：站点描述、关键词
2. 生成 sitemap.xml：
   - 创建 /sitemap.xml 端点（使用 Minimal API）
   - 包含所有已发布文章的 URL、更新时间、优先级
   - 每日凌晨自动更新（使用后台服务）
3. 添加 robots.txt（允许搜索引擎爬取）
4. 结构化数据：为文章页添加 JSON-LD Schema.org 标记（Article 类型）

**Part 2: RSS Feed**
1. 创建 /rss.xml 或 /feed 端点
2. 使用 System.ServiceModel.Syndication 生成 RSS 2.0 格式
3. 包含最新 20 篇文章的标题、摘要、链接、发布时间
4. 在页面 <head> 中添加 RSS 自动发现链接
5. 在页面底部添加 RSS 订阅图标

技术要求：
- sitemap.xml 和 rss.xml 使用 XML 序列化
- RSS feed 缓存 1 小时（使用内存缓存或 Redis）
- 确保 URL 使用绝对路径（包含域名）
```

**AI Prompt (English):**
```
Add SEO optimization and RSS feed to Bin-Blog:

**Part 1: SEO Optimization**
1. Add dynamic Meta tags to each page (using Blazor's HeadContent):
   - Post detail page: title, description, og:image, og:url
   - Home page: site description, keywords
2. Generate sitemap.xml:
   - Create /sitemap.xml endpoint (using Minimal API)
   - Include all published post URLs, update time, priority
   - Auto-update daily at midnight (using background service)
3. Add robots.txt (allow search engine crawling)
4. Structured data: Add JSON-LD Schema.org markup (Article type) to post pages

**Part 2: RSS Feed**
1. Create /rss.xml or /feed endpoint
2. Use System.ServiceModel.Syndication to generate RSS 2.0 format
3. Include latest 20 posts with title, summary, link, publish date
4. Add RSS auto-discovery link in page <head>
5. Add RSS subscription icon in page footer

Technical requirements:
- sitemap.xml and rss.xml use XML serialization
- RSS feed cached for 1 hour (use memory cache or Redis)
- Ensure URLs are absolute paths (include domain)
```

---

#### 2.3 图片上传与管理
**AI Prompt (中文):**
```
为 Bin-Blog 实现图片上传和管理系统：

**Part 1: 图片上传功能**
1. 创建 /admin/images 图片管理页面
2. 实现图片上传组件（支持拖拽上传）：
   - 限制文件类型（jpg, png, gif, webp）
   - 限制文件大小（最大 5MB）
   - 自动生成缩略图（使用 SixLabors.ImageSharp）
3. 图片存储策略：
   - 本地存储：/wwwroot/uploads/images/{year}/{month}/
   - 文件名：{timestamp}_{guid}.{extension}
   - 可选：对接云存储（阿里云 OSS 或 AWS S3）
4. 数据库记录：
   - 创建 Image 表（Id, FileName, FilePath, FileSize, UploadedAt, UploadedBy）
   - 添加到 BlogDbContext

**Part 2: 图片选择器**
1. 在 Markdown 编辑器中添加"插入图片"按钮
2. 弹出图片选择对话框，显示已上传的图片
3. 支持图片搜索和分页加载
4. 选择图片后自动插入 Markdown 语法：![alt](url)

**Part 3: 图片优化**
1. 上传时自动压缩大图（宽度超过 1920px 则缩放）
2. 生成多种尺寸（原图、大图、缩略图）
3. 使用 WebP 格式（浏览器支持时）
4. 添加图片延迟加载（lazy loading）

技术要求：
- 使用 Blazor InputFile 组件接收上传
- 图片处理使用 SixLabors.ImageSharp 库
- 添加上传进度条显示
- 防止文件名冲突（使用 GUID）
```

**AI Prompt (English):**
```
Implement image upload and management system for Bin-Blog:

**Part 1: Image Upload Feature**
1. Create /admin/images image management page
2. Implement image upload component (support drag-and-drop):
   - Restrict file types (jpg, png, gif, webp)
   - Limit file size (max 5MB)
   - Auto-generate thumbnails (using SixLabors.ImageSharp)
3. Image storage strategy:
   - Local storage: /wwwroot/uploads/images/{year}/{month}/
   - Filename: {timestamp}_{guid}.{extension}
   - Optional: Integrate cloud storage (Aliyun OSS or AWS S3)
4. Database records:
   - Create Image table (Id, FileName, FilePath, FileSize, UploadedAt, UploadedBy)
   - Add to BlogDbContext

**Part 2: Image Picker**
1. Add "Insert Image" button in Markdown editor
2. Show image selection dialog with uploaded images
3. Support image search and pagination
4. Auto-insert Markdown syntax after selection: ![alt](url)

**Part 3: Image Optimization**
1. Auto-compress large images on upload (scale if width > 1920px)
2. Generate multiple sizes (original, large, thumbnail)
3. Use WebP format (when browser supports)
4. Add image lazy loading

Technical requirements:
- Use Blazor InputFile component for upload
- Image processing using SixLabors.ImageSharp library
- Add upload progress bar
- Prevent filename conflicts (use GUID)
```

---

### Phase 3: 高级功能 / Advanced Features (Low Priority)

#### 3.1 文章统计与分析
**AI Prompt (中文):**
```
为 Bin-Blog 添加文章统计和分析功能：

1. 访问量统计：
   - 在 PostDetail 页面加载时自动增加 ViewCount
   - 使用 Redis 去重（同一 IP 24 小时内只计数一次）
   - 每小时批量更新数据库（使用后台服务）

2. 创建 /admin/statistics 统计页面，显示：
   - 总文章数、总访问量、总点赞数
   - 最热门文章 Top 10（按 ViewCount 排序）
   - 文章发布趋势图（按月统计）
   - 标签使用频率统计

3. 可视化图表（可选）：
   - 使用 Chart.js 或 ApexCharts
   - 访问量趋势折线图
   - 标签分布饼图

4. 导出功能：
   - 支持导出统计数据为 CSV/JSON
   - 生成月度/年度报告

技术要求：
- Redis 使用 SET 数据结构存储访问记录
- 使用 IHostedService 实现后台定时任务
- 统计查询使用异步方法（避免阻塞主线程）
```

**AI Prompt (English):**
```
Add article statistics and analytics to Bin-Blog:

1. View count tracking:
   - Auto-increment ViewCount when PostDetail page loads
   - Use Redis for deduplication (same IP counted once per 24 hours)
   - Batch update database hourly (using background service)

2. Create /admin/statistics page showing:
   - Total posts, total views, total likes
   - Top 10 most popular posts (sorted by ViewCount)
   - Article publishing trend chart (monthly statistics)
   - Tag usage frequency statistics

3. Visualization charts (optional):
   - Use Chart.js or ApexCharts
   - View count trend line chart
   - Tag distribution pie chart

4. Export functionality:
   - Support exporting statistics as CSV/JSON
   - Generate monthly/yearly reports

Technical requirements:
- Redis using SET data structure to store view records
- Use IHostedService for background scheduled tasks
- Statistical queries use async methods (avoid blocking main thread)
```

---

#### 3.2 代码高亮优化
**AI Prompt (中文):**
```
优化 Bin-Blog 的代码高亮显示：

1. 集成 Prism.js 或 Highlight.js：
   - 在 _Layout.cshtml 或 App.razor 中引入 CSS 和 JS
   - 选择合适的主题（例如：Dracula、One Dark）
   - 支持主题切换（亮色/暗色代码主题随页面主题切换）

2. Markdown 代码块增强：
   - 显示语言标签（如 C#, JavaScript, Python）
   - 添加"复制代码"按钮
   - 支持行号显示
   - 支持代码行高亮（使用 ```c# {1,3-5} 语法）

3. 性能优化：
   - 使用 IntersectionObserver 延迟高亮（代码块进入视口时才执行）
   - 缓存高亮结果（避免重复渲染）

4. 支持的语言：
   - 常用语言：C#, JavaScript, TypeScript, Python, Java, Go, SQL
   - Web 技术：HTML, CSS, SCSS, JSON, YAML, Markdown
   - Shell：Bash, PowerShell

实现方式：
- 在 Markdig 管道中配置代码高亮扩展
- 或在前端使用 JavaScript 动态高亮（推荐）
- 确保代码块样式与页面整体设计一致
```

**AI Prompt (English):**
```
Optimize code syntax highlighting for Bin-Blog:

1. Integrate Prism.js or Highlight.js:
   - Include CSS and JS in _Layout.cshtml or App.razor
   - Choose appropriate theme (e.g., Dracula, One Dark)
   - Support theme switching (code theme switches with page theme)

2. Enhanced Markdown code blocks:
   - Show language label (e.g., C#, JavaScript, Python)
   - Add "Copy Code" button
   - Support line numbers
   - Support line highlighting (using ```c# {1,3-5} syntax)

3. Performance optimization:
   - Use IntersectionObserver for lazy highlighting (execute when code block enters viewport)
   - Cache highlighting results (avoid re-rendering)

4. Supported languages:
   - Common: C#, JavaScript, TypeScript, Python, Java, Go, SQL
   - Web: HTML, CSS, SCSS, JSON, YAML, Markdown
   - Shell: Bash, PowerShell

Implementation:
- Configure code highlighting extension in Markdig pipeline
- Or use JavaScript for dynamic highlighting on frontend (recommended)
- Ensure code block styles consistent with overall page design
```

---

#### 3.3 评论管理与点赞功能
**AI Prompt (中文):**
```
增强 Bin-Blog 的互动功能：

**Part 1: 点赞功能**
1. 在文章详情页添加"点赞"按钮（心形图标）
2. 创建 Like 表：
   - Id, PostId, UserId (可为空，支持匿名点赞)
   - IPAddress, CreatedAt
   - 唯一约束：同一用户/IP 只能点赞一次
3. 实现点赞逻辑：
   - 用户点击后调用 LikeService.ToggleLikeAsync
   - 更新 BlogPost.LikeCount
   - 使用 Redis 缓存点赞状态（避免频繁查询数据库）
4. 显示点赞数量和状态（已点赞/未点赞）

**Part 2: 评论管理（基于 Giscus）**
1. 当前已集成 Giscus，但可以添加：
   - 评论数量显示（通过 GitHub API 获取）
   - 在文章列表显示评论数
2. 可选：自建评论系统
   - 创建 Comment 表（Id, PostId, UserId, Content, CreatedAt, ParentId）
   - 支持嵌套回复（使用自关联外键）
   - 添加评论审核功能（IsApproved 字段）
   - Markdown 支持和表情包

**Part 3: 收藏功能**
1. 类似点赞功能，创建 Favorite 表
2. 用户可以收藏文章到"我的收藏"
3. 在个人中心显示收藏列表

技术要求：
- 点赞/收藏使用乐观锁防止并发问题
- Redis 使用 ZADD 存储点赞排行榜
- 评论支持防 XSS 攻击（输入过滤和转义）
```

**AI Prompt (English):**
```
Enhance interactive features for Bin-Blog:

**Part 1: Like Feature**
1. Add "Like" button to post detail page (heart icon)
2. Create Like table:
   - Id, PostId, UserId (nullable, support anonymous likes)
   - IPAddress, CreatedAt
   - Unique constraint: one like per user/IP
3. Implement like logic:
   - Call LikeService.ToggleLikeAsync on click
   - Update BlogPost.LikeCount
   - Use Redis to cache like status (avoid frequent DB queries)
4. Display like count and status (liked/not liked)

**Part 2: Comment Management (Giscus-based)**
1. Currently integrated Giscus, but can add:
   - Comment count display (fetch via GitHub API)
   - Show comment count in post list
2. Optional: Self-hosted comment system
   - Create Comment table (Id, PostId, UserId, Content, CreatedAt, ParentId)
   - Support nested replies (using self-referencing foreign key)
   - Add comment moderation (IsApproved field)
   - Markdown support and emoji

**Part 3: Favorite Feature**
1. Similar to like feature, create Favorite table
2. Users can save articles to "My Favorites"
3. Show favorites list in user profile

Technical requirements:
- Use optimistic locking for like/favorite to prevent concurrency issues
- Redis using ZADD to store like leaderboard
- Comments support XSS prevention (input filtering and escaping)
```

---

## 🔧 技术改进建议 / Technical Improvements

#### 4.1 性能优化
**AI Prompt:**
```
Optimize performance for Bin-Blog:
1. Enable response caching for static pages (Cache-Control headers)
2. Implement Redis caching for frequently accessed data (post list, hot posts)
3. Use lazy loading for images and components
4. Enable Brotli compression for text resources
5. Optimize database queries (add missing indexes, use AsNoTracking)
6. Implement CDN for static assets (optional)
7. Use SignalR compression for Blazor Server connections
```

---

#### 4.2 安全加固
**AI Prompt:**
```
Enhance security for Bin-Blog:
1. Implement rate limiting (use AspNetCoreRateLimit)
2. Add CAPTCHA for login/register (use Google reCAPTCHA)
3. Enable HTTPS-only cookies for authentication
4. Implement Content Security Policy (CSP) headers
5. Add SQL injection prevention (use parameterized queries, already done by EF Core)
6. Implement XSS prevention (sanitize user input)
7. Add logging and monitoring (use Serilog)
8. Regular security audits using OWASP ZAP or similar tools
```

---

#### 4.3 测试覆盖
**AI Prompt:**
```
Add comprehensive testing to Bin-Blog:
1. Unit tests for services:
   - BlogService (GetPosts, GetPostById, SearchPosts)
   - AuthService (Register, Login, ValidateToken)
   - LikeService, CommentService
2. Integration tests for database operations:
   - Test EF Core migrations
   - Test CRUD operations
3. End-to-end tests using bUnit (Blazor Unit Testing):
   - Test page rendering
   - Test user interactions
4. Use xUnit or NUnit as testing framework
5. Aim for 80%+ code coverage
6. Set up CI/CD pipeline (GitHub Actions) to run tests automatically
```

---

## 📝 使用说明 / Usage Instructions

### 如何使用这些提示词 / How to Use These Prompts

1. **选择功能** / Choose a feature：根据优先级选择要开发的功能 / Select a feature based on priority
2. **复制提示词** / Copy the prompt：复制对应的 AI 提示词（中文或英文）/ Copy the corresponding AI prompt (Chinese or English)
3. **提供给 AI** / Provide to AI：将提示词提供给 GitHub Copilot、ChatGPT 或其他 AI 助手 / Provide the prompt to GitHub Copilot, ChatGPT, or other AI assistants
4. **审查代码** / Review code：AI 生成代码后，仔细审查并测试 / After AI generates code, carefully review and test
5. **迭代优化** / Iterate：根据实际情况调整和优化 / Adjust and optimize based on actual needs

### 开发流程建议 / Recommended Development Workflow

```bash
# 1. 创建新分支 / Create new branch
git checkout -b feature/authentication

# 2. 使用 AI 提示词生成代码 / Use AI prompt to generate code

# 3. 测试功能 / Test the feature
dotnet watch

# 4. 运行数据库迁移（如有需要）/ Run migrations (if needed)
dotnet ef migrations add AddAuthenticationFeature
dotnet ef database update

# 5. 提交代码 / Commit code
git add .
git commit -m "feat: implement JWT authentication"

# 6. 合并到主分支 / Merge to main branch
git checkout main
git merge feature/authentication
```

---

## 🎨 设计原则 / Design Principles

在使用 AI 开发时，请遵循以下设计原则：
When using AI for development, follow these design principles:

1. **极简主义 / Minimalism**：保持 UI 简洁，避免过度设计 / Keep UI clean, avoid over-design
2. **性能优先 / Performance First**：优化数据库查询，使用缓存 / Optimize DB queries, use caching
3. **安全第一 / Security First**：永远不要信任用户输入 / Never trust user input
4. **可维护性 / Maintainability**：编写清晰的代码和注释 / Write clean code with comments
5. **响应式设计 / Responsive Design**：确保在移动设备上良好显示 / Ensure good display on mobile devices
6. **渐进增强 / Progressive Enhancement**：核心功能无需 JavaScript 也能工作 / Core features work without JavaScript

---

## 📚 参考资源 / Reference Resources

- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Markdig GitHub](https://github.com/xoofx/markdig)
- [Giscus](https://giscus.app/)
- [OWASP Security Guidelines](https://owasp.org/)

---

## ✨ 结语 / Conclusion

本文档提供了 Bin-Blog 项目后续开发的完整指南和 AI 提示词。建议按照优先级逐步实现各项功能，确保每个功能都经过充分测试后再进行下一步开发。

This document provides a complete guide and AI prompts for future development of the Bin-Blog project. It is recommended to implement features gradually according to priority, ensuring each feature is thoroughly tested before moving to the next.

祝开发顺利！🚀 / Happy coding! 🚀

---

**Last Updated**: 2026-02-08  
**Version**: 1.0  
**Maintainer**: Bin-Blog Development Team
