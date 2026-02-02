(function() {
    function applyTheme(theme) {
        try {
            if (theme) {
                document.documentElement.setAttribute('data-theme', theme);
            }
        } catch (e) {
            console.error('Failed to apply theme', e);
        }
    }

    // On load, apply saved theme or system preference
    var saved = localStorage.getItem('theme');
    if (!saved && window.matchMedia) {
        saved = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }
    applyTheme(saved);

    window.themeManager = {
        getTheme: function() {
            return Promise.resolve(localStorage.getItem('theme') || (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'));
        },
        setTheme: function(theme) {
            try {
                localStorage.setItem('theme', theme);
                applyTheme(theme);
            } catch (e) {
                console.error('Failed to set theme', e);
            }
        }
    };
})();
