(function($) {
    var initializeEditor = function() {
        var textAreas = $("textarea.codemirror");

        textAreas.each(function(){
            var textArea = $(this);
            var mode = textArea.data("codemirror-mode");
            var theme = textArea.data("codemirror-theme");

            var editor = CodeMirror.fromTextArea(textArea[0], {
                lineNumbers: true,
                mode: mode,
                indentUnit: 4,
                indentWithTabs: true,
                enterMode: "keep",
                tabMode: "shift",
                theme: theme,
                autoCloseTags: true
            });
        });
    };

    $(function () {
        initializeEditor();
    });
})(jQuery);