window.setDarkModeClass = function (enable) {
    if (enable) {
        document.body.classList.add('dark-mode');
    } else {
        document.body.classList.remove('dark-mode');
    }
}