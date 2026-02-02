window.loadGiscus = (containerId, config) => {
    try {
        const container = document.getElementById(containerId);
        if (!container) return;

        // 清空容器，防止重复加载
        while (container.firstChild) container.removeChild(container.firstChild);

        const script = document.createElement('script');
        script.src = 'https://giscus.app/client.js';
        script.async = true;
        script.crossOrigin = 'anonymous';

        // 设置 giscus 所需的属性
        if (config.repo) script.setAttribute('data-repo', config.repo);
        if (config.repoId) script.setAttribute('data-repo-id', config.repoId);
        if (config.category) script.setAttribute('data-category', config.category);
        if (config.categoryId) script.setAttribute('data-category-id', config.categoryId);
        if (config.mapping) script.setAttribute('data-mapping', config.mapping);
        if (config.strict) script.setAttribute('data-strict', config.strict);
        if (config.reactionsEnabled) script.setAttribute('data-reactions-enabled', config.reactionsEnabled);
        if (config.emitMetadata) script.setAttribute('data-emit-metadata', config.emitMetadata);
        if (config.inputPosition) script.setAttribute('data-input-position', config.inputPosition);
        if (config.theme) script.setAttribute('data-theme', config.theme);
        if (config.lang) script.setAttribute('data-lang', config.lang);

        container.appendChild(script);
    } catch (e) {
        console.error('loadGiscus error', e);
    }
};
